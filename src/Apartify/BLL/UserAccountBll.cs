using Apartify.DAL;
using Apartify.Models;
using System;
using System.Collections.Generic;

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
        public UserAccount GetUserById(int id)
        {
            return dal.GetById(id);
        }

        // Login
        public UserAccount Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Username hoặc Password không được để trống");
            }

            return dal.Login(username, password);
        }

        // Tạo user
        public void CreateUser(UserAccount user)
        {
            if (string.IsNullOrEmpty(user.Username))
                throw new Exception("Username không được để trống");

            if (string.IsNullOrEmpty(user.Password))
                throw new Exception("Password không được để trống");

            dal.Add(user);
        }

        // Update user
        public void UpdateUser(UserAccount user)
        {
            if (user.UserId <= 0)
                throw new Exception("User không hợp lệ");

            dal.Update(user);
        }

        // Xóa user
        public void DeleteUser(int id)
        {
            if (id <= 0)
                throw new Exception("Id không hợp lệ");

            dal.Delete(id);
        }

        // Khóa / mở khóa tài khoản
        public void ChangeStatus(int userId, int status)
        {
            var user = dal.GetById(userId);

            if (user == null)
                throw new Exception("User không tồn tại");

            user.Status = status;

            dal.Update(user);
        }
    }
}