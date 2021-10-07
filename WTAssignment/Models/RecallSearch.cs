using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WTAssignment.Models
{
    public class RecallSearch
    {
        public List<RecallDetail> recallDetails { get; set; }

        public string itemName { get; set; }
        public int saleYear { get; set; }
        public List<int> saleYears { get; set; }

        public List<string> itemNames { get; set; } 
        public Item item { get; set; }

    }
}
