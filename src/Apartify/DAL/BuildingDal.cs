using System;
using System.Collections.Generic;
using System.Text;

using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IBuildingDal
    {
        IEnumerable<Building> GetAllBuildings();
        Building? GetBuildingById(int id);
        void AddBuilding(Building building);
        void UpdateBuilding(Building building);
        void DeleteBuilding(int id);
        bool Save();
    }

    public class BuildingDal : IBuildingDal
    {
        private readonly ApartifyContext _context;

        public BuildingDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<Building> GetAllBuildings()
        {
            return _context.Buildings.ToList();
        }

        public Building? GetBuildingById(int id)
        {
            return _context.Buildings
                .Include(b => b.Apartments) 
                .FirstOrDefault(b => b.BuildingId == id);
        }

        public void AddBuilding(Building building)
        {
            _context.Buildings.Add(building);
            Save();
        }

        public void UpdateBuilding(Building building)
        {
            _context.Buildings.Update(building);
            Save();
        }

        public void DeleteBuilding(int id)
        {
            var building = _context.Buildings.Find(id);
            if (building != null)
            {
                _context.Buildings.Remove(building);
                Save();
            }
        }

        public bool Save() => _context.SaveChanges() > 0;
    }
}