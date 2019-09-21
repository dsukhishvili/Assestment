using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Service.DAL;
using UserManagement.Service.DTOModels;
using UserManagement.Service.Services;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _service { get; set; }
        public UserController(IUserRepository repo)
        {
            _service = new UserService(repo); 
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserDTO user)
        {
            await _service.CreateUser(user);
            return Ok();
        }
    }
}