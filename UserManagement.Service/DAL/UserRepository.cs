using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Service.DTOModels;
using UserManagement.Service.Models;

namespace UserManagement.Service.DAL
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserContext _context { get; set; }
        public UserRepository(UserContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
        public async Task<User> GetFullUser(int id)
        {
            return await _context.Users
                 .Where(u => u.ID == id)
                 .Include(u => u.City)
                 .Include(u => u.Phones)
                 .Include(u => u.ContactPersons)
                 .FirstOrDefaultAsync();
        }
        public async Task<RelatedUser> GetRelatedUser(int userId, int relatedUserId)
        {
            return await _context.RelatedUsers.FirstAsync(u => u.UserID == userId && u.RelatedUserID == relatedUserId);
        }

        public async Task DeleteUser(int id)
        {
            var fullUser = await GetFullUser(id);
            _context.Users.Remove(fullUser);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RelatedUser>> GetAllRelatedUsers()
        {
            return await _context.RelatedUsers.ToListAsync();
        }
    }
}
