using System;
using System.Collections.Generic;
using System.Text;

using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IContractDal
    {
        IEnumerable<Contract> GetAllContracts();
        Contract? GetContractById(int id);
        void Add(Contract contract);
        void Update(Contract contract);
        void Delete(int id);
        bool Save();
    }

    public class ContractDal : IContractDal
    {
        private readonly ApartifyContext _context;

        public ContractDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<Contract> GetAllContracts()
        {

            return _context.Contracts
                .Include(c => c.Apartment)
                .Include(c => c.Resident)
                .ToList();
        }

        public Contract? GetContractById(int id)
        {
            return _context.Contracts
                .Include(c => c.Apartment)
                .Include(c => c.Resident)
                .FirstOrDefault(c => c.ContractId == id);
        }

        public void Add(Contract contract)
        {
            _context.Contracts.Add(contract);
            Save();
        }

        public void Update(Contract contract)
        {
            _context.Contracts.Update(contract);
            Save();
        }

        public void Delete(int id)
        {
            var contract = _context.Contracts.Find(id);
            if (contract != null)
            {
                _context.Contracts.Remove(contract);
                Save();
            }
        }

        public bool Save() => _context.SaveChanges() > 0;
    }
}