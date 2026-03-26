using System.Collections.Generic;
using System.Linq;
using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IBuildingDal
    {
        IEnumerable<Building> GetAll();
        Building? GetById(int id);
        void Add(Building building);
        void Update(Building building);
        void Delete(int id);
        bool Save();
    }

    public class BuildingDal : IBuildingDal
    {
        private readonly ApartifyContext _context;

        public BuildingDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<Building> GetAll()
        {
            return _context.Buildings
                .Include(b => b.Apartments)
                .ToList();
        }

        public Building? GetById(int id)
        {
            return _context.Buildings
                .Include(b => b.Apartments)
                .FirstOrDefault(b => b.BuildingId == id);
        }

        public void Add(Building building)
        {
            _context.Buildings.Add(building);
        }

        public void Update(Building building)
        {
            _context.Buildings.Update(building);
        }

        public void Delete(int id)
        {
            var building = _context.Buildings.Find(id);
            if (building != null)
            {
                _context.Buildings.Remove(building);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}