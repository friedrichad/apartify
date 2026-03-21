using Apartify.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Apartify.DAL
{
    public interface ITransactionDal
    {
        IEnumerable<Transaction> GetAll();
        Transaction? GetById(int id);
        void Add(Transaction transaction);
        void Update(Transaction transaction);
        void Delete(int id);
        bool Save();
    }

    public class TransactionDal : ITransactionDal
    {
        private readonly ApartifyContext _context;

        public TransactionDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _context.Transactions
                .Include(t => t.Fee)
                .Include(t => t.Method)
                .Include(t => t.Resident)
                .ToList();
        }

        public Transaction? GetById(int id)
        {
            return _context.Transactions
                .Include(t => t.Fee)
                .Include(t => t.Method)
                .Include(t => t.Resident)
                .FirstOrDefault(t => t.TransactionId == id);
        }

        public void Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
        }

        public void Update(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
        }

        public void Delete(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}