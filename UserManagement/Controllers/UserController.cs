using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Service.DAL;
using UserManagement.Service.DTOModels;
using UserManagement.Service.Models;
using UserManagement.Service.Services;
using UserManagement.Service.Infrastructure;
using AutoMapper;
using UserManagement.ViewModels;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _service { get; set; }
        private readonly IMapper _mapper;

        public UserController(IUserRepository repo, IHostingEnvironment hostingEnv,
            ILogger logger, IMapper mapper)
        {
            _service = new UserService(repo,hostingEnv, logger);
            _mapper = mapper;
        }

        [HttpGet("odata")]
        public IActionResult Get(ODataQueryOptions<User> query)
        {
            var result = _service.QueryUser(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BasicUserViewModel user)
        {
            var userDto = _mapper.Map<BasicUserDTO>(user);
            await _service.CreateUser(userDto);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id,[FromBody]BasicUserViewModel userInfo)
        {
            var userIdIsValid = await _service.CheckUserById(id);
            if (!userIdIsValid)
                return BadRequest("Provided Id is invalid");
            var userDto = _mapper.Map<BasicUserDTO>(userInfo);
            await _service.UpdateUserBasicInfo(id, userDto);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetUser(int id)
        {
            var userIdIsValid = await _service.CheckUserById(id);
            if (!userIdIsValid)
                return BadRequest("User was not found with the provided Id");
            var fullUserDto = await _service.GetfullUser(id);
            var userVm = _mapper.Map<FullUserViewModel>(fullUserDto);
            return Ok(userVm);
        }

        [HttpPost("{userId}/RelatedUser")]
        public async Task<IActionResult> UpsertContactPerson(int userId, RelatedUserViewModel relatedUserVM)
        {
            var relatedUserDto = _mapper.Map<RelatedUserDto>(relatedUserVM);
            await _service.UpsertRelatedUser(userId, relatedUserDto);
            return Ok();
        }

        [HttpDelete("{userId}/RelatedUsers/{relatedUserId}")]
        public async Task<IActionResult> RemoveContactPerson(int userId, int relatedUserId)
        {
            await _service.DeleteRelatedUser(userId, relatedUserId);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _service.DeleteUser(id);
            return Ok();
        }

        [HttpPost("{userId}/UserImage")]
        public async Task<IActionResult> UploadImage(int userId, IFormFile file)
        {
            var userIdIsValid = await _service.CheckUserById(userId);
            if (!userIdIsValid)
                return BadRequest("Provided user id was invalid");

            if (file.ContentType == "image/jpeg")
            {
                await _service.UpdateUserPicture(userId, file);
                return Ok();
            }
            else
                return BadRequest("Invalid image format, should be .jpeg");
        }      
    }
}