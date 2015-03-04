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

    public class ResourceRequest
    {
        public List<UnallocatedResourceEntity> Resources { get; set; }
        public UnallocatedResourceEntity Resource { get; set; }
        public RequestedAction Action { get; set; }

    }

    public class miniResource
    {
        public int ResourceId { get; set; }

    }

    public class UnallocatedResourceEntity
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Level { get; set; }
        public string Skills { get; set; }
        public string RequiredFrom { get; set; }
        public string RequestDate { get; set; }
        public string NextProject { get; set; }
        public string Comments { get; set; }
        public string PositionStatus { get; set; }
        public string RequiredTill { get; set; }
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
        public string EmpId { get; set; }

    }

    public class GroupedProject
    {
        public string Project { get; set; }
        public int Count { get; set; }
    }

    public class ResourceBySkill
    {
        public int value { get; set; }
        public string label { get; set; }
        public string color { get; set; }
        public string highlight { get; set; }
    }
}