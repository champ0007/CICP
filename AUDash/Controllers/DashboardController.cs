﻿using AUDash.Models;
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


namespace AUDash.Controllers
{
    public class DashboardController : ApiController
    {
        DBRepository repo = new DBRepository();

        //GET api/Dashboard/GetDashboardCounts
        public string GetDashboardCounts()
        {
            List<string> dashboardCounts = ParseDashboardCounts(repo.GetDashboardCounts());

            return JsonConvert.SerializeObject(dashboardCounts);

        }

        //GET api/Dashboard/GetProjectChartData //Added by Vibhav
        public List<string> GetProjectChartData()
        {
            return ParseProjectData(repo.GetReferenceData("Projects"));
        }

        //GET api/Dashboard/GetRevenueChartData //Added by Vibhav
        public List<string> GetRevenueChartData()
        {
            return ParseRevenueData(repo.GetReferenceData("Invoices"));
        }

        //GET api/Dashboard/GetTechChartData //Added by Vibhav
        public List<string> GetTechChartData()
        {
            return ParseSKillData(repo.GetReferenceData("Projects"));
        }

        //GET api/Dashboard/GetProjChartData //Added by Vibhav
        public List<string> GetProjChartData()
        {
            return ParseProjBySkillData(repo.GetReferenceData("Projects"));
        }
        //GET api/GetSoldProposedChartData
        public List<string> GetSoldProposedChartData()
        {
            //return ParseSoldProposedData(repo.GetReferenceData("Projects"));
            return ParseSoldProposedData(repo.GetReferenceData("Resources"), repo.GetReferenceData("GSSResources"));
        }

        //GET api/Dashboard/GetReferenceData
        public string GetReferenceData(string storageId)
        {
            string response = repo.GetReferenceData(storageId);
            return response == string.Empty ? null : response;
        }

        //GET api/Dashboard/GetResourceChartData
        public List<string> GetResourceChartData()
        {
            return ParseResourceData(repo.GetReferenceData("GSSResources"));
        }

        //GET api/Dashboard/GetProjectDistributionChartData
        public List<string> GetProjectDistributionChartData()
        {
            return ParseProjectDistributionData(repo.GetReferenceData("Projects"));
        }

        //GET api/Dashboard/GetResourceDeploymentChartData
        public List<string> GetResourceDeploymentChartData()
        {
            return ParseResourceDeploymentChartData(repo.GetReferenceData("ResourceDataCount"));
        }

        //POST api/Dashboard/SetReferenceData
        [HttpPost]
        public void SetReferenceData([FromBody] string referenceData)
        {
            ReferenceData refData = JsonConvert.DeserializeObject<ReferenceData>(referenceData);
            //Set Session Values

            //HttpContext.Current.Session[refData.storageId] = refData.storageData;
            //string sessionvalue = Convert.ToString(HttpContext.Current.Session[refData.storageId]);

            repo.SetReferenceData(refData.storageId, refData.storageData);
        }

        //GET api/Dashboard/GetResourceList
        public string GetResourceList()
        {
            //List<Resource> Resources = new List<Resource>();
            //Resources.Add(new Resource() { FirstName = "Shakil", LastName = "Shaikh", CurrentProject = "Telstra", ProposedProject = "None", Level = "Manager", AvailableOn = "01-Dec-2014", Skills = "Adobe CQ", StartDate = "01-Mar-2014" });
            //Resources.Add(new Resource() { FirstName = "Shakil", LastName = "Shaikh", CurrentProject = "Telstra", ProposedProject = "None", Level = "Manager", AvailableOn = "01-Dec-2014", Skills = "Adobe CQ", StartDate = "01-Mar-2014" });
            //Resources.Add(new Resource() { FirstName = "Shakil", LastName = "Shaikh", CurrentProject = "Telstra", ProposedProject = "None", Level = "Manager", AvailableOn = "01-Dec-2014", Skills = "Adobe CQ", StartDate = "01-Mar-2014" });
            //Resources.Add(new Resource() { FirstName = "Shakil", LastName = "Shaikh", CurrentProject = "Telstra", ProposedProject = "None", Level = "Manager", AvailableOn = "01-Dec-2014", Skills = "Adobe CQ", StartDate = "01-Mar-2014" });
            //Resources.Add(new Resource() { FirstName = "Shakil", LastName = "Shaikh", CurrentProject = "Telstra", ProposedProject = "None", Level = "Manager", AvailableOn = "01-Dec-2014", Skills = "Adobe CQ", StartDate = "01-Mar-2014" });

            return JSONConcat(repo.GetAllResources());
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
        public string GetKeyUpdates()
        {
            List<KeyUpdates> kUpdates = new List<KeyUpdates>();
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
                            Period = Convert.ToDateTime(Convert.ToString(invoiceWorkSheet.Cells[rowCount, 5].Value)).ToString("MMM-yy"),
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
                    ExcelWorksheet resourceWorkSheet = package.Workbook.Worksheets.Where(x => x.Name.Contains("US-I")).First();
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
            //"{\"Apr14\":30,\"May14\":30,\"Jun14\":45,\"Jul14\":45,\"Aug14\":51,\"Sep14\":51,\"Oct14\":51,\"Nov14\":51,\"Dec14\":51,\"Jan15\":0,\"Feb15\":0,\"Mar15\":0}";
            //string baseData1 = "{\"Jan14\":30,\"Feb14\":30,\"Mar14\":45,\"Jul14\":45,\"Aug14\":51,\"Sep14\":51,\"Oct14\":51,\"Nov14\":51,\"Dec14\":51,\"Jan15\":35,\"Feb15\":0,\"Mar15\":0}";
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

        //Added by Vibhav. Create chart data from data row
        //private List<string> ParseRevenueData(string projectData)
        //{
        //    List<Invoice> monthlyRev = JsonConvert.DeserializeObject<List<Invoice>>(projectData);
        //    //get current year
        //    int currentYear = DateTime.Today.Year - 2000;

        //    // fiscal year is from april to march, so for jan, feb, mar are in previous fiscal
        //    if (DateTime.Today.Month == 1 || DateTime.Today.Month == 2 || DateTime.Today.Month == 3)
        //    {
        //        currentYear = currentYear - 1;
        //    }
        //    List<string> chartLabelsb = new List<string>();
        //    List<int> chartDatab = new List<int>();

        //    //fetch only rows with current/prev/next year data
        //    List<Invoice> relevantData = new List<Invoice>();
        //    //relevantData = monthlyRev
        //    //    .FindAll(e => !String.IsNullOrEmpty(e.ATBApprovalSentOn) && !e.ATBApprovalSentOn.ToLower().Contains("n") && !e.ATBApprovalSentOn.ToLower().Contains("-") && (e.ATBApprovalSentOn.Substring(e.ATBApprovalSentOn.Length - 4, 4) == currentYear.ToString() || e.ATBApprovalSentOn.Substring(e.ATBApprovalSentOn.Length - 4, 4) == (currentYear - 1).ToString() || e.ATBApprovalSentOn.Substring(e.ATBApprovalSentOn.Length - 4, 4) == (currentYear + 1).ToString()));

        //    //relevantData[1].Period.Substring(relevantData[1].Period.IndexOf('/',2)+1,4)

        //    //var a = monthlyRev.Select(i => i.Period.Substring(0,2)).Distinct();
        //    //var aaaaa = monthlyRev.Select(i => i.Period.Substring(i.Period.Length-4)).Distinct();//"1/10/2013".ToString().Substring("1/10/2013".LastIndexOf('/')+1)

        //    relevantData = monthlyRev
        //        .FindAll(e => !String.IsNullOrEmpty(e.Period) && (e.Period.Substring(4, 2) == (currentYear - 1).ToString() || e.Period.Substring(4, 2) == currentYear.ToString() || e.Period.Substring(4, 2) == (currentYear + 1).ToString()));

        //    //var b = relevantData.Select(i => i.Period.Substring(0, 2)).Distinct();
        //    //var bbbbb = relevantData.Select(i => i.Period.Substring(i.Period.IndexOf('/', 2) + 1, 4)).Distinct();

        //    List<RevenueByMonth> currYrData = new List<RevenueByMonth>();
        //    List<RevenueByMonth> prevYrData = new List<RevenueByMonth>();
        //    RevenueByMonth currDt;
        //    RevenueByMonth prevDt;
        //    bool roundComplte = false;
        //    //start with april
        //    for (int i = 4; i <= 12; i++)
        //    {
        //        // when all month's data added, break the loop
        //        if (roundComplte && i == 4)
        //        {
        //            break;
        //        }
        //        double currRevenue = 0.0;
        //        double prevRevenue = 0.0;
        //        currDt = new RevenueByMonth();
        //        prevDt = new RevenueByMonth();
        //        foreach (var rec in relevantData)
        //        {
        //            //Console.WriteLine("i= " + i + " : " + rec.Period);
        //            //if data is for current(latest) year
        //            //if (rec.ATBApprovalSentOn.Substring(rec.ATBApprovalSentOn.Length - 4, 4) == currentYear.ToString())
        //            if (rec.Period.Substring(4, 2) == (currentYear).ToString())
        //            {
        //                //if data is for selected month
        //                // if (rec.ATBApprovalSentOn.Substring(0, rec.ATBApprovalSentOn.IndexOf('/')) == i.ToString() || rec.ATBApprovalSentOn.Substring(0, rec.ATBApprovalSentOn.IndexOf('/')) == "0" + i.ToString())
        //                if (rec.Period.Substring(0, rec.Period.IndexOf('/')) == i.ToString() || rec.Period.Substring(0, rec.Period.IndexOf('/')) == "0" + i.ToString())
        //                {
        //                    //add revenue to month's total
        //                    if (rec.Amount >= 0)
        //                    {
        //                        currRevenue = currRevenue + Convert.ToDouble(rec.Amount);
        //                    }
        //                }
        //            }
        //            //if data is for previous year
        //            //if (rec.ATBApprovalSentOn.Substring(rec.ATBApprovalSentOn.Length - 4, 4) == (currentYear - 1).ToString())
        //            if (rec.Period.Substring(4, 2) == (currentYear - 1).ToString())
        //            {
        //                //if (rec.ATBApprovalSentOn.Substring(0, rec.ATBApprovalSentOn.IndexOf('/')) == i.ToString() || rec.ATBApprovalSentOn.Substring(0, rec.ATBApprovalSentOn.IndexOf('/')) == "0" + i.ToString())
        //                if (rec.Period.Substring(0, rec.Period.IndexOf('/')) == i.ToString() || rec.Period.Substring(0, rec.Period.IndexOf('/')) == "0" + i.ToString())
        //                {
        //                    prevRevenue = prevRevenue + Convert.ToDouble(rec.Amount);
        //                }
        //            }
        //        }

        //        currDt.amount = Math.Round(currRevenue);
        //        currDt.month = i;
        //        prevDt.amount = Math.Round(prevRevenue);
        //        prevDt.month = i;

        //        currYrData.Add(currDt);
        //        prevYrData.Add(prevDt);

        //        //use this to fetch data for jan, feb. mar at the end
        //        if (i == 12)
        //        {
        //            i = 0;
        //            roundComplte = true;
        //            currentYear = currentYear + 1;
        //        }
        //    }
        //    string currentFY = "FY" + (currentYear).ToString().Substring(2, 2);
        //    string prevFY = "FY" + (currentYear - 1).ToString().Substring(2, 2);
        //    List<string> returnList = new List<string>();
        //    //send current year data
        //    returnList.Add(JsonConvert.SerializeObject(currYrData.Select(e => e.amount)));
        //    //send previous year data
        //    returnList.Add(JsonConvert.SerializeObject(prevYrData.Select(e => e.amount)));
        //    //send current, prev year FY
        //    returnList.Add(JsonConvert.SerializeObject(currentFY));
        //    returnList.Add(JsonConvert.SerializeObject(prevFY));
        //    returnList.Add(JsonConvert.SerializeObject(new List<string> { prevYrData.Sum(e => Math.Round(e.amount / 1000000, 2)).ToString(), currYrData.Sum(e => Math.Round(e.amount / 1000000, 2)).ToString() }));
        //    returnList.Add(JsonConvert.SerializeObject(new List<string> { prevFY, currentFY }));
        //    return returnList;

        //}

        //Added by Vibhav. 
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
            dashboardCounts.Add(JsonConvert.DeserializeObject<List<ProjectEntity>>(dashboardData["Projects"]).Count.ToString());
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
                .GroupBy(s => Convert.ToDateTime(s.AvailableOn).ToString("MMMyy"))
                .Select(group => new ResourcesGroupedByMonth() { Month = group.Key, Count = group.Count() }).ToList();

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
            currentRevenueMonths.Add(ChartMonths.Apr.ToString() + "-" +  currentFiscalYear, 0);
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


            relevantCurrentInvoices = invoices.FindAll(e => currentRevenueMonths.ContainsKey(e.Period));
            decimal sumCurrentYear = relevantCurrentInvoices.Sum(e => e.Amount);

            relevantPreviousInvoices = invoices.FindAll(e => previousRevenueMonths.ContainsKey(e.Period));
            decimal sumPreviousYear = relevantPreviousInvoices.Sum(e => e.Amount);

            List<string> returnList = new List<string>();

            string currFY = "FY" + (currentFY).ToString().Substring(2, 2);
            string prevFY = "FY" + (currentFY - 1).ToString().Substring(2, 2);

            //send current year data
            returnList.Add(JsonConvert.SerializeObject(relevantCurrentInvoices.Select(x=> x.Amount)));
            //send previous year data
            returnList.Add(JsonConvert.SerializeObject(relevantPreviousInvoices.Select(x=> x.Amount)));
            //send current, prev year FY
            returnList.Add(JsonConvert.SerializeObject(currFY));
            returnList.Add(JsonConvert.SerializeObject(prevFY));
            returnList.Add(JsonConvert.SerializeObject(new List<string> { sumPreviousYear.ToString(), sumCurrentYear.ToString() }));
            returnList.Add(JsonConvert.SerializeObject(new List<string> { prevFY, currFY }));
            return returnList;

        }


    }
}
