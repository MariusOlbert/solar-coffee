using SolarCoffee.Data.Models;
using SolarCoffee.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Serialization
{
    public static class AddressMapper
    {
        public static CustomerAddressModel SerializeCustomerAddress(CustomerAddress customerAddress)
        {
            return new CustomerAddressModel
            {
                Id = customerAddress.Id,
                AddressLine1 = customerAddress.AddressLine1,
                AddressLine2 = customerAddress.AddressLine2,
                City = customerAddress.City,
                State = customerAddress.State,
                Country = customerAddress.Country,
                PostalCode = customerAddress.PostalCode,
                CreatedOn = customerAddress.CreatedOn,
                UpdatedOn = customerAddress.UpdatedOn
            };
        }
        public static CustomerAddress SerializeCustomerAddress(CustomerAddressModel customerAddress)
        {
            return new CustomerAddress
            {
                Id = customerAddress.Id,
                AddressLine1 = customerAddress.AddressLine1,
                AddressLine2 = customerAddress.AddressLine2,
                City = customerAddress.City,
                State = customerAddress.State,
                Country = customerAddress.Country,
                PostalCode = customerAddress.PostalCode,
                CreatedOn = customerAddress.CreatedOn,
                UpdatedOn = customerAddress.UpdatedOn
            };
        }
    }
}
