using System;
using System.Collections.Generic;
using System.Linq;
using Apartify.DAL;
using Apartify.Models;

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
            return _apartmentDal.GetAllApartments();
        }

        public Apartment? GetDetail(int id)
        {
            return _apartmentDal.GetApartmentById(id);
        }

        public bool Create(Apartment apartment)
        {
            if (string.IsNullOrWhiteSpace(apartment.Number))
                return false;

            _apartmentDal.Add(apartment);
            return _apartmentDal.Save();
        }

        public bool Edit(Apartment apartment)
        {
            var existing = _apartmentDal.GetApartmentById(apartment.ApartmentId);
            if (existing == null) return false;

            _apartmentDal.Update(apartment);
            return _apartmentDal.Save();
        }

        public bool Remove(int id)
        {
            var apt = _apartmentDal.GetApartmentById(id);
            if (apt == null) return false;

            if (apt.Contracts.Any())
                return false;

            _apartmentDal.Delete(id);
            return _apartmentDal.Save();
        }
    }
}