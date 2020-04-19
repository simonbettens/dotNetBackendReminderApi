using Microsoft.EntityFrameworkCore;
using ReminderApi.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReminderApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly ReminderDbContext dbContext;
        public readonly DbSet<User> users;
        public UserRepository(ReminderDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.users = dbContext.User;
        }

        public void Add(User user)
        {
            users.Add(user);
        }

        public User GetBy(string email)
        {
            return users.FirstOrDefault(u=>u.Email.Equals(email));
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}
