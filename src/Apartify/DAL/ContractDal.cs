using System.Collections.Generic;
using System.Linq;
using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IContractDal
    {
        IEnumerable<Contract> GetAll();
        Contract? GetById(int id);
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

        public IEnumerable<Contract> GetAll()
        {
            return _context.Contracts
                .Include(c => c.Apartment)
                .Include(c => c.Resident)
                .ToList();
        }

        public Contract? GetById(int id)
        {
            return _context.Contracts
                .Include(c => c.Apartment)
                .Include(c => c.Resident)
                .FirstOrDefault(c => c.ContractId == id);
        }

        public void Add(Contract contract)
        {
            _context.Contracts.Add(contract);
        }

        public void Update(Contract contract)
        {
            _context.Contracts.Update(contract);
        }

        public void Delete(int id)
        {
            var contract = _context.Contracts.Find(id);
            if (contract != null)
            {
                _context.Contracts.Remove(contract);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}