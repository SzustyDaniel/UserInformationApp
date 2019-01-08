using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider
{
    [DelimitedRecord(","), IgnoreFirst]
    class UserEngine
    {
        #region Properties
        [FieldOrder(0)]
        public int ID { get; set; }
        [FieldOrder(1)]
        public string FirstName { get; set; }
        [FieldOrder(2)]
        public string LastName { get; set; }
        [FieldOrder(3)]
        public string Email { get; set; }
        [FieldOrder(4)]
        public string Gender { get; set; }
        [FieldOrder(5)]
        public string Country { get; set; }
        [FieldOrder(6)]
        public string City { get; set; }
        [FieldOrder(7)]
        public string Street { get; set; }
        [FieldOrder(8)]
        public string PhoneNumber { get; set; }
        #endregion

        public UserEngine(int id, string firstName, string lastName, string email, string gender, string country, string city, string street, string phonenumber)
        {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Gender = gender;
            this.Country = country;
            this.City = city;
            this.Street = street;
            this.PhoneNumber = phonenumber;
        }

        public UserEngine()
        {
        }
    }
}
