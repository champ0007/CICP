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


namespace AUDash.Controllers
{
    public class DashboardController : ApiController
    {
        DBRepository repo = new DBRepository();

        //GET api/Dashboard/getactiveprojects
        public int GetActiveProjects()
        {
            return 11;
        }

        //GET api/Dashboard/getopentasks
        public int GetOpenTasks()
        {
            return 20;
        }

        //GET api/Dashboard/getpendinginvoices
        public int GetPendingInvoices()
        {
            return 30;
        }

        //GET api/Dashboard/getactiveresources
        public int GetActiveResources()
        {
            return 400;
        }

        //GET api/Dashboard/GetProjectChartData //Added by Vibhav
        public List<string> GetProjectChartData()
        {
            return ParseProjectData(repo.GetReferenceData("Projects"));
        }

        //GET api/GetSoldProposedChartData
        public List<string> GetSoldProposedChartData()
        {
            return ParseSoldProposedData(repo.GetReferenceData("Projects"));
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
            return ParseResourceData(repo.GetReferenceData("Resources"));
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
                            Period = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 5].Value),
                            Date = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 6].Value),
                            Hours = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 7].Value),
                            Amount = Convert.ToString(invoiceWorkSheet.Cells[rowCount, 8].Value),
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
                    ExcelWorksheet resourceWorkSheet = package.Workbook.Worksheets["US-I"];
                    while (resourceWorkSheet.Cells[rowCount, 1].Value != null)
                    {
                        resources.Add(new ResourceEntity()
                        {
                            FirstName = Convert.ToString(resourceWorkSheet.Cells[rowCount, 1].Value).Split(',')[1].Trim(),
                            LastName = Convert.ToString(resourceWorkSheet.Cells[rowCount, 1].Value).Split(',')[0].Trim(),
                            Skills = Convert.ToString(resourceWorkSheet.Cells[rowCount, 2].Value),
                            Level = Convert.ToString(resourceWorkSheet.Cells[rowCount, 4].Value),
                            LastProject = String.Empty,
                            CurrentProject = Convert.ToString(resourceWorkSheet.Cells[rowCount, 17].Value),
                            NextProject = String.Empty,
                            AvailableOn = Convert.ToString(resourceWorkSheet.Cells[5, 19].Value).Split(' ')[0]
                        });

                        rowCount++;
                    }
                }
            }

            DBRepository repo = new DBRepository();
            repo.SetReferenceData("Resources", JsonConvert.SerializeObject(resources));
        }

        private List<string> ParseResourceData(string resourceData)
        {
            List<string> Projects = new List<string>();

            List<ResourceEntity> resources = JsonConvert.DeserializeObject<List<ResourceEntity>>(resourceData);
            foreach (ResourceEntity resource in resources)
            {
                if (resource.CurrentProject.IndexOf(",") > 0)
                {

                    foreach (string project in resource.CurrentProject.Split(','))
                    {
                        if (project.Contains("AU"))
                        {
                            Projects.Add(project.Split('(')[0].Trim().Split('_')[0]);
                        }
                    }

                }
                else
                {
                    if (resource.CurrentProject.Contains("AU"))
                    {
                        Projects.Add(resource.CurrentProject.Split('(')[0].Trim().Split('_')[0]);
                    }

                }
            }

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
            soldProjects.Add(((ChartMonths)DateTime.Now.Month).ToString() + DateTime.Now.Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(1).Month).ToString() + DateTime.Now.AddMonths(1).Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(2).Month).ToString() + DateTime.Now.AddMonths(2).Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(3).Month).ToString() + DateTime.Now.AddMonths(3).Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(4).Month).ToString() + DateTime.Now.AddMonths(4).Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(5).Month).ToString() + DateTime.Now.AddMonths(5).Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(6).Month).ToString() + DateTime.Now.AddMonths(6).Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(7).Month).ToString() + DateTime.Now.AddMonths(7).Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(8).Month).ToString() + DateTime.Now.AddMonths(8).Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(9).Month).ToString() + DateTime.Now.AddMonths(9).Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(10).Month).ToString() + DateTime.Now.AddMonths(10).Year.ToString(), 0);
            soldProjects.Add(((ChartMonths)DateTime.Now.AddMonths(11).Month).ToString() + DateTime.Now.AddMonths(11).Year.ToString(), 0);


            Dictionary<string, int> proposedProjects = new Dictionary<string, int>();

            proposedProjects.Add(((ChartMonths)DateTime.Now.Month).ToString() + DateTime.Now.Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(1).Month).ToString() + DateTime.Now.AddMonths(1).Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(2).Month).ToString() + DateTime.Now.AddMonths(2).Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(3).Month).ToString() + DateTime.Now.AddMonths(3).Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(4).Month).ToString() + DateTime.Now.AddMonths(4).Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(5).Month).ToString() + DateTime.Now.AddMonths(5).Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(6).Month).ToString() + DateTime.Now.AddMonths(6).Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(7).Month).ToString() + DateTime.Now.AddMonths(7).Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(8).Month).ToString() + DateTime.Now.AddMonths(8).Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(9).Month).ToString() + DateTime.Now.AddMonths(9).Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(10).Month).ToString() + DateTime.Now.AddMonths(10).Year.ToString(), 0);
            proposedProjects.Add(((ChartMonths)DateTime.Now.AddMonths(11).Month).ToString() + DateTime.Now.AddMonths(11).Year.ToString(), 0);

            foreach (ProjectEntity project in projs)
            {
                if (ParseProjectStatus(project.Stage) == "Sold")
                {
                    PopulateProjects(soldProjects, project);
                    PopulateProjects(proposedProjects, project);
                }
                else
                {
                    PopulateProjects(proposedProjects, project);
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


        private static void PopulateProjects(Dictionary<string, int> projects, ProjectEntity project)
        {
            for (DateTime projectDate = Convert.ToDateTime(project.StartDate); projectDate <= Convert.ToDateTime(project.EndDate); projectDate = projectDate.AddMonths(1))
            {
                if (projects.ContainsKey(((ChartMonths)projectDate.Month).ToString() + projectDate.Year.ToString()))
                {
                    projects[((ChartMonths)projectDate.Month).ToString() + projectDate.Year.ToString()] += Convert.ToInt32(project.TotalResources);
                }
            }
        }



    }
}
