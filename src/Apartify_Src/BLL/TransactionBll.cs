using System.Collections.Generic;
using Apartify.DAL;
using Apartify.Models;

namespace Apartify.BLL
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> GetAllTransactions();
        Transaction? GetTransactionById(int id);
        bool AddTransaction(Transaction transaction);
        bool UpdateTransaction(Transaction transaction);
        bool DeleteTransaction(int id);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionDal _transactionDal;

        public TransactionService(ITransactionDal transactionDal)
        {
            _transactionDal = transactionDal;
        }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return _transactionDal.GetAll();
        }

        public Transaction? GetTransactionById(int id)
        {
            return _transactionDal.GetById(id);
        }

        public bool AddTransaction(Transaction transaction)
        {
            _transactionDal.Add(transaction);
            return _transactionDal.Save();
        }

        public bool UpdateTransaction(Transaction transaction)
        {
            _transactionDal.Update(transaction);
            return _transactionDal.Save();
        }

        public bool DeleteTransaction(int id)
        {
            _transactionDal.Delete(id);
            return _transactionDal.Save();
        }
    }
}