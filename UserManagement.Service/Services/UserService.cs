using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Service.DAL;
using UserManagement.Service.DTOModels;
using UserManagement.Service.Helpers;
using UserManagement.Service.Infrastructure;
using UserManagement.Service.Models;


namespace UserManagement.Service.Services
{
    public class UserService
    {
        private IUserRepository _repo { get; set; }
        private ILogger _logger { get; set; }
        private ImageHelper _imgHelper { get; set; }

        public UserService(IUserRepository repo, IHostingEnvironment hostingEnvironment, ILogger logger)
        {
            _repo = repo;
            _logger = logger;
            _imgHelper = new ImageHelper(hostingEnvironment);
        }

        public async Task CreateUser(BasicUserDTO userDto)
        {
            try
            {
                var user = new User
                {
                    Firstname = userDto.Firstname,
                    Lastname = userDto.Lastname,
                    Gender = userDto.Gender,
                    DateOfBirth = userDto.DateOfBirth.Date,
                    IdentificationNumber = userDto.IdentificationNumber,
                    CityID = userDto.CityID,
                    PictureRelativePath = "",
                    Phones = new List<Phone>(),
                    ContactPersons = new List<RelatedUser>()
                };
                foreach (var item in userDto.Phones)
                {
                    var phone = new Phone
                    {
                        Type = item.Type,
                        Number = item.Number
                    };
                    user.Phones.Add(phone);
                }
                await _repo.Create(user);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw e;
            }

        }

        public async Task<bool> CheckUserById(int id)
        {
            try
            {
                var user = await _repo.GetById(id);
                return user != null;
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw e;
            }

        }

        public async Task UpdateUserPicture(int id, IFormFile file)
        {
            try
            {
                var relativePath = await _imgHelper.SaveImageToDisk(id, file);
                var user = await _repo.GetById(id);
                user.PictureRelativePath = relativePath;
                await _repo.Update(id, user);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw e;
            }
            
        }

        public async Task UpdateUserBasicInfo(int id, BasicUserDTO userDto)
        {
            try
            {
                var user = await _repo.GetFullUser(id);
                if (user != null)
                {
                    user.Firstname = userDto.Firstname;
                    user.Lastname = userDto.Lastname;
                    user.Gender = userDto.Gender;
                    user.DateOfBirth = userDto.DateOfBirth.Date;
                    user.IdentificationNumber = userDto.IdentificationNumber;
                    user.CityID = userDto.CityID;
                }
                foreach (var item in userDto.Phones)
                {
                    var phone = new Phone
                    {
                        Type = item.Type,
                        Number = item.Number
                    };
                    user.Phones.Add(phone);
                }
                await _repo.Update(id, user);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw e;
            }
        }

        public async Task UpsertRelatedUser(int id, RelatedUserDto relatedUserDto)
        {
            try
            {
                var userIdIsValid = await CheckUserById(id);
                if (!userIdIsValid)
                    return;
                var relatedUserIdIsValid = await CheckUserById(relatedUserDto.RelatedUserId);
                if (!relatedUserIdIsValid)
                    return;
                var fullUser = await _repo.GetFullUser(id);
                if (fullUser.ContactPersons == null)
                    fullUser.ContactPersons = new List<RelatedUser>();
                if (!fullUser.ContactPersons.Any(u => u.RelatedUserID == relatedUserDto.RelatedUserId))
                    fullUser.ContactPersons.Add(new RelatedUser { RelationType = relatedUserDto.RelationType, UserID = id, RelatedUserID = relatedUserDto.RelatedUserId });
                else
                {
                    var contactPerson = fullUser.ContactPersons.First(u => u.RelatedUserID == relatedUserDto.RelatedUserId);
                    contactPerson.RelationType = relatedUserDto.RelationType;
                }
                await _repo.Update(id, fullUser);
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw e;
            }

        }

        public async Task DeleteRelatedUser(int id, int relatedUserId)
        {
            try
            {
                var userIdIsValid = await CheckUserById(id);
                if (!userIdIsValid)
                    return;
                var fullUser = await _repo.GetFullUser(id);
                if (fullUser.ContactPersons == null)
                    return;
                if (!fullUser.ContactPersons.Any(u => u.RelatedUserID == relatedUserId))
                    return;
                var relatedUserIdIsValid = await CheckUserById(relatedUserId);
                if (!relatedUserIdIsValid)
                    return;
                else
                {
                    var relatedUserObject = await _repo.GetRelatedUser(id, relatedUserId);
                    fullUser.ContactPersons.Remove(relatedUserObject);
                    await _repo.Update(id, fullUser);
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw e;
            }

        }

        public async Task DeleteUser(int id)
        {
            try
            {
                var userIdIsValid = await CheckUserById(id);
                if (userIdIsValid)
                    await _repo.DeleteUser(id);

            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw e;
            }
        }

        public async Task<FullUserDTO> GetfullUser(int id)
        {
            try
            {
                var fullUser = await _repo.GetFullUser(id);
                var fullUserDto = new FullUserDTO
                {
                    ID = id,
                    Firstname = fullUser.Firstname,
                    Lastname = fullUser.Lastname,
                    DateOfBirth = fullUser.DateOfBirth.Date,
                    Gender = fullUser.Gender,
                    IdentificationNumber = fullUser.IdentificationNumber,
                    CityID = fullUser.CityID,
                    RelativePath = fullUser.PictureRelativePath,
                    RelatedUsers = new List<RelatedUserDto>(),
                    Phones = new List<PhoneDTO>()
                };
                if (fullUser.ContactPersons != null)
                {
                    foreach (var item in fullUser.ContactPersons)
                    {
                        fullUserDto.RelatedUsers.Add(new RelatedUserDto
                        {
                            RelatedUserId = item.RelatedUserID,
                            RelationType = item.RelationType
                        });
                    }
                }
                if (fullUser.Phones != null)
                {
                    foreach (var item in fullUser.Phones)
                    {
                        fullUserDto.Phones.Add(new PhoneDTO
                        {
                            Type = item.Type,
                            Number = item.Number
                        });
                    }
                }
                if (!string.IsNullOrEmpty(fullUser.PictureRelativePath))
                    fullUserDto.ImageBase64 = _imgHelper.GetImageFromDisk(fullUser.PictureRelativePath);
                return fullUserDto;
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw e;
            }

        }

        public async Task<List<RelatedPersonReportDTO>> GetRelationReport()
        {
            try
            {
                var result = new List<RelatedPersonReportDTO>();
                var relations = await _repo.GetAllRelatedUsers();
                var groupedRelations = relations.GroupBy(u => new { u.UserID, u.RelationType }).GroupBy(u => u.Key.UserID).ToList();
                foreach (var userGroup in groupedRelations)
                {
                    var resultItem = new RelatedPersonReportDTO
                    {
                        UserID = userGroup.Key,
                        Relations = new Dictionary<RelationType, int>()
                    };
                    foreach (var ralationGroup in userGroup)
                    {
                        resultItem.Relations.Add(ralationGroup.Key.RelationType, ralationGroup.Count());
                    }
                    result.Add(resultItem);
                }
                return result;
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw e;
            }

        }
    }
}
