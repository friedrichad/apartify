using System.Collections.Generic;
using System.Linq;
using Apartify.DAL;
using Apartify.Models;
using Apartify.BLL.Helpers;

namespace Apartify.BLL
{
    public interface IBuildingBll
    {
        IEnumerable<Building> GetList();
        Building? GetDetail(int id);
        bool Create(Building building);
        bool Edit(Building building);
        bool Remove(int id);
    }

    public class BuildingBll : IBuildingBll
    {
        private readonly IBuildingDal _buildingDal;

        public BuildingBll(IBuildingDal buildingDal)
        {
            _buildingDal = buildingDal;
        }

        public IEnumerable<Building> GetList()
        {
            return _buildingDal.GetAll();
        }

        public Building? GetDetail(int id)
        {
            return _buildingDal.GetById(id);
        }

        public bool Create(Building building)
        {
            ValidateHelper.ValidateBuilding(building);

            _buildingDal.Add(building);
            return _buildingDal.Save();
        }

        public bool Edit(Building building)
        {
            ValidateHelper.ValidateBuilding(building);

            var existing = _buildingDal.GetById(building.BuildingId);
            if (existing == null) return false;

            _buildingDal.Update(building);
            return _buildingDal.Save();
        }

        public bool Remove(int id)
        {
            var building = _buildingDal.GetById(id);
            if (building == null) return false;

            if (building.Apartments.Any())
                return false;

            _buildingDal.Delete(id);
            return _buildingDal.Save();
        }
    }
}