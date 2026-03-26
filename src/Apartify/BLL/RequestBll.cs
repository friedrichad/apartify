using Apartify.DAL;
using Apartify.Models;
using System.Collections.Generic;
using Apartify.BLL.Helpers;

namespace Apartify.BLL
{
    public interface IRequestBll
    {
        IEnumerable<Request> GetList();
        Request? GetDetail(int id);
        bool Create(Request request);
        bool Edit(Request request);
        bool Remove(int id);
    }

    public class RequestBll : IRequestBll
    {
        private readonly IRequestDal _requestDal;

        public RequestBll(IRequestDal requestDal)
        {
            _requestDal = requestDal;
        }

        public IEnumerable<Request> GetList()
        {
            return _requestDal.GetAll();
        }

        public Request? GetDetail(int id)
        {
            return _requestDal.GetById(id);
        }

        public bool Create(Request request)
        {
            ValidateHelper.ValidateRequest(request);

            _requestDal.Add(request);
            return _requestDal.Save();
        }

        public bool Edit(Request request)
        {
            ValidateHelper.ValidateRequest(request);

            _requestDal.Update(request);
            return _requestDal.Save();
        }

        public bool Remove(int id)
        {
            _requestDal.Delete(id);
            return _requestDal.Save();
        }
    }
}