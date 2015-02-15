using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AUDash.Models
{
    public class ProjectEntity
    {
        public string Id { get; set; }
        public string Client { get; set; }
        public string ProjectName { get; set; }
        public string Stage { get; set; }
        public string GDM { get; set; }
        public string Probability { get; set; }
        public string Technology { get; set; }
        public string TotalResources { get; set; }
        public string WorkOrderStatus { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }

    }

    public class ProjGroupedByStatus
    {
        public int Count { get; set; }
        public string ProjectStatus { get; set; }
    }

    public class ProjectRequest
    {
        public List<ProjectEntity> Projects { get; set; }
        public ProjectEntity Project { get; set; }
        public RequestedAction action { get; set; }

    }
   
}