using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Service.Models;

namespace UserManagement.Service.DAL
{
    public class UserRepository:GenericRepository<User>,IUserRepository
    {
        public UserRepository(UserContext dbContext):base(dbContext){}
    }
}
