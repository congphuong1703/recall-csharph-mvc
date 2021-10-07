using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTAssignment.Models
{
    public class RecallDetail
    {
        public ItemsInOrder itemsInOrder { get; set; }
        public Item item { get; set; }
        public int untisSold { get; set; }
        public Address address { get; set; }
        public Customer customer { get; set; }

        public CustomerOrder customerOrder { get; set; }
        public decimal totalCost { get; set; }
        public ItemCategory category { get; set; }

        public int totalCustomers { get; set; }

    }
}
