using System;
using System.Collections.Generic;
using System.Linq;
using Apartify.DAL;
using Apartify.Models;
using Apartify.BLL.Helpers;

namespace Apartify.BLL
{
    public interface IApartmentBll
    {
        IEnumerable<Apartment> GetList();
        Apartment? GetDetail(int id);
        bool Create(Apartment apartment);
        bool Edit(Apartment apartment);
        bool Remove(int id);
    }

    public class ApartmentBll : IApartmentBll
    {
        private readonly IApartmentDal _apartmentDal;

        public ApartmentBll(IApartmentDal apartmentDal)
        {
            _apartmentDal = apartmentDal;
        }

        public IEnumerable<Apartment> GetList()
        {
            return _apartmentDal.GetAll();
        }

        public Apartment? GetDetail(int id)
        {
            return _apartmentDal.GetById(id);
        }

        public bool Create(Apartment apartment)
        {
            ValidateHelper.ValidateApartment(apartment);

            _apartmentDal.Add(apartment);
            return _apartmentDal.Save();
        }

        public bool Edit(Apartment apartment)
        {
            ValidateHelper.ValidateApartment(apartment);

            var existing = _apartmentDal.GetById(apartment.ApartmentId);
            if (existing == null) return false;

            _apartmentDal.Update(apartment);
            return _apartmentDal.Save();
        }

        public bool Remove(int id)
        {
            var apt = _apartmentDal.GetById(id);
            if (apt == null) return false;

            if (apt.Contracts.Any())
                return false;

            _apartmentDal.Delete(id);
            return _apartmentDal.Save();
        }
    }
}