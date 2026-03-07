using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;

namespace Apartify.BLL
{
    public interface IContractBll
    {
        IEnumerable<Contract> GetList();
        Contract? GetDetail(int id);
        string CreateContract(Contract contract); 
        bool Remove(int id);
    }

    public class ContractBll : IContractBll
    {
        private readonly IContractDal _contractDal;

        public ContractBll(IContractDal contractDal)
        {
            _contractDal = contractDal;
        }

        public IEnumerable<Contract> GetList() => _contractDal.GetAllContracts();

        public Contract? GetDetail(int id) => _contractDal.GetContractById(id);

        public string CreateContract(Contract contract)
        {

            if (contract.StartDate >= contract.EndDate)
            {
                return "Ngày bắt đầu phải trước ngày kết thúc!";
            }

       
            if (contract.ApartmentId == null || contract.ResidentId == null)
            {
                return "Căn hộ và Cư dân không được để trống!";
            }

            _contractDal.Add(contract);
            return "Success";
        }

        public bool Remove(int id)
        {
            var contract = _contractDal.GetContractById(id);
            if (contract == null) return false;

            _contractDal.Delete(id);
            return true;
        }
    }
}