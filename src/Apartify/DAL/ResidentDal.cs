using System;
using System.Collections.Generic;
using System.Text;

using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IResidentDal
    {
        IEnumerable<Resident> GetAllResidents();
        Resident? GetResidentById(int id);
        void Add(Resident resident);
        void Update(Resident resident);
        void Delete(int id);
        bool Save();
    }

    public class ResidentDal : IResidentDal
    {
        private readonly ApartifyContext _context;

        public ResidentDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<Resident> GetAllResidents()
        {
            return _context.Residents.ToList();
        }

        public Resident? GetResidentById(int id)
        {
            return _context.Residents.FirstOrDefault(r => r.ResidentId == id);
        }

        public void Add(Resident resident)
        {
            _context.Residents.Add(resident);
        }

        public void Update(Resident resident)
        {
            _context.Residents.Update(resident);
        }

        public void Delete(int id)
        {
            var resident = _context.Residents.Find(id);
            if (resident != null)
            {
                _context.Residents.Remove(resident);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}