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
        //GET api/Dashboard/getactiveprojects
        public int GetActiveProjects()
        {
            return 10;
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

        [HttpPost]
        public void AsyncMonerisResponse()
        {

        }
    }
}
