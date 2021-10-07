using System;
using System.Collections.Generic;

#nullable disable

namespace WTAssignment.Models
{
    public partial class Address
    {
        public Address()
        {
            Customers = new HashSet<Customer>();
        }

        public int AddressId { get; set; }
        public string AddressLine { get; set; }
        public string Suburb { get; set; }
        public string Postcode { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
