using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Service.Models;

namespace UserManagement.Service.DAL
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City
                {
                    ID = 1,
                    Name = "TBilisi"
                },
                new City
                {
                    ID = 2,
                    Name = "Batumi"
                },
                new City
                {
                    ID = 3,
                    Name = "Kutaisi"
                });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<RelatedUser> RelatedUsers { get; set; }
    }
}
