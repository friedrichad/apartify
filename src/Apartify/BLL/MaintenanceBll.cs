using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;

namespace Apartify.BLL
{
    public interface IMaintenanceBll
    {
        IEnumerable<Maintenance> GetList();
        Maintenance? GetDetail(int id);
        void CreateRequest(Maintenance maintenance);
        void UpdateStatus(int id, string status);
        bool Remove(int id);
    }

    public class MaintenanceBll : IMaintenanceBll
    {
        private readonly IMaintenanceDal _maintenanceDal;

        public MaintenanceBll(IMaintenanceDal maintenanceDal)
        {
            _maintenanceDal = maintenanceDal;
        }

        public IEnumerable<Maintenance> GetList() => _maintenanceDal.GetAll();

        public Maintenance? GetDetail(int id) => _maintenanceDal.GetById(id);

        public void CreateRequest(Maintenance maintenance)
        {
            maintenance.Status = "Pending";
            maintenance.ReportDate = DateOnly.FromDateTime(DateTime.Now);
            _maintenanceDal.Add(maintenance);
        }

        public void UpdateStatus(int id, string status)
        {
            var item = _maintenanceDal.GetById(id);
            if (item != null)
            {
                item.Status = status;
                _maintenanceDal.Update(item);
            }
        }

        public bool Remove(int id)
        {
            var item = _maintenanceDal.GetById(id);
            if (item == null) return false;

            _maintenanceDal.Delete(id);
            return true;
        }
    }
}