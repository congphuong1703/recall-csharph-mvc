using System;
using System.Collections.Generic;

#nullable disable

namespace WTAssignment.Models
{
    public partial class ItemCategory
    {
        public ItemCategory()
        {
            InverseParentCategory = new HashSet<ItemCategory>();
            Items = new HashSet<Item>();
        }

        public int CategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ItemCategory ParentCategory { get; set; }
        public virtual ICollection<ItemCategory> InverseParentCategory { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
