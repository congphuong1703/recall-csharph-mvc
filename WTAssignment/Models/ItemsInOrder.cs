using System;
using System.Collections.Generic;

#nullable disable

namespace WTAssignment.Models
{
    public partial class ItemsInOrder
    {
        public int OrderNumber { get; set; }
        public int ItemId { get; set; }
        public int NumberOf { get; set; }
        public decimal? TotalItemCost { get; set; }

        public virtual Item Item { get; set; }
        public virtual CustomerOrder OrderNumberNavigation { get; set; }
    }
}
