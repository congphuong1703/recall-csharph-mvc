using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WTAssignment.Models
{
    public partial class Item
    {
        public Item()
        {
            ItemMarkupHistories = new HashSet<ItemMarkupHistory>();
            ItemsInOrders = new HashSet<ItemsInOrder>();
            Reviews = new HashSet<Review>();
        }

        [Key]
        [Display(Name = "Item #")]
        public int ItemId { get; set; }

        [Display(Name = "Item Name")]
        [MaxLength(150, ErrorMessage = "Item name must not be more than 150 characters")]
        [Required]
        public string ItemName { get; set; }

        [Display(Name = "Item Description")]
        [MaxLength(5000, ErrorMessage = "Item description must not be more than 5000 characters")]
        [MinLength(5, ErrorMessage = "Item description must not be less than 10 characters")]
        [Required]
        public string ItemDescription { get; set; }

        [Display(Name = "Item Cost")]
        [DataType(DataType.Currency)]
        // alternatively
        //[DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [Range(0, 99999999.99, ErrorMessage = "Item cost must be between 0 and 99999999.99")]
        [Required]
        public decimal ItemCost { get; set; }

        [Display(Name = "Item Image")]
        [MaxLength(5000, ErrorMessage = "Item image url must not be more than 5000 characters")]
        [Required]
        public string ItemImage { get; set; }


        [Display(Name = "Item Category")]
        public int? CategoryId { get; set; }

        public virtual ItemCategory Category { get; set; }
        public virtual ICollection<ItemMarkupHistory> ItemMarkupHistories { get; set; }
        public virtual ICollection<ItemsInOrder> ItemsInOrders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}


