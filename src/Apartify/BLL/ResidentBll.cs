using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;
using Apartify.BLL.Helpers;

namespace Apartify.BLL
{
    public interface IResidentBll
    {
        IEnumerable<Resident> GetList();
        Resident? GetDetail(int id);
        bool Create(Resident resident);
        bool Edit(Resident resident);
        bool Remove(int id);
    }
    public class ResidentBll : IResidentBll
    {
        private readonly IResidentDal _residentDal;

        public ResidentBll(IResidentDal residentDal)
        {
            _residentDal = residentDal;
        }

        public IEnumerable<Resident> GetList()
        {
            return _residentDal.GetAll();
        }

        public Resident? GetDetail(int id)
        {
            return _residentDal.GetById(id);
        }

        public bool Create(Resident resident)
        {
            ValidateHelper.ValidateResident(resident);

            _residentDal.Add(resident);
            return _residentDal.Save();
        }

        public bool Edit(Resident resident)
        {
            ValidateHelper.ValidateResident(resident);

            _residentDal.Update(resident);
            return _residentDal.Save();
        }

        public bool Remove(int id)
        {
            _residentDal.Delete(id);
            return _residentDal.Save();
        }
    }
}