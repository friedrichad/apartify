using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;

namespace Apartify.BLL
{

    public interface IUserAccountBll
    {
        IEnumerable<UserAccount> GetAll();
        UserAccount? GetById(int id);
        UserAccount? Login(string username, string password);
        bool Register(UserAccount user);
        void Update(UserAccount user);
        bool Delete(int id);
    }


    public class UserAccountBll : IUserAccountBll
    {
        private readonly IUserAccountDal _userDal;


        public UserAccountBll(IUserAccountDal userDal)
        {
            _userDal = userDal;
        }

        public IEnumerable<UserAccount> GetAll()
        {
            return _userDal.GetAllUserAccounts();
        }

        public UserAccount? GetById(int id)
        {
            return _userDal.GetUserAccountById(id);
        }

        public UserAccount? Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            return _userDal.Login(username, password);
        }


        public bool Register(UserAccount user)
        {
    
            var allUsers = _userDal.GetAllUserAccounts();
            bool isExisted = allUsers.Any(u => u.Username == user.Username);

            if (isExisted)
            {
                return false; 
            }

            _userDal.AddUserAccount(user);
            return true;
        }

        public void Update(UserAccount user)
        {
            _userDal.UpdateUserAccount(user);
        }

        public bool Delete(int id)
        {
            var user = _userDal.GetUserAccountById(id);
            if (user == null) return false;

            _userDal.DeleteUserAccount(id);
            return true;
        }
    }
}