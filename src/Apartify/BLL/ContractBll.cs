using System;
using System.Collections.Generic;
using Apartify.DAL;
using Apartify.Models;
using Apartify.BLL.Helpers;

namespace Apartify.BLL
{
    public interface IContractBll
    {
        IEnumerable<Contract> GetList();
        Contract? GetDetail(int id);
        bool Create(Contract contract);
        bool Edit(Contract contract);
        bool Remove(int id);
    }

    public class ContractBll : IContractBll
    {
        private readonly IContractDal _contractDal;

        public ContractBll(IContractDal contractDal)
        {
            _contractDal = contractDal;
        }

        public IEnumerable<Contract> GetList()
        {
            return _contractDal.GetAllContracts();
        }

        public Contract? GetDetail(int id)
        {
            return _contractDal.GetContractById(id);
        }

        public bool Create(Contract contract)
        {
            ValidateHelper.ValidateContract(contract);

            _contractDal.Add(contract);
            return _contractDal.Save();
        }

        public bool Edit(Contract contract)
        {
            ValidateHelper.ValidateContract(contract);

            var existing = _contractDal.GetContractById(contract.ContractId);
            if (existing == null) return false;

            _contractDal.Update(contract);
            return _contractDal.Save();
        }

        public bool Remove(int id)
        {
            var contract = _contractDal.GetContractById(id);
            if (contract == null) return false;

            _contractDal.Delete(id);
            return _contractDal.Save();
        }
    }
}