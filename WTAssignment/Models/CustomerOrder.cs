using System;
using System.Collections.Generic;

#nullable disable

namespace WTAssignment.Models
{
    public partial class CustomerOrder
    {
        public CustomerOrder()
        {
            ItemsInOrders = new HashSet<ItemsInOrder>();
        }

        public int OrderNumber { get; set; }
        public int? CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DatePaid { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<ItemsInOrder> ItemsInOrders { get; set; }
    }
}
