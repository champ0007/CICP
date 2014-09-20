using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AUDashboard.Models
{
    public class Resource
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Level { get; set; }
        public string CurrentProject { get; set; }

        public string ProposedProject { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime AvailableOn { get; set; }

        public string Skills { get; set; }
    }
}