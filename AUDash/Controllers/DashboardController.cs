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
            return 40;
        }

        //GET api/Dashboard/GetReferenceData
        public string GetReferenceData(string storageId)
        {
            string response = repo.GetReferenceData(storageId);
            return response == string.Empty ? null : response;
        }

        //POST api/Dashboard/SetReferenceData
        [HttpPost]
        public void SetReferenceData([FromBody] string referenceData)
        {
            ReferenceData refData = JsonConvert.DeserializeObject<ReferenceData>(referenceData);
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
        public void UploadFile()
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

    }
}
