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

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _service { get; set; }
        public UserContext _userContext { get; set; }

        public UserController(IUserRepository repo, IHostingEnvironment hostingEnv, UserContext context, ILogger logger)
        {
            _service = new UserService(repo,hostingEnv, logger);
            _userContext = context;
        }

        [HttpGet("odata")]
        public IActionResult Get(ODataQueryOptions<User> query)
        {
            var user = this._userContext.Users.AsQueryable();
    
            return Ok(new
            {
                Items = query.ApplyTo(user),
                Count = query.Count?.GetEntityCount(query.Filter?.ApplyTo(user, new ODataQuerySettings()) ?? user)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(BasicUserDTO user)
        {
            await _service.CreateUser(user);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser(int id, BasicUserDTO userInfo)
        {
            var userIdIsValid = await _service.CheckUserById(id);
            if (!userIdIsValid)
                return BadRequest("Provided Id is invalid");
            await _service.UpdateUserBasicInfo(id, userInfo);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult>GetUser(int id)
        {
            var userIdIsValid = await _service.CheckUserById(id);
            if (!userIdIsValid)
                return BadRequest("User was not found with the provided Id");
            var result = await _service.GetfullUser(id);
            
            return Ok(result);
        }

        [HttpPost]
        [Route("RelatedUser")]
        public async Task<IActionResult> UpsertContactPerson(int userId, RelatedUserDto relatedUserDto)
        {
            await _service.UpsertRelatedUser(userId, relatedUserDto);
            return Ok();
        }

        [HttpDelete]
        [Route("RelatedUser")]
        public async Task<IActionResult> RemoveContactPerson(int userId, int relatedUserId)
        {
            await _service.DeleteRelatedUser(userId, relatedUserId);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _service.DeleteUser(id);
            return Ok();
        }

        [HttpPost]
        [Route("UserImage")]
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

        [HttpGet]
        [Route("RelationsReport")]
        public async Task<IActionResult> GetReport()
        {
            var report = await _service.GetRelationReport();
            return Ok(report);
        }
      
    }
}