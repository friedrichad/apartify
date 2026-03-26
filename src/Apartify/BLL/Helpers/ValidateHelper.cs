using System;
using System.Linq;
using System.Text.RegularExpressions;
using Apartify.Models;

namespace Apartify.BLL.Helpers
{
    public static class ValidateHelper
    {
        // NEW: Regular expression for Month format (yyyy-MM)
        private static readonly Regex MonthRegex = new Regex(@"^\d{4}-(0[1-9]|1[0-2])$");

        // NEW: Allowed statuses for Request
        private static readonly string[] AllowedStatuses = { "Pending", "Processing", "Done" };

        public static void ValidateBuilding(Building building)
        {
            if (string.IsNullOrWhiteSpace(building.Name))
                throw new Exception("Building name cannot be empty");
            if (building.Name.Length > 100)
                throw new Exception("Building name cannot exceed 100 characters");

            // FIX: Address is nullable in DB, only check length if provided
            if (!string.IsNullOrWhiteSpace(building.Address) && building.Address.Length > 200)
                throw new Exception("Address cannot exceed 200 characters");
        }

        public static void ValidateApartment(Apartment apartment)
        {
            // FIX: Apartment Number is nullable in schema, check length if exists
            if (!string.IsNullOrWhiteSpace(apartment.Number) && apartment.Number.Length > 20)
                throw new Exception("Apartment number cannot exceed 20 characters");

            if (apartment.Area.HasValue && apartment.Area <= 0)
                throw new Exception("Apartment area must be greater than 0");

            if (apartment.Floor.HasValue && apartment.Floor <= 0)
                throw new Exception("Floor must be greater than 0");

            // NEW: FK Validation
            if (apartment.BuildingId <= 0) // FIX: Check ID > 0
                throw new Exception("Invalid Building ID");
            
            // Suggestion: if (!_buildingDal.Exists(apartment.BuildingId)) throw ...
        }

        public static void ValidateResident(Resident resident)
        {
            if (string.IsNullOrWhiteSpace(resident.FullName))
                throw new Exception("Resident full name cannot be empty");
            if (resident.FullName.Length > 100)
                throw new Exception("Full name cannot exceed 100 characters");

            if (!string.IsNullOrWhiteSpace(resident.Email))
            {
                if (resident.Email.Length > 100)
                    throw new Exception("Email cannot exceed 100 characters");
                if (!IsValidEmail(resident.Email))
                    throw new Exception("Invalid email format");
            }

            if (!string.IsNullOrWhiteSpace(resident.Phone))
            {
                if (resident.Phone.Length > 20)
                    throw new Exception("Phone number cannot exceed 20 characters");
                if (!IsValidPhone(resident.Phone))
                    throw new Exception("Invalid phone number format");
            }

            // NEW: FK Validation
            if (resident.UserId <= 0)
                throw new Exception("Invalid User ID");
        }


        public static void ValidateContract(Contract contract, bool isApartmentOccupied = false)
        {
            // NEW: FK Validations
            if (contract.ApartmentId <= 0)
                throw new Exception("Invalid Apartment ID");
            if (contract.ResidentId <= 0)
                throw new Exception("Invalid Resident ID");

            if (contract.StartDate.HasValue && contract.EndDate.HasValue)
            {
                if (contract.StartDate > contract.EndDate)
                    throw new Exception("Start date cannot be greater than end date");
            }

            // NEW: Business Logic - Check if apartment already has an active contract
            if (isApartmentOccupied)
                throw new Exception("Apartment already has an active contract");
        }



        public static void ValidateRequest(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Description))
                throw new Exception("Request description cannot be empty");
            if (request.Description.Length > 200)
                throw new Exception("Description cannot exceed 200 characters");

            // NEW: List validation for Status
            if (string.IsNullOrWhiteSpace(request.Status))
                throw new Exception("Status cannot be empty");
            if (!AllowedStatuses.Contains(request.Status))
                throw new Exception("Status must be one of: Pending, Processing, Done");

            // NEW: FK Validations
            if (request.ResidentId <= 0)
                throw new Exception("Invalid Resident ID");
            if (request.ApartmentId <= 0)
                throw new Exception("Invalid Apartment ID");
        }

        public static void ValidateUserAccount(UserAccount user, bool isUsernameDuplicate = false)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new Exception("Username cannot be empty");
            if (user.Username.Length > 50)
                throw new Exception("Username cannot exceed 50 characters");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Password cannot be empty");
            if (user.Password.Length > 100)
                throw new Exception("Password cannot exceed 100 characters");

            // NEW: Business Logic - No duplicate Username
            if (isUsernameDuplicate)
                throw new Exception("Username already exists");
        }

        private static bool IsValidEmail(string email)
        {
            try {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            } catch { return false; }
        }

        private static bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^[0-9]{10,11}$");
        }
    }
}
