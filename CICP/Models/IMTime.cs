using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AUDash.Models
{
    public class IMTime
    {
        public string IMLead { get; set; }
        public string ServiceClientName { get; set; }
        public string MandateSF { get; set; }
        public int Period { get; set; }
        public decimal IMHours { get; set; }
        public decimal NonIMHours { get; set; }
        public decimal TotalHours { get; set; }

    }
}