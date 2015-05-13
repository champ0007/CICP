using AUDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft;
using Newtonsoft.Json;
using AUDash.Repository;
using OfficeOpenXml;
using System.IO;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;


namespace AUDash.Controllers
{
    public class DashboardController : ApiController
    {
        DBRepository repo = new DBRepository();

        private string AUTH_TOKEN = "admin-admin";

        public string GetAuthentication(string authToken)
        {
            string uid = authToken.Split('-').First();
            string pass = authToken.Split('-').Last();
            if (authToken.Equals(AUTH_TOKEN))
                return "true";
            else
                return "false";
        }

        //GET api/Dashboard/GetDashboardCounts
        public string GetDashboardCounts(string authToken)
        {
            List<string> dashboardCounts = new List<string>();
            if (authToken.Equals(AUTH_TOKEN))
                dashboardCounts = ParseDashboardCounts(repo.GetDashboardCounts());
            return JsonConvert.SerializeObject(dashboardCounts);
        }

        //GET api/Dashboard/GetProjectChartData //Added by Vibhav
        public List<string> GetProjectChartData(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseProjectData(repo.GetReferenceData("Projects"));
            else
                return new List<string>();
        }

        //GET api/Dashboard/GetRevenueChartData //Added by Vibhav
        public List<string> GetRevenueChartData(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseRevenueData(repo.GetReferenceData("Invoices"));
            else
                return new List<string>();
        }

        //GET api/Dashboard/GetTechChartData //Added by Vibhav
        public List<string> GetTechChartData(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseSKillData(repo.GetReferenceData("Projects"));
            else
                return new List<string>();
        }

        //GET api/Dashboard/GetProjChartData //Added by Vibhav
        public List<string> GetProjChartData(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseProjBySkillData(repo.GetReferenceData("Projects"));
            else
                return new List<string>();
        }
        //GET api/GetSoldProposedChartData
        public List<string> GetSoldProposedChartData(string authToken)
        {
            //return ParseSoldProposedData(repo.GetReferenceData("Projects"));
            if (authToken.Equals(AUTH_TOKEN))
                return ParseSoldProposedData(repo.GetReferenceData("Resources"), repo.GetReferenceData("GSSResources"));
            else
                return new List<string>();

        }

        //GET api/Dashboard/GetReferenceData
        public string GetReferenceData(string storageId, string authToken)
        {
            string response = string.Empty;
            if (authToken.Equals(AUTH_TOKEN))
                response = repo.GetReferenceData(storageId);

            return response == string.Empty ? null : response;
        }

        //GET api/Dashboard/GetResourceChartData
        public List<string> GetResourceChartData(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseResourceData(repo.GetReferenceData("GSSResources"));
            else
                return new List<string>();
        }

        //GET api/Dashboard/GetProjectDistributionChartData
        public List<string> GetProjectDistributionChartData(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseProjectDistributionData(repo.GetReferenceData("Projects"));
            else
                return new List<string>();
        }

        //GET api/Dashboard/GetResourceDeploymentChartData
        public List<string> GetResourceDeploymentChartData(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseResourceDeploymentChartData(repo.GetReferenceData("ResourceDataCount"));
            else
                return new List<string>();
        }

        //POST api/Dashboard/SetReferenceData
        [HttpPost]
        public void SetReferenceData([FromBody] string referenceData)
        {
            ReferenceData refData = JsonConvert.DeserializeObject<ReferenceData>(referenceData);
            //Set Session Values
            //HttpContext.Current.Session[refData.storageId] = refData.storageData;
            //string sessionvalue = Convert.ToString(HttpContext.Current.Session[refData.storageId]);
            if (refData.authToken.Equals(AUTH_TOKEN))
            {
                repo.SetReferenceData(refData.storageId, refData.storageData);
            }
        }

        //GET api/Dashboard/GetResourceList
        public string GetResourceList(string authToken)
        {
            //List<Resource> Resources = new List<Resource>();
            //Resources.Add(new Resource() { FirstName = "Shakil", LastName = "Shaikh", CurrentProject = "Telstra", ProposedProject = "None", Level = "Manager", AvailableOn = "01-Dec-2014", Skills = "Adobe CQ", StartDate = "01-Mar-2014" });
            //Resources.Add(new Resource() { FirstName = "Shakil", LastName = "Shaikh", CurrentProject = "Telstra", ProposedProject = "None", Level = "Manager", AvailableOn = "01-Dec-2014", Skills = "Adobe CQ", StartDate = "01-Mar-2014" });
            //Resources.Add(new Resource() { FirstName = "Shakil", LastName = "Shaikh", CurrentProject = "Telstra", ProposedProject = "None", Level = "Manager", AvailableOn = "01-Dec-2014", Skills = "Adobe CQ", StartDate = "01-Mar-2014" });
            //Resources.Add(new Resource() { FirstName = "Shakil", LastName = "Shaikh", CurrentProject = "Telstra", ProposedProject = "None", Level = "Manager", AvailableOn = "01-Dec-2014", Skills = "Adobe CQ", StartDate = "01-Mar-2014" });
            //Resources.Add(new Resource() { FirstName = "Shakil", LastName = "Shaikh", CurrentProject = "Telstra", ProposedProject = "None", Level = "Manager", AvailableOn = "01-Dec-2014", Skills = "Adobe CQ", StartDate = "01-Mar-2014" });
            if (authToken.Equals(AUTH_TOKEN))
                return JSONConcat(repo.GetAllResources());
            else
                return string.Empty;
        }

        private string JSONConcat(List<Resource> resourceList)
        {
            string concatString = "[";

            foreach (Resource resource in resourceList)
            {
                //concatString += resource.ResourceData + ",";
                concatString += resource.ResourceData.Replace("{", "{\"ResourceId\":\" " + resource.ResourceId + "\",") + ",";
            }
            concatString = concatString.Substring(0, concatString.Length - 1);
            concatString += "]";

            return concatString;

        }

        //POST api/Dashboard/AddResource
        [HttpPost]
        public void AddResource([FromBody]string resource)
        {

            repo.AddResource(resource);
        }

        //POST api/Dashboard/EditResource
        [HttpPost]
        public void EditResource([FromBody]string resource)
        {
            miniResource resourceData = new miniResource();
            resourceData = JsonConvert.DeserializeObject<miniResource>(resource.Substring(0, resource.IndexOf("\"FirstName")).Replace(",", "}"));
            string editedResourceData = "{" + resource.Substring(resource.IndexOf("\"FirstName"));

            repo.EditResource(resource, resourceData.ResourceId);
        }

        //GET api/Dashboard/GetKeyUpdates
        public string GetKeyUpdates(string authToken)
        {
            List<KeyUpdates> kUpdates = new List<KeyUpdates>();
            if (authToken.Equals(AUTH_TOKEN))
            {
                kUpdates.Add(new KeyUpdates()
                {
                    Heading = "Telstra Client India Visit",
                    Highlights = new List<string>() {
                    "Brendan Devers, Jen Cochrane and Adam Sandler along with Telstra client will be visiting Hyderabad and Mumbai office between 1st Sept and 5th Sept"
                 }
                });

                kUpdates.Add(new KeyUpdates()
                {
                    Heading = " Decisions on AU/DD Testing CoE continue",
                    Highlights = new List<string>() {
                    "Process identified",
                    "Identification of right resources for CoE in progress"
                 }
                });
            }

            return JsonConvert.SerializeObject(kUpdates);

        }

        //POST api/Dashboard/UploadFile
        [HttpPost]
        public void UploadInvoices()
        {
            HttpPostedFile uploadedFile = HttpContext.Current.Request.Files[0];

            List<Invoice> invoices = new List<Invoice>();
            string strError;
            int rowCount = 6;
            //byte[] file =  File.ReadAllBytes(@"C:\invoices.xlsx");
            Stream inputStream = uploadedFile.InputStream;
            using (ExcelPackage package = new ExcelPackage(inputStream))
            {
                if (package.Workbook.Worksheets.Count <= 0)
                    strError = "Your Excel file does not contain any work sheets";
                else
                {
                    ExcelWorksheet invoiceWorkSheet = package.Workbook.Worksheets["EDC Billing & Collections"];
                    while (invoiceWorkSheet.Cells[rowCount, 2].Value != null)
                    {
                        invoices.Add(new Invoice()
                        {
                            Project = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 2].Value),
                            Partner = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 3].Value),
                            Resource = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 4].Value),
                            //Period = Convert.ToDateTime(Convert.ToString(invoiceWorkSheet.Cells[rowCount, 5].Value)).ToString("MMM-yy"),
                            Period = Convert.ToDateTime(Convert.ToString(invoiceWorkSheet.Cells[rowCount, 5].Value)).ToString("MM-dd-yyyy"),
                            Date = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 6].Value),
                            Hours = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 7].Value),
                            Amount = Convert.ToDecimal(invoiceWorkSheet.Cells[rowCount, 8].Value),
                            ATBApproval = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 9].Value),
                            ATBApprovalSentOn = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 10].Value).IndexOf(" ") > 0 ? Convert.ToString(invoiceWorkSheet.Cells[rowCount, 10].Value).Substring(0, Convert.ToString(invoiceWorkSheet.Cells[rowCount, 10].Value).IndexOf(" ")) : Convert.ToString(invoiceWorkSheet.Cells[rowCount, 10].Value),
                            InvoiceRaised = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 11].Value),
                            InvoiceNo = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 12].Value),
                            InvoiceRaisedOn = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 13].Value).IndexOf(" ") > 0 ? Convert.ToString(invoiceWorkSheet.Cells[rowCount, 13].Value).Substring(0, Convert.ToString(invoiceWorkSheet.Cells[rowCount, 13].Value).IndexOf(" ")) : Convert.ToString(invoiceWorkSheet.Cells[rowCount, 13].Value),
                            Comments = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 14].Value),
                            PaymentReceived = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 15].Value)
                        });

                        rowCount++;
                    }
                }
            }

            DBRepository repo = new DBRepository();
            repo.SetReferenceData("Invoices", JsonConvert.SerializeObject(invoices));
        }

        public void UploadResources()
        {
            HttpPostedFile uploadedFile = HttpContext.Current.Request.Files[0];
            //byte[] file = File.ReadAllBytes(@"C:\Availability Report - USI TAB.xlsx");

            List<ResourceEntity> resources = new List<ResourceEntity>();
            string strError;
            int rowCount = 5;
            //MemoryStream ms = new MemoryStream(file);

            Stream inputStream = uploadedFile.InputStream;

            using (ExcelPackage package = new ExcelPackage(inputStream))
            {
                if (package.Workbook.Worksheets.Count <= 0)
                    strError = "Your Excel file does not contain any work sheets";
                else
                {
                    ExcelWorksheet resourceWorkSheet = package.Workbook.Worksheets.Where(x => x.Name.Contains("US-I") || x.Name.Contains("USI")).First();
                    while (resourceWorkSheet.Cells[rowCount, 1].Value != null)
                    {
                        resources.Add(new ResourceEntity()
                        {
                            FirstName = Convert.ToString(resourceWorkSheet.Cells[rowCount, 1].Value).Split(',')[1].Trim(),
                            LastName = Convert.ToString(resourceWorkSheet.Cells[rowCount, 1].Value).Split(',')[0].Trim(),
                            EmpId = Convert.ToString(resourceWorkSheet.Cells[rowCount, 2].Value),
                            Skills = Convert.ToString(resourceWorkSheet.Cells[rowCount, 3].Value),
                            Level = Convert.ToString(resourceWorkSheet.Cells[rowCount, 6].Value),
                            LastProject = String.Empty,
                            CurrentProject = ParseProject(Convert.ToString(resourceWorkSheet.Cells[rowCount, 19].Value)),
                            NextProject = ParseProject(Convert.ToString(resourceWorkSheet.Cells[rowCount, 20].Value)),
                            ProposedStartDate = ParseProposedProject(Convert.ToString(resourceWorkSheet.Cells[rowCount, 20].Value), "StartDate"),
                            ProposedEndDate = ParseProposedProject(Convert.ToString(resourceWorkSheet.Cells[rowCount, 20].Value), "EndDate"),
                            AvailableOn = Convert.ToString(resourceWorkSheet.Cells[rowCount, 21].Value).Split(' ')[0]
                        });

                        rowCount++;
                    }
                }
            }

            DBRepository repo = new DBRepository();
            repo.SetReferenceData("GSSResources", JsonConvert.SerializeObject(resources));
            repo.SetReferenceData("ResourceDataCount", GetResourceDataCount(resources.Count(), DateTime.Now.ToString("MMMyy")));
        }

        public void UploadCICPMasterData()
        {
            HttpPostedFile uploadedFile = HttpContext.Current.Request.Files[0];
            Stream inputStream = uploadedFile.InputStream;
            int rowCount = 2;
            ExcelPackage package = new ExcelPackage(inputStream);
            List<ActualBudget> actualBudgetData = new List<ActualBudget>();
            List<IMTime> IMTimeData = new List<IMTime>();
            List<Opportunities> opportunitiesData = new List<Opportunities>();
            List<OpportunityStatus> opportunityStatusData = new List<OpportunityStatus>();

            //Read Actual/Budget Data
            ExcelWorksheet resourceWorkSheet = package.Workbook.Worksheets.Where(x => x.Name.Equals("ActualBudget")).First();
            while (resourceWorkSheet.Cells[rowCount, 1].Value != null)
            {
                actualBudgetData.Add(new ActualBudget()
                {
                    Series = Convert.ToString(resourceWorkSheet.Cells[rowCount, 1].Value),
                    Period = Convert.ToInt32(resourceWorkSheet.Cells[rowCount, 2].Value),
                    TotalHours = Convert.ToDecimal(resourceWorkSheet.Cells[rowCount, 3].Value)
                });

                rowCount++;
            }

            rowCount = 2;
            resourceWorkSheet = package.Workbook.Worksheets.Where(x => x.Name.Equals("IMTime")).First();
            while (resourceWorkSheet.Cells[rowCount, 1].Value != null)
            {
                IMTimeData.Add(new IMTime()
                {
                    IMLead = Convert.ToString(resourceWorkSheet.Cells[rowCount, 1].Value),
                    ServiceClientName = Convert.ToString(resourceWorkSheet.Cells[rowCount, 2].Value),
                    MandateSF = Convert.ToString(resourceWorkSheet.Cells[rowCount, 3].Value),
                    Period = Convert.ToInt32(resourceWorkSheet.Cells[rowCount, 4].Value),
                    IMHours = Convert.ToDecimal(resourceWorkSheet.Cells[rowCount, 5].Value),
                    NonIMHours = Convert.ToDecimal(resourceWorkSheet.Cells[rowCount, 6].Value),
                    TotalHours = Convert.ToDecimal(resourceWorkSheet.Cells[rowCount, 7].Value)
                });

                rowCount++;
            }

            rowCount = 2;
            resourceWorkSheet = package.Workbook.Worksheets.Where(x => x.Name.Equals("Opportunities")).First();
            while (resourceWorkSheet.Cells[rowCount, 1].Value != null)
            {
                opportunitiesData.Add(new Opportunities()
                {
                    USIOpportunity = Convert.ToString(resourceWorkSheet.Cells[rowCount, 1].Value),
                    OpportunityId = Convert.ToString(resourceWorkSheet.Cells[rowCount, 2].Value),
                    Account = Convert.ToString(resourceWorkSheet.Cells[rowCount, 3].Value),
                    Opportunity = Convert.ToString(resourceWorkSheet.Cells[rowCount, 4].Value),
                    USIProposalSupport = Convert.ToString(resourceWorkSheet.Cells[rowCount, 5].Value),
                    IMLead = Convert.ToString(resourceWorkSheet.Cells[rowCount, 6].Value),
                    USILead = Convert.ToString(resourceWorkSheet.Cells[rowCount, 7].Value),
                    IMService = Convert.ToString(resourceWorkSheet.Cells[rowCount, 8].Value),
                    TotalEstimatedRevenue = Convert.ToDecimal(resourceWorkSheet.Cells[rowCount, 9].Value),
                    TotalEstimatedNSR = Convert.ToDecimal(resourceWorkSheet.Cells[rowCount, 10].Value),
                    TotalThirdPartyEstRevenue = Convert.ToDecimal(resourceWorkSheet.Cells[rowCount, 11].Value),
                    ClosePeriod = Convert.ToInt32(resourceWorkSheet.Cells[rowCount, 12].Value.ToString().Split('-')[1].Trim()),
                    ClosePeriodYear = Convert.ToInt32(resourceWorkSheet.Cells[rowCount, 12].Value.ToString().Split('-')[0].Trim()),
                    LeadPursuitPartner = Convert.ToString(resourceWorkSheet.Cells[rowCount, 13].Value),
                    SalesStage = Convert.ToString(resourceWorkSheet.Cells[rowCount, 14].Value),
                });

                rowCount++;
            }

            rowCount = 2;
            resourceWorkSheet = package.Workbook.Worksheets.Where(x => x.Name.Equals("Opportunities Status")).First();
            while (resourceWorkSheet.Cells[rowCount, 1].Value != null)
            {
                opportunityStatusData.Add(new OpportunityStatus()
                {
                    IMLead = Convert.ToString(resourceWorkSheet.Cells[rowCount, 1].Value),
                    USIOpportunity = Convert.ToString(resourceWorkSheet.Cells[rowCount, 2].Value),
                    Account = Convert.ToString(resourceWorkSheet.Cells[rowCount, 3].Value),
                    Opportunity = Convert.ToString(resourceWorkSheet.Cells[rowCount, 4].Value),
                    CurrentStatus = Convert.ToString(resourceWorkSheet.Cells[rowCount, 5].Value),
                });

                rowCount++;
            }

            repo.SetReferenceData("ActualBudget", JsonConvert.SerializeObject(actualBudgetData));
            repo.SetReferenceData("IMTime", JsonConvert.SerializeObject(IMTimeData));
            repo.SetReferenceData("Opportunities", JsonConvert.SerializeObject(opportunitiesData));
            repo.SetReferenceData("OpportunitiesStatus", JsonConvert.SerializeObject(opportunityStatusData));
        }

        //POST api/Dashboard/UpsertProject
        [HttpPost]
        public string UpsertProject([FromBody]ProjectRequest projectRequest)
        {
            if (projectRequest.action == RequestedAction.Delete)
            {
                int index = projectRequest.Projects.FindIndex(x => x.Id == projectRequest.Project.Id);
                projectRequest.Projects.RemoveAt(index);
            }
            else if (projectRequest.action == RequestedAction.Upsert)
            {
                if (projectRequest.Project.Id == null)
                {
                    projectRequest.Project.Id = DateTime.Now.ToString("dMyyHHmmss");
                    projectRequest.Projects.Add(projectRequest.Project);
                }
                else
                {
                    int index = projectRequest.Projects.FindIndex(x => x.Id == projectRequest.Project.Id);
                    if (index >= 0)
                    {
                        projectRequest.Projects[index] = projectRequest.Project;
                    }
                }
            }

            repo.SetReferenceData("Projects", JsonConvert.SerializeObject(projectRequest.Projects));

            return repo.GetReferenceData("Projects");
        }


        //POST api/Dashboard/UpsertResource
        [HttpPost]
        public string UpsertResource([FromBody]ResourceRequest resourceRequest)
        {

            if (resourceRequest.Action == RequestedAction.Delete)
            {
                int index = resourceRequest.Resources.FindIndex(x => x.Id == resourceRequest.Resource.Id);
                resourceRequest.Resources.RemoveAt(index);
            }
            else if (resourceRequest.Action == RequestedAction.Upsert)
            {
                if (resourceRequest.Resource.Id == null)
                {
                    resourceRequest.Resource.Id = DateTime.Now.ToString("dMyyHHmmss");
                    resourceRequest.Resources.Add(resourceRequest.Resource);
                }
                else
                {
                    int index = resourceRequest.Resources.FindIndex(x => x.Id == resourceRequest.Resource.Id);
                    if (index >= 0)
                    {
                        resourceRequest.Resources[index] = resourceRequest.Resource;
                    }
                }
            }

            repo.SetReferenceData("Resources", JsonConvert.SerializeObject(resourceRequest.Resources));

            return repo.GetReferenceData("Resources");
        }


        //GET api/Dashboard/GetActualVsBudgetHoursChartData //Added by Bhushan
        public List<string> GetActualVsBudgetHoursChartData(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
            {
                repo.GetReferenceData("ActualBudget");
                return ParseActualVsBudgetData(JsonConvert.DeserializeObject<List<ActualBudget>>(repo.GetReferenceData("ActualBudget")));
            }
            else
                return null;
        }

        public List<string> GetActivePursuitsChartData(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseActivePursuitsData(JsonConvert.DeserializeObject<List<Opportunities>>(repo.GetReferenceData("Opportunities")));
            else
                return null;
        }

        public List<string> GetPursuitsByLeadChartData(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParsePursuitsByLeadData(JsonConvert.DeserializeObject<List<Opportunities>>(repo.GetReferenceData("Opportunities")));
            else
                return null;
        }

        //GET api/Dashboard/GetActualUSIEngmntByIMLead //Added by Bhushan
        public List<string> GetActualUSIEngmntByIMLead(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseActualUSIEngmntByIMLead(JsonConvert.DeserializeObject<List<IMTime>>(repo.GetReferenceData("IMTime")));
            else
                return null;
        }

        public List<string> GetActualUSIEngmntByClient(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseActualUSIEngmntByClient(JsonConvert.DeserializeObject<List<IMTime>>(repo.GetReferenceData("IMTime")));
            else
                return null;
        }

        public List<string> GetQualifiedPursuitsByCustomer(string authToken)
        {
            if (authToken.Equals(AUTH_TOKEN))
                return ParseQualifiedPursuitsByCustomer(JsonConvert.DeserializeObject<List<Opportunities>>(repo.GetReferenceData("Opportunities")));
            else
                return null;
        }

        private List<string> ParseActualUSIEngmntByIMLead(List<IMTime> IMTimeData)
        {
            List<string> returnList = new List<string>();
            if (IMTimeData != null)
            {
                List<IMTime> groupedResult = IMTimeData
                     .GroupBy(s => new { s.IMLead })
                    .Select(g => new IMTime
                    {
                        IMLead = g.Key.IMLead,
                        TotalHours = g.Sum(x => x.TotalHours)
                    }).OrderBy(x => x.TotalHours).ToList();
                returnList.Add(JsonConvert.SerializeObject(groupedResult.Select(s => s.IMLead).ToList()));
                
                List<List<decimal>> embeddedData = new List<List<decimal>>();
                List<decimal> finalData = groupedResult.Select(s => s.TotalHours).ToList();
                embeddedData.Add(finalData);
                returnList.Add(JsonConvert.SerializeObject(embeddedData));
            }
            return returnList;
        }

        private List<string> ParseActualUSIEngmntByClient(List<IMTime> IMTimeData)
        {
            List<string> returnList = new List<string>();
            if (IMTimeData != null)
            {
                List<IMTime> groupedResult = IMTimeData
                    .GroupBy(s => new { s.ServiceClientName })
                   .Select(g => new IMTime
                   {
                       ServiceClientName = g.Key.ServiceClientName,
                       TotalHours = g.Sum(x => x.TotalHours)
                   }).OrderBy(x => x.TotalHours).ToList();
                returnList.Add(JsonConvert.SerializeObject(groupedResult.Select(s => s.ServiceClientName).ToList()));
                returnList.Add(JsonConvert.SerializeObject(groupedResult.Select(s => s.TotalHours).ToList()));
            }
            return returnList;
        }

        private List<string> ParseQualifiedPursuitsByCustomer(List<Opportunities> opportunitiesData)
        {
            List<string> returnList = new List<string>();
            if (opportunitiesData != null)
            {
                List<Opportunities> groupedResult = opportunitiesData
                 .Where(s => s.USIOpportunity.Contains("Proposal"))
                 .GroupBy(s => new { s.Account })
                 .Select(g => new Opportunities
                 {
                     Account = g.Key.Account,
                     TotalEstimatedRevenue = g.Sum(x => x.TotalEstimatedRevenue)
                 }).OrderBy(x => x.TotalEstimatedRevenue).ToList();

                returnList.Add(JsonConvert.SerializeObject(groupedResult.Select(x => x.Account).ToList()));
                returnList.Add(JsonConvert.SerializeObject(groupedResult.Select(x => x.TotalEstimatedRevenue).ToList()));
            }
            return returnList;
        }

        private List<string> ParseActualVsBudgetData(List<ActualBudget> actualBudgetData)
        {
            List<string> returnList = new List<string>();
            if (actualBudgetData != null)
            {
             
                var groupedResult = actualBudgetData
                     .GroupBy(s => new { s.Series, s.Period })
                    .Select(g => new
                            {
                                Series = g.Key.Series,
                                Period = g.Key.Period,
                                TotalHours = g.Sum(x => x.TotalHours)
                            }).OrderBy(x => x.Series).ThenBy(x => x.Period);

                returnList.Add(JsonConvert.SerializeObject(groupedResult.Where(x => x.Series.Equals("Actual")).Select(x => x.TotalHours).ToList()));
                returnList.Add(JsonConvert.SerializeObject(groupedResult.Where(x => x.Series.Equals("Budget")).Select(x => x.TotalHours).ToList()));
                returnList.Add("Actual");
                returnList.Add("Budget");
            }
            return returnList;

        }


        private List<string> ParsePursuitsByLeadData(List<Opportunities> opportunitiesData)
        {
            List<string> returnList = new List<string>();
            if (opportunitiesData != null)
            {
                List<Opportunities> groupedResult = opportunitiesData
                  .Where(s => s.USIOpportunity.Contains("Yes"))
                  .GroupBy(s => new { s.USIOpportunity, s.IMLead })
                  .Select(g => new Opportunities
                  {
                      USIOpportunity = g.Key.USIOpportunity,
                      IMLead = g.Key.IMLead,
                      TotalCount = g.Count()
                  }).OrderBy(x => x.USIOpportunity).ThenBy(x => x.TotalCount).ToList();

                List<string> KeyLeads = groupedResult.Where(x => x.USIOpportunity.Contains("Proposal")).OrderBy(x => x.TotalCount).GroupBy(s => s.IMLead).Select(s => s.Key).ToList();
                List<string> KeyCategories = groupedResult.GroupBy(s => s.USIOpportunity).Select(s => s.Key).ToList();

                foreach (string category in KeyCategories)
                {
                    List<int> dataValues = new List<int>();
                    foreach (string lead in KeyLeads)
                    {
                        if (groupedResult.Where(s => s.IMLead == lead && s.USIOpportunity == category).Count() > 0)
                        {
                            dataValues.Add(groupedResult.Where(s => s.IMLead == lead && s.USIOpportunity == category).Select(s => s.TotalCount).First());
                        }
                        else
                        {
                            dataValues.Add(0);
                        }
                    }
                    returnList.Add(JsonConvert.SerializeObject(dataValues));
                }

                returnList.Add(JsonConvert.SerializeObject(KeyCategories));
                returnList.Add(JsonConvert.SerializeObject(KeyLeads));


                //returnList.Add("[0, 0, 0, 1, 2, 2, 3, 4, 6]");
                //returnList.Add("[3, 4, 2, 4, 4, 0, 8, 9, 3]");
                //returnList.Add("Yes - Proposal Support");
                //returnList.Add("Yes-Still Qualifying");
            }
            return returnList;

        }

        private List<string> ParseActivePursuitsData(List<Opportunities> opportunitiesData)
        {

            List<string> returnList = new List<string>();

            if (opportunitiesData != null)
            {
                List<Opportunities> groupedResult = opportunitiesData
                    .Where(s => s.USIOpportunity.Contains("Yes"))
                    .GroupBy(s => new { s.USIOpportunity, s.ClosePeriodYear, s.ClosePeriod })
                    .Select(g => new Opportunities
                            {
                                USIOpportunity = g.Key.USIOpportunity,
                                ClosePeriodYear = g.Key.ClosePeriodYear,
                                ClosePeriod = g.Key.ClosePeriod,
                                DisplayClosePeriod = g.Key.ClosePeriodYear + "-" + g.Key.ClosePeriod.ToString("D2"),
                                TotalCount = g.Count()
                            }).OrderBy(x => x.USIOpportunity).ThenBy(x => x.ClosePeriodYear).ThenBy(x => x.ClosePeriod).ToList();

                List<string> KeyPeriods = groupedResult.OrderBy(s => s.ClosePeriodYear).ThenBy(s => s.ClosePeriod).GroupBy(s => s.DisplayClosePeriod).Select(s => s.Key).ToList();
                List<string> KeyCategories = groupedResult.GroupBy(s => s.USIOpportunity).Select(s => s.Key).ToList();

                foreach (string category in KeyCategories)
                {
                    List<int> dataValues = new List<int>();
                    foreach (string period in KeyPeriods)
                    {
                        if (groupedResult.Where(s => s.DisplayClosePeriod == period && s.USIOpportunity == category).Count() > 0)
                        {
                            dataValues.Add(groupedResult.Where(s => s.DisplayClosePeriod == period && s.USIOpportunity == category).Select(s => s.TotalCount).First());
                        }
                        else
                        {
                            dataValues.Add(0);
                        }
                    }
                    returnList.Add(JsonConvert.SerializeObject(dataValues));
                }

                returnList.Add(JsonConvert.SerializeObject(KeyPeriods));
                returnList.Add(JsonConvert.SerializeObject(KeyCategories));
            }
            return returnList;
        }



        private string GetResourceDataCount(int resourceCount, string currentDate)
        {
            int currentFY = GetFiscalYear();
            string currentFiscalYear = currentFY.ToString().Substring(2, 2);
            string nextYear = Convert.ToString(currentFY + 1).Substring(2, 2);

            Dictionary<string, int> resourceMonths = new Dictionary<string, int>();
            resourceMonths.Add(ChartMonths.Apr.ToString() + currentFiscalYear, 0);
            resourceMonths.Add(ChartMonths.May.ToString() + currentFiscalYear, 0);
            resourceMonths.Add(ChartMonths.Jun.ToString() + currentFiscalYear, 0);
            resourceMonths.Add(ChartMonths.Jul.ToString() + currentFiscalYear, 0);
            resourceMonths.Add(ChartMonths.Aug.ToString() + currentFiscalYear, 0);
            resourceMonths.Add(ChartMonths.Sep.ToString() + currentFiscalYear, 0);
            resourceMonths.Add(ChartMonths.Oct.ToString() + currentFiscalYear, 0);
            resourceMonths.Add(ChartMonths.Nov.ToString() + currentFiscalYear, 0);
            resourceMonths.Add(ChartMonths.Dec.ToString() + currentFiscalYear, 0);
            resourceMonths.Add(ChartMonths.Jan.ToString() + nextYear, 0);
            resourceMonths.Add(ChartMonths.Feb.ToString() + nextYear, 0);
            resourceMonths.Add(ChartMonths.Mar.ToString() + nextYear, 0);

            string serializedObject = JsonConvert.SerializeObject(resourceMonths);
            string savedData = repo.GetReferenceData("ResourceDataCount");
            Dictionary<string, int> SavedCollection = JsonConvert.DeserializeObject<Dictionary<string, int>>(savedData);
            Dictionary<string, int> updatedCollection = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializedObject);

            foreach (KeyValuePair<string, int> entry in resourceMonths)
            {
                if (SavedCollection.ContainsKey(entry.Key))
                {
                    updatedCollection[entry.Key] = SavedCollection[entry.Key];
                }
            }

            if (updatedCollection.ContainsKey(currentDate))
            {
                updatedCollection[currentDate] = resourceCount;
            }

            return JsonConvert.SerializeObject(updatedCollection);

        }

        private List<string> ParseResourceData(string resourceData)
        {
            List<string> Projects = new List<string>();

            List<ResourceEntity> resources = JsonConvert.DeserializeObject<List<ResourceEntity>>(resourceData);

            Projects = resources.Select(x => x.CurrentProject).ToList();

            List<GroupedProject> GroupedProjects = Projects
                .GroupBy(s => s)
                .Select(group => new GroupedProject() { Project = group.Key, Count = group.Count() }).ToList();

            List<string> chartLabelsb = new List<string>();
            List<int> chartDatab = new List<int>();

            foreach (GroupedProject pro in GroupedProjects)
            {
                chartLabelsb.Add(pro.Project);
                chartDatab.Add(pro.Count);
            }

            List<string> returnList = new List<string>();
            returnList.Add(JsonConvert.SerializeObject(chartLabelsb));
            returnList.Add(JsonConvert.SerializeObject(chartDatab));

            return returnList;

        }

        //Added by Vibhav. Create chart data from data row
        private List<string> ParseProjectData(string projectData)
        {
            List<string> Projects = new List<string>();
            List<ProjectEntity> projs = JsonConvert.DeserializeObject<List<ProjectEntity>>(projectData);

            foreach (ProjectEntity p in projs)
            {
                Projects.Add(p.Stage);
            }

            List<ProjGroupedByStatus> GroupedProjects = Projects
                .GroupBy(s => s)
                .Select(group => new ProjGroupedByStatus() { ProjectStatus = group.Key, Count = group.Count() }).ToList();
            List<string> chartLabelsb = new List<string>();
            List<int> chartDatab = new List<int>();

            foreach (ProjGroupedByStatus pro in GroupedProjects)
            {
                chartLabelsb.Add(pro.ProjectStatus);
                chartDatab.Add(pro.Count);
            }

            List<string> returnList = new List<string>();
            returnList.Add(JsonConvert.SerializeObject(chartLabelsb));
            returnList.Add(JsonConvert.SerializeObject(chartDatab));

            return returnList;

        }

        private List<string> ParseSoldProposedData(string projects)
        {
            List<ProjectEntity> projs = JsonConvert.DeserializeObject<List<ProjectEntity>>(projects);

            Dictionary<string, int> soldProjects = new Dictionary<string, int>();
            soldProjects.Add(((ChartMonths)DateTime.Now.Month).ToString() + DateTime.Now.Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(1).Month).ToString() + DateTime.Now.AddMonths(1).Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(2).Month).ToString() + DateTime.Now.AddMonths(2).Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(3).Month).ToString() + DateTime.Now.AddMonths(3).Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(4).Month).ToString() + DateTime.Now.AddMonths(4).Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(5).Month).ToString() + DateTime.Now.AddMonths(5).Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(6).Month).ToString() + DateTime.Now.AddMonths(6).Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(7).Month).ToString() + DateTime.Now.AddMonths(7).Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(8).Month).ToString() + DateTime.Now.AddMonths(8).Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(9).Month).ToString() + DateTime.Now.AddMonths(9).Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(10).Month).ToString() + DateTime.Now.AddMonths(10).Year.ToString().Substring(2, 2), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(11).Month).ToString() + DateTime.Now.AddMonths(11).Year.ToString().Substring(2, 2), 0);


            Dictionary<string, int> proposedProjects = new Dictionary<string, int>();

            proposedProjects.Add(((ChartMonths)DateTime.Now.Month).ToString() + DateTime.Now.Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(1).Month).ToString() + DateTime.Now.AddMonths(1).Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(2).Month).ToString() + DateTime.Now.AddMonths(2).Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(3).Month).ToString() + DateTime.Now.AddMonths(3).Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(4).Month).ToString() + DateTime.Now.AddMonths(4).Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(5).Month).ToString() + DateTime.Now.AddMonths(5).Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(6).Month).ToString() + DateTime.Now.AddMonths(6).Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(7).Month).ToString() + DateTime.Now.AddMonths(7).Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(8).Month).ToString() + DateTime.Now.AddMonths(8).Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(9).Month).ToString() + DateTime.Now.AddMonths(9).Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(10).Month).ToString() + DateTime.Now.AddMonths(10).Year.ToString().Substring(2, 2), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(11).Month).ToString() + DateTime.Now.AddMonths(11).Year.ToString().Substring(2, 2), 0);

            foreach (ProjectEntity project in projs)
            {
                if (ParseProjectStatus(project.Stage) == "Sold")
                {
                    PopulateProjects(soldProjects, project, ProjectAttribute.Resources);
                    PopulateProjects(proposedProjects, project, ProjectAttribute.Resources);
                }
                else
                {
                    PopulateProjects(proposedProjects, project, ProjectAttribute.Resources);
                }
            }

            List<string> chartData = new List<string>();

            chartData.Add(JsonConvert.SerializeObject(soldProjects.Keys.ToList<string>()));
            chartData.Add(JsonConvert.SerializeObject(soldProjects.Values.ToList<int>()));
            chartData.Add(JsonConvert.SerializeObject(proposedProjects.Values.ToList<int>()));

            return chartData;

        }

        private string ParseProjectStatus(string projectStage)
        {
            string projectStatus = string.Empty;

            switch (projectStage)
            {
                case "Contacted":
                    projectStatus = "Proposed";
                    break;
                case "Qualified":
                    projectStatus = "Proposed";
                    break;
                case "Proposal":
                    projectStatus = "Proposed";
                    break;
                case "Verbal Commit":
                    projectStatus = "Proposed";
                    break;
                case "Sold":
                    projectStatus = "Sold";
                    break;
                case "Design":
                    projectStatus = "Sold";
                    break;
                case "Development":
                    projectStatus = "Sold";
                    break;
                case "UAT":
                    projectStatus = "Sold";
                    break;
                case "Completed":
                    projectStatus = "Sold";
                    break;
                case "Abandoned / Lost":
                    projectStatus = "Lost";
                    break;
            }

            return projectStatus;

        }


        private static void PopulateProjects(Dictionary<string, int> projects, ProjectEntity project, ProjectAttribute attribute)
        {
            int incrementedAttribute = 0;

            if (attribute.Equals(ProjectAttribute.Resources))
            {
                incrementedAttribute = Convert.ToInt32(project.TotalResources);
            }
            else if (attribute.Equals(ProjectAttribute.Project))
            {
                incrementedAttribute = 1;

            }


            for (DateTime projectDate = Convert.ToDateTime(project.StartDate); projectDate <= Convert.ToDateTime(project.EndDate); projectDate = projectDate.AddMonths(1))
            {
                if (projects.ContainsKey(((ChartMonths)projectDate.Month).ToString() + projectDate.Year.ToString().Substring(2, 2)))
                {
                    projects[((ChartMonths)projectDate.Month).ToString() + projectDate.Year.ToString().Substring(2, 2)] += incrementedAttribute;
                }
            }
        }

        private static void PopulateEntity(Dictionary<string, int> resources, ResourcesGroupedByMonth resource)
        {
            int incrementedAttribute = 0;

            incrementedAttribute = Convert.ToInt32(resource.Count);

            for (DateTime projectDate = DateTime.Now; projectDate <= Convert.ToDateTime(ParseMonthYear(resource.Month)); projectDate = projectDate.AddMonths(1))
            {
                if (resources.ContainsKey(((ChartMonths)projectDate.Month).ToString() + projectDate.Year.ToString().Substring(2, 2)))
                {
                    resources[((ChartMonths)projectDate.Month).ToString() + projectDate.Year.ToString().Substring(2, 2)] += incrementedAttribute;
                }
            }
        }

        private static void PopulateUnallocatedEntity(Dictionary<string, int> resources, UnallocatedResourceEntity resource)
        {
            int incrementedAttribute = 1;

            for (DateTime projectDate = Convert.ToDateTime(resource.RequiredFrom); projectDate <= Convert.ToDateTime(resource.RequiredTill); projectDate = projectDate.AddMonths(1))
            {
                if (resources.ContainsKey(((ChartMonths)projectDate.Month).ToString() + projectDate.Year.ToString().Substring(2, 2)))
                {
                    resources[((ChartMonths)projectDate.Month).ToString() + projectDate.Year.ToString().Substring(2, 2)] += incrementedAttribute;
                }
            }
        }

        private List<string> ParseProjectDistributionData(string projects)
        {
            int currentFY = GetFiscalYear();
            string currentFiscalYear = currentFY.ToString().Substring(2, 2);
            string nextYear = Convert.ToString(currentFY + 1).Substring(2, 2);

            List<ProjectEntity> projs = JsonConvert.DeserializeObject<List<ProjectEntity>>(projects);

            Dictionary<string, int> totalProjects = new Dictionary<string, int>();
            totalProjects.Add(ChartMonths.Apr.ToString() + currentFiscalYear, 0);
            totalProjects.Add(ChartMonths.May.ToString() + currentFiscalYear, 0);
            totalProjects.Add(ChartMonths.Jun.ToString() + currentFiscalYear, 0);
            totalProjects.Add(ChartMonths.Jul.ToString() + currentFiscalYear, 0);
            totalProjects.Add(ChartMonths.Aug.ToString() + currentFiscalYear, 0);
            totalProjects.Add(ChartMonths.Sep.ToString() + currentFiscalYear, 0);
            totalProjects.Add(ChartMonths.Oct.ToString() + currentFiscalYear, 0);
            totalProjects.Add(ChartMonths.Nov.ToString() + currentFiscalYear, 0);
            totalProjects.Add(ChartMonths.Dec.ToString() + currentFiscalYear, 0);
            totalProjects.Add(ChartMonths.Jan.ToString() + nextYear, 0);
            totalProjects.Add(ChartMonths.Feb.ToString() + nextYear, 0);
            totalProjects.Add(ChartMonths.Mar.ToString() + nextYear, 0);

            foreach (ProjectEntity project in projs)
            {
                if (ParseProjectStatus(project.Stage) == "Sold")
                {
                    PopulateProjects(totalProjects, project, ProjectAttribute.Project);
                }
            }

            List<string> chartData = new List<string>();

            chartData.Add(JsonConvert.SerializeObject(totalProjects.Keys.ToList<string>()));
            chartData.Add(JsonConvert.SerializeObject(totalProjects.Values.ToList<int>()));

            return chartData;


        }

        private int GetFiscalYear()
        {
            return (DateTime.Now.Month >= 4 ? DateTime.Now.Year : (DateTime.Now.Year - 1));
        }

        private List<string> ParseSKillData(string skillData)
        {
            List<ProjectEntity> projs = JsonConvert.DeserializeObject<List<ProjectEntity>>(skillData);

            List<ResourceBySkill> resourceList = projs.Where(p => p.Stage == "Sold").GroupBy(e => e.Technology)
               .Select(ls => new ResourceBySkill()
               {
                   label = ls.Key,
                   value = ls.Sum(w => Convert.ToInt32(w.TotalResources))
               }).ToList();

            //foreach (var data in resourceList)
            //{
            //    data.color = "";
            //}

            List<string> returnList = new List<string>();

            returnList.Add(JsonConvert.SerializeObject(resourceList));

            return returnList;

        }

        //Added by Vibhav. 
        private List<string> ParseProjBySkillData(string projectData)
        {
            List<string> Projects = new List<string>();
            List<ProjectEntity> projs = JsonConvert.DeserializeObject<List<ProjectEntity>>(projectData);

            foreach (ProjectEntity p in projs)
            {
                Projects.Add(p.Technology);
            }

            List<ProjGroupedByStatus> GroupedProjects = Projects
                .GroupBy(s => s)
                .Select(group => new ProjGroupedByStatus() { ProjectStatus = group.Key, Count = group.Count() }).ToList();

            //foreach (var data in resourceList)
            //{
            //    data.color = "";
            //}

            List<string> returnList = new List<string>();

            returnList.Add(JsonConvert.SerializeObject(GroupedProjects));

            return returnList;

        }

        private List<string> ParseDashboardCounts(Dictionary<string, string> dashboardData)
        {
            List<string> dashboardCounts = new List<string>();

            dashboardCounts.Add(JsonConvert.DeserializeObject<List<Invoice>>(dashboardData["Invoices"]).Where(x => x.PaymentReceived == "Pending").Count().ToString());
            dashboardCounts.Add(JsonConvert.DeserializeObject<List<ProjectEntity>>(dashboardData["Projects"]).Where(x => x.Stage == "Sold").Count().ToString());
            dashboardCounts.Add(JsonConvert.DeserializeObject<List<ActionItem>>(dashboardData["NewToDoItems"]).Where(x => x.Status == "Open").Count().ToString());
            dashboardCounts.Add(JsonConvert.DeserializeObject<List<ResourceEntity>>(dashboardData["GSSResources"]).Count.ToString());

            return dashboardCounts;

        }

        private string ParseProject(string currentProject)
        {
            string cProject = string.Empty;

            if (currentProject.IndexOf(",") > 0)
            {

                foreach (string project in currentProject.Split(','))
                {
                    if (project.Contains("AU"))
                    {
                        cProject = project.Split('(')[0].Trim().Split('_')[0];
                    }
                }

            }
            else
            {
                if (currentProject.Contains("AU"))
                {
                    cProject = currentProject.Split('(')[0].Trim().Split('_')[0];
                }

            }

            return cProject;
        }

        private List<string> ParseResourceDeploymentChartData(string ResourceDataCount)
        {
            List<string> returnList = new List<string>();

            Dictionary<string, int> SavedCollection = JsonConvert.DeserializeObject<Dictionary<string, int>>(ResourceDataCount);

            returnList.Add(JsonConvert.SerializeObject(SavedCollection.Keys.ToList<string>()));
            returnList.Add(JsonConvert.SerializeObject(SavedCollection.Values.ToList<int>()));

            return returnList;

        }

        private List<string> ParseSoldProposedData(string resources, string GSSresources)
        {
            List<ResourceEntity> gssResources = JsonConvert.DeserializeObject<List<ResourceEntity>>(GSSresources);
            List<UnallocatedResourceEntity> unallocatedResources = JsonConvert.DeserializeObject<List<UnallocatedResourceEntity>>(resources);
            List<ProjectEntity> projects = JsonConvert.DeserializeObject<List<ProjectEntity>>(repo.GetReferenceData("Projects"));

            List<ResourcesGroupedByMonth> GroupedGssResources = gssResources
                .Where(s => s.AvailableOn != string.Empty)
                .GroupBy(s => Convert.ToDateTime(s.AvailableOn).ToString("MMMyy"))
                .Select(group => new ResourcesGroupedByMonth() { Month = group.Key, Count = group.Count() }).ToList();

            List<UnallocatedResourceEntity> ProposedGssResources = gssResources
                .Where(s => s.AvailableOn == string.Empty && s.CurrentProject == string.Empty && s.NextProject != string.Empty)
                .Select(s => new UnallocatedResourceEntity() { RequiredFrom = s.ProposedStartDate, RequiredTill = s.ProposedEndDate }).ToList();

            //List<ResourcesGroupedByMonth> GroupedUnallocatedResources = unallocatedResources
            //    .GroupBy(s => Convert.ToDateTime(s.RequiredFrom).ToString("MMMyy"))
            //    .Select(group => new ResourcesGroupedByMonth() { Month = group.Key, Count = group.Count() }).ToList();

            Dictionary<string, int> soldEntity = new Dictionary<string, int>();
            soldEntity.Add(((ChartMonths)DateTime.Now.Month).ToString() + DateTime.Now.Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(1).Month).ToString() + DateTime.Now.AddMonths(1).Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(2).Month).ToString() + DateTime.Now.AddMonths(2).Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(3).Month).ToString() + DateTime.Now.AddMonths(3).Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(4).Month).ToString() + DateTime.Now.AddMonths(4).Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(5).Month).ToString() + DateTime.Now.AddMonths(5).Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(6).Month).ToString() + DateTime.Now.AddMonths(6).Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(7).Month).ToString() + DateTime.Now.AddMonths(7).Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(8).Month).ToString() + DateTime.Now.AddMonths(8).Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(9).Month).ToString() + DateTime.Now.AddMonths(9).Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(10).Month).ToString() + DateTime.Now.AddMonths(10).Year.ToString().Substring(2, 2), 0);
            soldEntity.Add(((ChartMonths)DateTime.Now.AddMonths(11).Month).ToString() + DateTime.Now.AddMonths(11).Year.ToString().Substring(2, 2), 0);


            Dictionary<string, int> proposedEntity = new Dictionary<string, int>();

            proposedEntity.Add(((ChartMonths)DateTime.Now.Month).ToString() + DateTime.Now.Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(1).Month).ToString() + DateTime.Now.AddMonths(1).Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(2).Month).ToString() + DateTime.Now.AddMonths(2).Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(3).Month).ToString() + DateTime.Now.AddMonths(3).Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(4).Month).ToString() + DateTime.Now.AddMonths(4).Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(5).Month).ToString() + DateTime.Now.AddMonths(5).Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(6).Month).ToString() + DateTime.Now.AddMonths(6).Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(7).Month).ToString() + DateTime.Now.AddMonths(7).Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(8).Month).ToString() + DateTime.Now.AddMonths(8).Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(9).Month).ToString() + DateTime.Now.AddMonths(9).Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(10).Month).ToString() + DateTime.Now.AddMonths(10).Year.ToString().Substring(2, 2), 0);
            proposedEntity.Add(((ChartMonths)DateTime.Now.AddMonths(11).Month).ToString() + DateTime.Now.AddMonths(11).Year.ToString().Substring(2, 2), 0);

            foreach (ResourcesGroupedByMonth resource in GroupedGssResources)
            {
                PopulateEntity(soldEntity, resource);
                PopulateEntity(proposedEntity, resource);
            }

            foreach (UnallocatedResourceEntity unallocatedResource in unallocatedResources)
            {
                PopulateUnallocatedEntity(proposedEntity, unallocatedResource);
            }

            foreach (UnallocatedResourceEntity unallocatedResource in ProposedGssResources)
            {
                PopulateUnallocatedEntity(proposedEntity, unallocatedResource);
            }

            List<string> chartData = new List<string>();

            chartData.Add(JsonConvert.SerializeObject(soldEntity.Keys.ToList<string>()));
            chartData.Add(JsonConvert.SerializeObject(soldEntity.Values.ToList<int>()));
            chartData.Add(JsonConvert.SerializeObject(proposedEntity.Values.ToList<int>()));

            return chartData;

        }

        private static string ParseMonthYear(string MMMyy)
        {
            string output = MMMyy.Substring(0, 3) + (Convert.ToInt32(MMMyy.Substring(3, 2)) + 2000).ToString();


            return output;

        }

        private List<string> ParseRevenueData(string invoiceData)
        {
            List<Invoice> invoices = JsonConvert.DeserializeObject<List<Invoice>>(invoiceData);
            List<Invoice> relevantCurrentInvoices = new List<Invoice>();
            List<Invoice> relevantPreviousInvoices = new List<Invoice>();

            int currentFY = GetFiscalYear();
            string currentFiscalYear = currentFY.ToString().Substring(2, 2);
            string nextYear = Convert.ToString(currentFY + 1).Substring(2, 2);
            string previousYear = Convert.ToString(currentFY - 1).Substring(2, 2);

            Dictionary<string, int> previousRevenueMonths = new Dictionary<string, int>();
            previousRevenueMonths.Add(ChartMonths.Apr.ToString() + "-" + previousYear, 0);
            previousRevenueMonths.Add(ChartMonths.May.ToString() + "-" + previousYear, 0);
            previousRevenueMonths.Add(ChartMonths.Jun.ToString() + "-" + previousYear, 0);
            previousRevenueMonths.Add(ChartMonths.Jul.ToString() + "-" + previousYear, 0);
            previousRevenueMonths.Add(ChartMonths.Aug.ToString() + "-" + previousYear, 0);
            previousRevenueMonths.Add(ChartMonths.Sep.ToString() + "-" + previousYear, 0);
            previousRevenueMonths.Add(ChartMonths.Oct.ToString() + "-" + previousYear, 0);
            previousRevenueMonths.Add(ChartMonths.Nov.ToString() + "-" + previousYear, 0);
            previousRevenueMonths.Add(ChartMonths.Dec.ToString() + "-" + previousYear, 0);
            previousRevenueMonths.Add(ChartMonths.Jan.ToString() + "-" + currentFiscalYear, 0);
            previousRevenueMonths.Add(ChartMonths.Feb.ToString() + "-" + currentFiscalYear, 0);
            previousRevenueMonths.Add(ChartMonths.Mar.ToString() + "-" + currentFiscalYear, 0);

            Dictionary<string, int> currentRevenueMonths = new Dictionary<string, int>();
            currentRevenueMonths.Add(ChartMonths.Apr.ToString() + "-" + currentFiscalYear, 0);
            currentRevenueMonths.Add(ChartMonths.May.ToString() + "-" + currentFiscalYear, 0);
            currentRevenueMonths.Add(ChartMonths.Jun.ToString() + "-" + currentFiscalYear, 0);
            currentRevenueMonths.Add(ChartMonths.Jul.ToString() + "-" + currentFiscalYear, 0);
            currentRevenueMonths.Add(ChartMonths.Aug.ToString() + "-" + currentFiscalYear, 0);
            currentRevenueMonths.Add(ChartMonths.Sep.ToString() + "-" + currentFiscalYear, 0);
            currentRevenueMonths.Add(ChartMonths.Oct.ToString() + "-" + currentFiscalYear, 0);
            currentRevenueMonths.Add(ChartMonths.Nov.ToString() + "-" + currentFiscalYear, 0);
            currentRevenueMonths.Add(ChartMonths.Dec.ToString() + "-" + currentFiscalYear, 0);
            currentRevenueMonths.Add(ChartMonths.Jan.ToString() + "-" + nextYear, 0);
            currentRevenueMonths.Add(ChartMonths.Feb.ToString() + "-" + nextYear, 0);
            currentRevenueMonths.Add(ChartMonths.Mar.ToString() + "-" + nextYear, 0);


            relevantCurrentInvoices = invoices.FindAll(e => currentRevenueMonths.ContainsKey(Convert.ToDateTime(e.Period).ToString("MMM-yy")));
            decimal sumCurrentYear = relevantCurrentInvoices.Sum(e => e.Amount);

            relevantPreviousInvoices = invoices.FindAll(e => previousRevenueMonths.ContainsKey(Convert.ToDateTime(e.Period).ToString("MMM-yy")));
            decimal sumPreviousYear = relevantPreviousInvoices.Sum(e => e.Amount);

            List<string> returnList = new List<string>();

            string currFY = "FY" + (currentFY).ToString().Substring(2, 2);
            string prevFY = "FY" + (currentFY - 1).ToString().Substring(2, 2);

            //send current year data
            returnList.Add(JsonConvert.SerializeObject(relevantCurrentInvoices.Select(x => x.Amount)));
            //send previous year data
            returnList.Add(JsonConvert.SerializeObject(relevantPreviousInvoices.Select(x => x.Amount)));
            //send current, prev year FY
            returnList.Add(JsonConvert.SerializeObject(currFY));
            returnList.Add(JsonConvert.SerializeObject(prevFY));
            returnList.Add(JsonConvert.SerializeObject(new List<string> { sumPreviousYear.ToString(), sumCurrentYear.ToString() }));
            returnList.Add(JsonConvert.SerializeObject(new List<string> { prevFY, currFY }));
            return returnList;

        }

        private string ParseProposedProject(string proposedProject, string dateType)
        {
            DateTime proposedDate = DateTime.Now;

            if (proposedProject.IndexOf(",") > 0)
            {

                foreach (string project in proposedProject.Split(','))
                {
                    if (project.Contains("AU") && project.Contains('-') && project.Contains('('))
                    {
                        if (dateType == "StartDate")
                        {
                            proposedDate = Convert.ToDateTime(project.Split('(')[1].Trim().Split('-')[0]);
                        }
                        else
                        {
                            proposedDate = Convert.ToDateTime(project.Split('(')[1].Trim().Split('-')[1].TrimEnd(')'));
                        }
                    }
                }

            }
            else
            {
                if (proposedProject.Contains("AU") && proposedProject.Contains('-') && proposedProject.Contains('('))
                {
                    if (dateType == "StartDate")
                    {
                        proposedDate = Convert.ToDateTime(proposedProject.Split('(')[1].Trim().Split('-')[0]);
                    }
                    else
                    {
                        proposedDate = Convert.ToDateTime(proposedProject.Split('(')[1].Trim().Split('-')[1].TrimEnd(')'));
                    }
                }

            }

            return proposedDate.ToString();



        }

    }
}
