using System.Collections.Generic;
using Apartify.DAL;
using Apartify.Models;
using Apartify.BLL.Helpers;

namespace Apartify.BLL
{
    public interface IStaffBll
    {
        IEnumerable<Staff> GetAllStaff();
        Staff? GetStaffById(int id);
        bool AddStaff(Staff staff);
        bool UpdateStaff(Staff staff);
        bool DeleteStaff(int id);
    }

    public class StaffBll : IStaffBll
    {
        private readonly IStaffDal _staffDal;

        public StaffBll(IStaffDal staffDal)
        {
            _staffDal = staffDal;
        }

        public IEnumerable<Staff> GetAllStaff()
        {
            return _staffDal.GetAll();
        }

        public Staff? GetStaffById(int id)
        {
            return _staffDal.GetById(id);
        }

        public bool AddStaff(Staff staff)
        {
            ValidateHelper.ValidateStaff(staff);

            _staffDal.Add(staff);
            return _staffDal.Save();
        }

        public bool UpdateStaff(Staff staff)
        {
            ValidateHelper.ValidateStaff(staff);

            _staffDal.Update(staff);
            return _staffDal.Save();
        }

        public bool DeleteStaff(int id)
        {
            _staffDal.Delete(id);
            return _staffDal.Save();
        }
    }
}