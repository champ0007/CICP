using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AUDash.Models
{
    public class Resource
    {
        public int ResourceId { get; set; }

        public string ResourceData { get; set; }
    }

    public class miniResource
    {
        public int ResourceId { get; set; }

    }

    public class ResourceEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Level { get; set; }
        public string Skills { get; set; }
        public string LastProject { get; set; }
        public string CurrentProject { get; set; }
        public string NextProject { get; set; }
        public string AvailableOn { get; set; }
    }

    public class GroupedProject
    {
        public string Project { get; set; }
        public int Count { get; set; }
    }

}