using System;
using System.Collections.Generic;
using System.Text;

using Apartify.DAL;
using Apartify.Models;

namespace Apartify.BLL
{
    public interface IApartmentBll
    {
        IEnumerable<Apartment> GetList();
        Apartment? GetDetail(int id);
        bool Create(Apartment apartment);
        void Edit(Apartment apartment);
        bool Remove(int id);
    }

    public class ApartmentBll : IApartmentBll
    {
        private readonly IApartmentDal _apartmentDal;

        public ApartmentBll(IApartmentDal apartmentDal)
        {
            _apartmentDal = apartmentDal;
        }

        public IEnumerable<Apartment> GetList() => _apartmentDal.GetAllApartments();

        public Apartment? GetDetail(int id) => _apartmentDal.GetApartmentById(id);

        public bool Create(Apartment apartment)
        {
            if (string.IsNullOrEmpty(apartment.Number)) return false;

            _apartmentDal.Add(apartment);
            return true;
        }

        public void Edit(Apartment apartment)
        {
            _apartmentDal.Update(apartment);
        }

        public bool Remove(int id)
        {
            var apt = _apartmentDal.GetApartmentById(id);
            if (apt == null) return false;

            if (apt.Contracts.Any())
            {
                return false;
            }

            _apartmentDal.Delete(id);
            return true;
        }
    }
}