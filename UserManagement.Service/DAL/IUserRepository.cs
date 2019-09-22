using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Service.Models;

namespace UserManagement.Service.DAL
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetFullUser(int id);
        Task<RelatedUser> GetRelatedUser(int userId, int relatedUserId);
        Task DeleteUser(int id);
        Task<List<RelatedUser>> GetAllRelatedUsers();
    }
}
