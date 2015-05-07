using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AUDash.Models
{
    public class ActualBudget
    {
        public string Series { get; set; }
        public int Period { get; set; }
        public decimal TotalHours { get; set; }
    }
}