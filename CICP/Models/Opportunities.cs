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
        public string Comments { get; set; }
    }

    public class opportunityComparer : IEqualityComparer<Opportunities>
    {

        public bool Equals(Opportunities x, Opportunities y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) ||
                Object.ReferenceEquals(y, null))
                return false;

            return x.Account == y.Account && x.ClosePeriod == y.ClosePeriod && x.ClosePeriodYear == y.ClosePeriodYear && x.DisplayClosePeriod == y.DisplayClosePeriod && x.IMLead == y.IMLead && x.IMService == y.IMService && x.LeadPursuitPartner == y.LeadPursuitPartner && x.Opportunity == y.Opportunity && x.OpportunityId == y.OpportunityId && x.SalesStage == y.SalesStage && x.TotalCount == y.TotalCount && x.TotalEstimatedNSR == y.TotalEstimatedNSR && x.TotalEstimatedRevenue == y.TotalEstimatedRevenue && x.TotalThirdPartyEstRevenue == y.TotalThirdPartyEstRevenue && x.USILead == y.USILead && x.USIOpportunity == y.USIOpportunity && x.USIProposalSupport == y.USIProposalSupport;
        }

        public int GetHashCode(Opportunities opp)
        {
            if (Object.ReferenceEquals(opp, null)) return 0;

            int hashAccount = opp.Account == null ? 0 : opp.Account.GetHashCode();
            int hashClosePeriod = opp.ClosePeriod == null ? 0 : opp.ClosePeriod.GetHashCode();
            int hashClosePeriodYear = opp.ClosePeriodYear == null ? 0 : opp.ClosePeriodYear.GetHashCode();
            int hashDisplayClosePeriod = opp.DisplayClosePeriod == null ? 0 : opp.DisplayClosePeriod.GetHashCode();
            int hashIMLead = opp.IMLead == null ? 0 : opp.IMLead.GetHashCode();
            int hashIMservice = opp.IMService == null ? 0 : opp.IMService.GetHashCode();
            int hashLeadPartner = opp.LeadPursuitPartner == null ? 0 : opp.LeadPursuitPartner.GetHashCode();
            int hashOpportunity = opp.Opportunity == null ? 0 : opp.Opportunity.GetHashCode();
            int hashOpportunityId = opp.OpportunityId == null ? 0 : opp.OpportunityId.GetHashCode();
            int hashSalesStage = opp.SalesStage == null ? 0 : opp.SalesStage.GetHashCode();
            int hashTotalCount = opp.TotalCount.GetHashCode();
            int hashTotalNSR = opp.TotalEstimatedNSR.GetHashCode();
            int hashTotalRevenue = opp.TotalEstimatedRevenue.GetHashCode();
            int hashTotal3rdParty = opp.TotalThirdPartyEstRevenue.GetHashCode();
            int hashUSILead = opp.USILead == null ? 0 : opp.USILead.GetHashCode();
            int hashUSIOpportunity = opp.USIOpportunity == null ? 0 : opp.USIOpportunity.GetHashCode();
            int hashUSIProposalSupport = opp.USIProposalSupport == null ? 0 : opp.USIProposalSupport.GetHashCode();
            int hashComments = opp.Comments == null ? 0 : opp.Comments.GetHashCode();

            return hashAccount ^ hashClosePeriod ^ hashClosePeriodYear ^ hashDisplayClosePeriod ^ hashIMLead ^ hashIMservice ^ hashLeadPartner ^ hashOpportunity ^ hashOpportunityId ^ hashSalesStage ^ hashTotal3rdParty ^ hashTotalCount ^ hashTotalNSR ^ hashTotalRevenue ^ hashUSILead ^ hashUSIOpportunity ^ hashUSIProposalSupport ^ hashComments;
        }
    }

    public class opportunityIdComparer : IEqualityComparer<Opportunities>
    {
        public bool Equals(Opportunities x, Opportunities y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) ||
                Object.ReferenceEquals(y, null))
                return false;

            return x.OpportunityId == y.OpportunityId;
        }

        public int GetHashCode(Opportunities opp)
        {
            if (Object.ReferenceEquals(opp, null)) return 0;

            int hashId = opp.OpportunityId.GetHashCode();

            return hashId;
        }
    }
}