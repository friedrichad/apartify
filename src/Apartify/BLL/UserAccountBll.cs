using Apartify.Models;
using System;
using System.Collections.Generic;
using Apartify.BLL.Helpers;
using Apartify.DAL;

namespace Apartify.BLL
{
    public interface IUserAccountBll
    {
        IEnumerable<UserAccount> GetList();
        UserAccount? GetDetail(int id);
        UserAccount? Login(string username, string password);
        bool Create(UserAccount user);
        bool Edit(UserAccount user);
        bool Remove(int id);
        bool ChangeStatus(int userId, int status);
    }

    public class UserAccountBll : IUserAccountBll
    {
        private readonly IUserAccountDal _userAccountDal;

        public UserAccountBll(IUserAccountDal userAccountDal)
        {
            _userAccountDal = userAccountDal;
        }

        // Lấy tất cả user
        public IEnumerable<UserAccount> GetList()
        {
            return _userAccountDal.GetAll();
        }

        // Lấy user theo id
        public UserAccount? GetDetail(int id)
        {
            return _userAccountDal.GetById(id);
        }

        // Login
        public UserAccount? Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Username or Password cannot be empty");
            }

            var user = _userAccountDal.GetByUsername(username);
            if (user != null && user.Password == password) // Should ideally hash password
            {
                return user;
            }
            return null;
        }

        // Tạo user
        public bool Create(UserAccount user)
        {
            ValidateHelper.ValidateUserAccount(user);

            if (_userAccountDal.GetByUsername(user.Username) != null)
                throw new Exception("Username already exists");

            _userAccountDal.Add(user);
            return _userAccountDal.Save();
        }

        // Update user
        public bool Edit(UserAccount user)
        {
            ValidateHelper.ValidateUserAccount(user);

            _userAccountDal.Update(user);
            return _userAccountDal.Save();
        }

        // Xóa user
        public bool Remove(int id)
        {
            _userAccountDal.Delete(id);
            return _userAccountDal.Save();
        }

        // Khóa / mở khóa tài khoản
        public bool ChangeStatus(int userId, int status)
        {
            var user = _userAccountDal.GetById(userId);

            if (user == null)
                throw new Exception("User does not exist");

            user.Status = status;

            _userAccountDal.Update(user);
            return _userAccountDal.Save();
        }
    }
}