using System.Collections.Generic;
using System.Linq;
using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IStaffDal
    {
        IEnumerable<Staff> GetAll();
        Staff? GetById(int id);
        void Add(Staff staff);
        void Update(Staff staff);
        void Delete(int id);
        bool Save();
    }

    public class StaffDal : IStaffDal
    {
        private readonly ApartifyContext _context;

        public StaffDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<Staff> GetAll()
        {
            return _context.Staff
                .Include(s => s.Building)
                .Include(s => s.User)
                .ToList();
        }

        public Staff? GetById(int id)
        {
            return _context.Staff
                .Include(s => s.Building)
                .Include(s => s.User)
                .FirstOrDefault(s => s.StaffId == id);
        }

        public void Add(Staff staff)
        {
            _context.Staff.Add(staff);
        }

        public void Update(Staff staff)
        {
            _context.Staff.Update(staff);
        }

        public void Delete(int id)
        {
            var staff = _context.Staff.Find(id);
            if (staff != null)
            {
                _context.Staff.Remove(staff);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}