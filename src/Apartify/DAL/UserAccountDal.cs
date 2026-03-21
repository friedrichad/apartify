using Apartify.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Apartify.DAL
{
    public interface IUserAccountDal
    {
        IEnumerable<UserAccount> GetAll();

        UserAccount? GetById(int id);
        UserAccount? GetByUsername(string username);
        void Add(UserAccount userAccount);
        void Update(UserAccount userAccount);
        void Delete(UserAccount userAccount);
        bool Save();
        
    }
    public class UserAccountDAL
    {
        private ApartifyContext context = new ApartifyContext();

        // Get all users
        public IEnumerable<UserAccount> GetAll()
        {
            return context.UserAccounts
                .Include(u => u.Roles)
                .Include(u => u.Resident)
                .Include(u => u.Staff)
                .ToList();
        }

        // Get user by id
        public UserAccount GetById(int id)
        {
            return context.UserAccounts
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.UserId == id);
        }

        // Get user by username
        public UserAccount GetByUsername(string username)
        {
            return context.UserAccounts
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Username == username);
        }

        // Insert
        public void Add(UserAccount user)
        {
            context.UserAccounts.Add(user);
            context.SaveChanges();
        }

        // Update
        public void Update(UserAccount user)
        {
            context.UserAccounts.Update(user);
            context.SaveChanges();
        }

        // Delete
        public void Delete(int id)
        {
            var user = context.UserAccounts.Find(id);
            if (user != null)
            {
                context.UserAccounts.Remove(user);
                context.SaveChanges();
            }
        }

        // Login
        public UserAccount Login(string username, string password)
        {
            return context.UserAccounts
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}