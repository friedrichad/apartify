using Apartify.Models;
using System;
using System.Collections.Generic;
using Apartify.BLL.Helpers;

namespace Apartify.BLL
{
    public interface IUserAccountBll
    {
        IEnumerable<UserAccount> GetAllUsers();
        UserAccount? GetUserById(int id);
        UserAccount? Login(string username, string password);
        void CreateUser(UserAccount user);
        void UpdateUser(UserAccount user);
        void DeleteUser(int id);
        void ChangeStatus(int userId, int status);
    }
    public class UserAccountBLL: IUserAccountBll
    {
        private UserAccountDAL dal = new UserAccountDAL();

        // Lấy tất cả user
        public IEnumerable<UserAccount> GetAllUsers()
        {
            return dal.GetAll();
        }

        // Lấy user theo id
        public UserAccount? GetUserById(int id)
        {
            return dal.GetById(id);
        }

        // Login
        public UserAccount? Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Username or Password cannot be empty");
            }
            if (username.Length > 50 || password.Length > 100)
            {
                return null;
            }

            return dal.Login(username, password);
        }

        // Tạo user
        public void CreateUser(UserAccount user)
        {
            ValidateHelper.ValidateUserAccount(user);

            if (dal.GetByUsername(user.Username) != null)
                throw new Exception("Username already exists");

            dal.Add(user);
        }

        // Update user
        public void UpdateUser(UserAccount user)
        {
            ValidateHelper.ValidateUserAccount(user);

            dal.Update(user);
        }

        // Xóa user
        public void DeleteUser(int id)
        {
            dal.Delete(id);
        }

        // Khóa / mở khóa tài khoản
        public void ChangeStatus(int userId, int status)
        {
            var user = dal.GetById(userId);

            if (user == null)
                throw new Exception("User does not exist");

            user.Status = status;

            dal.Update(user);
        }
    }
}