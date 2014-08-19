using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AUDashboard.Controllers
{
    public class DashboardController : ApiController
    {
        [HttpGet]
        public int GetActiveProjects()
        {
            return 10;
        }

        [HttpGet]
        public int GetOpenTasks()
        {
            return 20;
        }

        [HttpGet]
        public int GetPendingInvoices()
        {
            return 30;
        }

        [HttpGet]
        public int GetActiveResources()
        {
            return 40;
        }

        [HttpPost]
        public void AsyncMonerisResponse()
        {

        }
    }
}
