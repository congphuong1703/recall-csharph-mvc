using System;
using System.Collections.Generic;

#nullable disable

namespace WTAssignment.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReviewDate { get; set; }
        public int? ItemId { get; set; }
        public int Rating { get; set; }
        public string ReviewDescription { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Item Item { get; set; }
    }
}
