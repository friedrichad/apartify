using Apartify.DAL;
using Apartify.Models;
using System.Collections.Generic;

namespace Apartify.BLL
{
    public interface IRequestService
    {
        IEnumerable<Request> GetAllRequests();
        Request? GetRequestById(int id);
        bool AddRequest(Request request);
        bool UpdateRequest(Request request);
        bool DeleteRequest(int id);
    }

    public class RequestService : IRequestService
    {
        private readonly IRequestDal _requestDal;

        public RequestService(IRequestDal requestDal)
        {
            _requestDal = requestDal;
        }

        public IEnumerable<Request> GetAllRequests()
        {
            return _requestDal.GetAll();
        }

        public Request? GetRequestById(int id)
        {
            return _requestDal.GetById(id);
        }

        public bool AddRequest(Request request)
        {
            _requestDal.Add(request);
            return _requestDal.Save();
        }

        public bool UpdateRequest(Request request)
        {
            _requestDal.Update(request);
            return _requestDal.Save();
        }

        public bool DeleteRequest(int id)
        {
            _requestDal.Delete(id);
            return _requestDal.Save();
        }
    }
}