using System;
using System.Collections.Generic;
using System.Text;

using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IResidentDal
    {
        IEnumerable<Resident> GetAll();
        Resident? GetById(int id);
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

        public IEnumerable<Resident> GetAll()
        {

            return _context.Residents.Include(r => r.User).ToList();
        }

        public Resident? GetById(int id)
        {
            return _context.Residents
                .Include(r => r.User)
                .Include(r => r.Contracts) 
                .FirstOrDefault(r => r.ResidentId == id);
        }

        public void Add(Resident resident)
        {
            _context.Residents.Add(resident);
            Save();
        }

        public void Update(Resident resident)
        {
            _context.Residents.Update(resident);
            Save();
        }

        public void Delete(int id)
        {
            var res = _context.Residents.Find(id);
            if (res != null)
            {
                _context.Residents.Remove(res);
                Save();
            }
        }

        public bool Save() => _context.SaveChanges() > 0;
    }
}