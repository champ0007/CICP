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
        public int GetActiveProjects()
        {
            return 10;
        }

        public int GetOpenTasks()
        {
            return 20;
        }

        public int GetPendingInvoices()
        {
            return 30;
        }

        public int GetActiveResources()
        {
            return 40;
        }
    }
}
