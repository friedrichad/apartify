using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;
using Apartify.BLL.Helpers;

namespace Apartify.BLL
{
    public interface IResidentService
    {
        IEnumerable<Resident> GetAllResidents();
        Resident? GetResidentById(int id);
        bool AddResident(Resident resident);
        bool UpdateResident(Resident resident);
        bool DeleteResident(int id);
    }
    public class ResidentService : IResidentService
    {
        private readonly IResidentDal _residentDal;

        public ResidentService(IResidentDal residentDal)
        {
            _residentDal = residentDal;
        }

        public IEnumerable<Resident> GetAllResidents()
        {
            return _residentDal.GetAllResidents();
        }

        public Resident? GetResidentById(int id)
        {
            return _residentDal.GetResidentById(id);
        }

        public bool AddResident(Resident resident)
        {
            ValidateHelper.ValidateResident(resident);

            _residentDal.Add(resident);
            return _residentDal.Save();
        }

        public bool UpdateResident(Resident resident)
        {
            ValidateHelper.ValidateResident(resident);

            _residentDal.Update(resident);
            return _residentDal.Save();
        }

        public bool DeleteResident(int id)
        {
            _residentDal.Delete(id);
            return _residentDal.Save();
        }
    }
}