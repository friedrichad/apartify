using System;
using System.Collections.Generic;
using System.Text;

using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IUserAccountDal
    {
        IEnumerable<UserAccount> GetAllUserAccounts();
        UserAccount? GetUserAccountById(int userId);
        UserAccount? Login(string username, string password);
        void AddUserAccount(UserAccount userAccount);
        void UpdateUserAccount(UserAccount userAccount);
        void DeleteUserAccount(int userId);
        bool Save();
    }

    public class UserAccountDal : IUserAccountDal
    {
        private readonly ApartifyContext _context;

        public UserAccountDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<UserAccount> GetAllUserAccounts()
        {
            return _context.UserAccounts.ToList();
        }

        public UserAccount? GetUserAccountById(int userId)
        {
            return _context.UserAccounts.Find(userId);
        }

        public UserAccount? Login(string username, string password)
        {
            return _context.UserAccounts
                .FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public void AddUserAccount(UserAccount userAccount)
        {
            _context.UserAccounts.Add(userAccount);
            Save();
        }

        public void UpdateUserAccount(UserAccount userAccount)
        {
            _context.UserAccounts.Update(userAccount);
            Save();
        }

        public void DeleteUserAccount(int userId)
        {
            var userAccount = _context.UserAccounts.Find(userId);
            if (userAccount != null)
            {
                _context.UserAccounts.Remove(userAccount);
                Save();
            }
        }
        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}