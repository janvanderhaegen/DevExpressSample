using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevExpress.DataAccess.Json;
using DevExpress.XtraReports.Native;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using Devexpress.Printing.MVC.Sample.Models;

namespace Devexpress.Printing.MVC.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ReportStorageWebExtension.RegisterExtensionGlobal(new ReportsStore());
        }
    }
}
