using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;

namespace Apartify.BLL
{
    public interface IResidentBll
    {
        IEnumerable<Resident> GetList();
        Resident? GetDetail(int id);
        bool Create(Resident resident);
        void Edit(Resident resident);
        string Remove(int id);
    }

    public class ResidentBll : IResidentBll
    {
        private readonly IResidentDal _residentDal;

        public ResidentBll(IResidentDal residentDal)
        {
            _residentDal = residentDal;
        }

        public IEnumerable<Resident> GetList() => _residentDal.GetAll();

        public Resident? GetDetail(int id) => _residentDal.GetById(id);

        public bool Create(Resident resident)
        {

            if (string.IsNullOrEmpty(resident.FullName) || string.IsNullOrEmpty(resident.Phone))
                return false;

            _residentDal.Add(resident);
            return true;
        }

        public void Edit(Resident resident)
        {
            _residentDal.Update(resident);
        }

        public string Remove(int id)
        {
            var res = _residentDal.GetById(id);
            if (res == null) return "Không tìm thấy cư dân!";


            if (res.Contracts.Any())
            {
                return "Không thể xóa cư dân này vì họ đang có dữ liệu hợp đồng!";
            }

            _residentDal.Delete(id);
            return "Success";
        }
    }
}