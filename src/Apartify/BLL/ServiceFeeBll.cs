using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;

namespace Apartify.BLL
{
    public interface IServiceFeeBll
    {
        IEnumerable<ServiceFee> GetList();
        ServiceFee? GetDetail(int id);
        void CreateFee(ServiceFee fee);
        void MarkAsPaid(int id);
        IEnumerable<ServiceFee> GetUnpaidFees();
    }

    public class ServiceFeeBll : IServiceFeeBll
    {
        private readonly IServiceFeeDal _feeDal;

        public ServiceFeeBll(IServiceFeeDal feeDal)
        {
            _feeDal = feeDal;
        }

        public IEnumerable<ServiceFee> GetList() => _feeDal.GetAll();

        public ServiceFee? GetDetail(int id) => _feeDal.GetById(id);

        public void CreateFee(ServiceFee fee)
        {
 
            fee.Paid = false;
            _feeDal.Add(fee);
        }

        public void MarkAsPaid(int id)
        {
            var fee = _feeDal.GetById(id);
            if (fee != null)
            {
                fee.Paid = true;
                _feeDal.Update(fee);
            }
        }

        public IEnumerable<ServiceFee> GetUnpaidFees()
        {
            return _feeDal.GetAll().Where(f => f.Paid == false);
        }
    }
}