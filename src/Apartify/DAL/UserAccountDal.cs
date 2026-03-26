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
        void Delete(int id);
        bool Save();
    }

    public class UserAccountDal : IUserAccountDal
    {
        private readonly ApartifyContext _context;

        public UserAccountDal(ApartifyContext context)
        {
            _context = context;
        }

        // Get all users
        public IEnumerable<UserAccount> GetAll()
        {
            return _context.UserAccounts
                .Include(u => u.Resident)
                .ToList();
        }

        // Get user by id
        public UserAccount? GetById(int id)
        {
            return _context.UserAccounts
                .Include(u => u.Resident)
                .FirstOrDefault(u => u.UserId == id);
        }

        // Get user by username
        public UserAccount? GetByUsername(string username)
        {
            return _context.UserAccounts
                .Include(u => u.Resident)
                .FirstOrDefault(u => u.Username == username);
        }

        // Insert
        public void Add(UserAccount user)
        {
            _context.UserAccounts.Add(user);
        }

        // Update
        public void Update(UserAccount user)
        {
            _context.UserAccounts.Update(user);
        }

        // Delete
        public void Delete(int id)
        {
            var user = _context.UserAccounts.Find(id);
            if (user != null)
            {
                _context.UserAccounts.Remove(user);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}