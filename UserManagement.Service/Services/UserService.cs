using System;
using System.Collections.Generic;
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

        public async Task CreateUser(UserDTO userDto)
        {

        }
    }
}
