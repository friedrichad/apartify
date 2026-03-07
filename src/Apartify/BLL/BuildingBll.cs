using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;

namespace Apartify.BLL
{
    public interface IBuildingBll
    {
        IEnumerable<Building> GetList();
        Building? GetDetail(int id);
        bool Create(Building building);
        void Edit(Building building);
        bool Remove(int id);
    }

    public class BuildingBll : IBuildingBll
    {
        private readonly IBuildingDal _buildingDal;

        public BuildingBll(IBuildingDal buildingDal)
        {
            _buildingDal = buildingDal;
        }

        public IEnumerable<Building> GetList() => _buildingDal.GetAllBuildings();

        public Building? GetDetail(int id) => _buildingDal.GetBuildingById(id);

        public bool Create(Building building)
        {

            if (string.IsNullOrWhiteSpace(building.Name)) return false;

            _buildingDal.AddBuilding(building);
            return true;
        }

        public void Edit(Building building)
        {
            _buildingDal.UpdateBuilding(building);
        }

        public bool Remove(int id)
        {
            var building = _buildingDal.GetBuildingById(id);
            if (building == null) return false;

            if (building.Apartments.Any() || building.Staff.Any())
            {
                return false;
            }

            _buildingDal.DeleteBuilding(id);
            return true;
        }
    }
}