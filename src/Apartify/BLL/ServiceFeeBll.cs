using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;
using Apartify.BLL.Helpers;

namespace Apartify.BLL
{
    public interface IServiceFeeBll
    {
        IEnumerable<ServiceFee> GetAllFees();
        ServiceFee? GetFeeById(int id);
        bool AddFee(ServiceFee fee);
        bool UpdateFee(ServiceFee fee);
        bool DeleteFee(int id);
    }
    public class ServiceFeeBll : IServiceFeeBll
    {
        private readonly IServiceFeeDal _feeDal;

        public ServiceFeeBll(IServiceFeeDal feeDal)
        {
            _feeDal = feeDal;
        }

        public IEnumerable<ServiceFee> GetAllFees()
        {
            return _feeDal.GetAll();
        }

        public ServiceFee? GetFeeById(int id)
        {
            return _feeDal.GetById(id);
        }

        public bool AddFee(ServiceFee fee)
        {
            ValidateHelper.ValidateServiceFee(fee);

            _feeDal.Add(fee);
            return _feeDal.Save();
        }

        public bool UpdateFee(ServiceFee fee)
        {
            ValidateHelper.ValidateServiceFee(fee);

            _feeDal.Update(fee);
            return _feeDal.Save();
        }

        public bool DeleteFee(int id)
        {
            _feeDal.Delete(id);
            return _feeDal.Save();
        }
    }
}