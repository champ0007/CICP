using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AUDash.Models
{
    public class Resource
    {
        public int ResourceID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Level { get; set; }
        public string CurrentProject { get; set; }

        public string ProposedProject { get; set; }

        public string StartDate { get; set; }

        public string AvailableOn { get; set; }

        public string Skills { get; set; }
    }
}