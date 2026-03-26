using Apartify.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Apartify.DAL
{
    public interface IRequestDal
    {
        IEnumerable<Request> GetAll();
        Request? GetById(int id);
        void Add(Request request);
        void Update(Request request);
        void Delete(int id);
        bool Save();
    }

    public class RequestDal : IRequestDal
    {
        private readonly ApartifyContext _context;

        public RequestDal(ApartifyContext context)
        {
            _context = context;
        }

        public IEnumerable<Request> GetAll()
        {
            return _context.Requests
                .Include(r => r.Resident)
                .Include(r => r.Apartment)
                .ToList();
        }

        public Request? GetById(int id)
        {
            return _context.Requests
                .Include(r => r.Resident)
                .Include(r => r.Apartment)
                .FirstOrDefault(r => r.RequestId == id);
        }

        public void Add(Request request)
        {
            _context.Requests.Add(request);
        }

        public void Update(Request request)
        {
            _context.Requests.Update(request);
        }

        public void Delete(int id)
        {
            var req = _context.Requests.Find(id);
            if (req != null)
            {
                _context.Requests.Remove(req);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}