using System;
using System.Collections.Generic;
using System.Text;

using Apartify.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartify.DAL
{
    public interface IServiceFeeDal
    {
        IEnumerable<ServiceFee> GetAll();
        ServiceFee? GetById(int id);
        void Add(ServiceFee fee);
        void Update(ServiceFee fee);
        void Delete(int id);
        bool Save();
    }

    public class ServiceFeeDal : IServiceFeeDal
    {
        private readonly ApartifyContext _context;

        public ServiceFeeDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<ServiceFee> GetAll()
        {

            return _context.ServiceFees
                .Include(f => f.Apartment)
                    .ThenInclude(a => a.Building)
                .ToList();
        }

        public ServiceFee? GetById(int id)
        {
            return _context.ServiceFees
                .Include(f => f.Apartment)
                .FirstOrDefault(f => f.FeeId == id);
        }

        public void Add(ServiceFee fee)
        {
            _context.ServiceFees.Add(fee);
            Save();
        }

        public void Update(ServiceFee fee)
        {
            _context.ServiceFees.Update(fee);
            Save();
        }

        public void Delete(int id)
        {
            var fee = _context.ServiceFees.Find(id);
            if (fee != null)
            {
                _context.ServiceFees.Remove(fee);
                Save();
            }
        }

        public bool Save() => _context.SaveChanges() > 0;
    }
}