using System;
using System.Collections.Generic;
using System.Text;
using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IApartmentDal
    {
        IEnumerable<Apartment> GetAllApartments();
        Apartment? GetApartmentById(int id);
        void Add(Apartment apartment);
        void Update(Apartment apartment);
        void Delete(int id);
        bool Save();
    }

    public class ApartmentDal : IApartmentDal
    {
        private readonly ApartifyContext _context;

        public ApartmentDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<Apartment> GetAllApartments()
        {

            return _context.Apartments.Include(a => a.Building).ToList();
        }

        public Apartment? GetApartmentById(int id)
        {
            return _context.Apartments
                .Include(a => a.Building)
                .FirstOrDefault(a => a.ApartmentId == id);
        }

        public void Add(Apartment apartment)
        {
            _context.Apartments.Add(apartment);
            Save();
        }

        public void Update(Apartment apartment)
        {
            _context.Apartments.Update(apartment);
            Save();
        }

        public void Delete(int id)
        {
            var apt = _context.Apartments.Find(id);
            if (apt != null)
            {
                _context.Apartments.Remove(apt);
                Save();
            }
        }

        public bool Save() => _context.SaveChanges() > 0;
    }
}