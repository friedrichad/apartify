using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;

namespace Apartify.BLL
{
    public interface IStaffBll
    {
        IEnumerable<Staff> GetList();
        Staff? GetDetail(int id);
        bool Create(Staff staff);
        void Edit(Staff staff);
        string Remove(int id);
    }

    public class StaffBll : IStaffBll
    {
        private readonly IStaffDal _staffDal;

        public StaffBll(IStaffDal staffDal)
        {
            _staffDal = staffDal;
        }

        public IEnumerable<Staff> GetList() => _staffDal.GetAll();

        public Staff? GetDetail(int id) => _staffDal.GetById(id);

        public bool Create(Staff staff)
        {
            if (string.IsNullOrEmpty(staff.FullName)) return false;

            _staffDal.Add(staff);
            return true;
        }

        public void Edit(Staff staff)
        {
            _staffDal.Update(staff);
        }

        public string Remove(int id)
        {
            var staff = _staffDal.GetById(id);
            if (staff == null) return "Không tìm thấy nhân viên!";

   
            if (staff.Maintenances.Any())
            {
                return "Không thể xóa nhân viên này vì họ đang có lịch sử bảo trì!";
            }

            _staffDal.Delete(id);
            return "Success";
        }
    }
}