using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AUDash.Models
{
    public class Opportunities
    {
        public string USIOpportunity { get; set; }
        public string OpportunityId { get; set; }
        public string Account { get; set; }
        public string Opportunity { get; set; }
        public string USIProposalSupport { get; set; }
        public string IMLead { get; set; }
        public string USILead { get; set; }
        public string IMService { get; set; }
        public decimal TotalEstimatedRevenue { get; set; }
        public decimal TotalEstimatedNSR { get; set; }
        public decimal TotalThirdPartyEstRevenue { get; set; }
        public int ClosePeriod { get; set; }
        public int ClosePeriodYear { get; set; }
        public string LeadPursuitPartner { get; set; }
        public string SalesStage { get; set; }
        public string DisplayClosePeriod { get; set; }
        public int TotalCount { get; set; }

    }
}