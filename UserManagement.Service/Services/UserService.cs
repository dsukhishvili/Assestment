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
using UserManagement.Service.Models;

namespace UserManagement.Service.Services
{
    public class UserService
    {
        private IUserRepository _repo { get; set; }
        public UserService(IUserRepository repo)
        {
            _repo = repo;
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

                throw e;
            }

        }

        public async Task<bool> CheckUserById(int id)
        {
            var user = await _repo.GetById(id);
            return user != null;
        }

        public async Task UpdateUserPicture(int id, string path)
        {
            var user = await _repo.GetById(id);
            user.PictureRelativePath = path;
            await _repo.Update(id, user);
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

                throw e;
            }
        }

        public async Task UpsertRelatedUser(int id, RelatedUserDto relatedUserDto)
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

        public async Task DeleteRelatedUser(int id, int relatedUserId)
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

                throw e;
            }
        }

        public async Task<FullUserDTO> GetfullUser(int id)
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
            if(fullUser.ContactPersons != null)
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
            if(fullUser.Phones != null)
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
            return fullUserDto;
        }
    }
}
