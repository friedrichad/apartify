using System;
using System.Collections.Generic;
using System.Text;

using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IMaintenanceDal
    {
        IEnumerable<Maintenance> GetAll();
        Maintenance? GetById(int id);
        void Add(Maintenance maintenance);
        void Update(Maintenance maintenance);
        void Delete(int id);
        bool Save();
    }

    public class MaintenanceDal : IMaintenanceDal
    {
        private readonly ApartifyContext _context;

        public MaintenanceDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<Maintenance> GetAll()
        {
            return _context.Maintenances
                .Include(m => m.Apartment)
                .Include(m => m.Staff)
                .ToList();
        }

        public Maintenance? GetById(int id)
        {
            return _context.Maintenances
                .Include(m => m.Apartment)
                .Include(m => m.Staff)
                .FirstOrDefault(m => m.MaintenanceId == id);
        }

        public void Add(Maintenance maintenance)
        {
            _context.Maintenances.Add(maintenance);
            Save();
        }

        public void Update(Maintenance maintenance)
        {
            _context.Maintenances.Update(maintenance);
            Save();
        }

        public void Delete(int id)
        {
            var item = _context.Maintenances.Find(id);
            if (item != null)
            {
                _context.Maintenances.Remove(item);
                Save();
            }
        }

        public bool Save() => _context.SaveChanges() > 0;
    }
}