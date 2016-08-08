using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Shared
{
    public class Address
    {
        public Address(string city, string street, string zipCode, string flatNumber, string phoneNumber)
        {
            City = city;
            Street = street;
            ZipCode = zipCode;
            FlatNumber = flatNumber;
            PhoneNumber = phoneNumber;
        }

        public string City { get; private set; }

        public string Street { get; private set; }

        public string ZipCode { get; private set; }

        public string FlatNumber { get; private set; }

        public string PhoneNumber { get; private set; }
    }
}
