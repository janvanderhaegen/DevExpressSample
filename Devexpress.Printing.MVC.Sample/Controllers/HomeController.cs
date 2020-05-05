using Devexpress.Printing.MVC.Sample.Models;
using DevExpress.DataAccess.Json;
using DevExpress.XtraReports.Native;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        static HomeController()
        {
            //This report contains bindings, would you like to convert them to expressions => disable prompt and upgrade automatically
            DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.ConvertBindingsToExpressions = PromptBoolean.True;

            ReportStorageWebExtension.RegisterExtensionGlobal(new ReportsDataStore());
            SerializationService.RegisterSerializer(MyDataSerializer.Name, new MyDataSerializer());
        }
        public ActionResult Index()
        {
            var report = Repository.CreateSampleReport();
            return View(report);
        }
    }
}