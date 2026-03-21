using System.Collections.Generic;
using Apartify.DAL;
using Apartify.Models;

namespace Apartify.BLL
{
    public interface IStaffService
    {
        IEnumerable<Staff> GetAllStaff();
        Staff? GetStaffById(int id);
        bool AddStaff(Staff staff);
        bool UpdateStaff(Staff staff);
        bool DeleteStaff(int id);
    }

    public class StaffService : IStaffService
    {
        private readonly IStaffDal _staffDal;

        public StaffService(IStaffDal staffDal)
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
            _staffDal.Add(staff);
            return _staffDal.Save();
        }

        public bool UpdateStaff(Staff staff)
        {
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