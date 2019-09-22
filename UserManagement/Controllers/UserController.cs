﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagement.Helpers;
using UserManagement.Service.DAL;
using UserManagement.Service.DTOModels;
using UserManagement.Service.Models;
using UserManagement.Service.Services;
using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;
using ILogger = UserManagement.Service.Infrastructure.ILogger;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _service { get; set; }
        private ImageHelper _imageHelper { get; set; }
        public UserContext _userContext { get; set; }
        public ILogger _logger { get; set; }

        public UserController(IUserRepository repo, IHostingEnvironment hostingEnv, UserContext context, ILogger logger)
        {
            _service = new UserService(repo);
            _imageHelper = new ImageHelper(hostingEnv);
            _userContext = context;
            _logger = logger;
            _logger.LogMessage("test", LogLevel.Warning);
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
                return BadRequest("Provided Id is invalid");
            var result = await _service.GetfullUser(id);
            if (!string.IsNullOrEmpty(result.RelativePath))
                result.ImageBase64 = _imageHelper.GetImageFromDisk(result.RelativePath);
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
                var relativePath = await _imageHelper.SaveImageToDisk(userId, file);
                await _service.UpdateUserPicture(userId, relativePath);
            }
            return Ok();
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