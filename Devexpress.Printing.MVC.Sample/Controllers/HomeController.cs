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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MasterPrint()
        {
            var report = ReportRepository.CreateSampleMasterReport();
            return View(report);
        }
        public ActionResult MasterEdit()
        {
            var report = ReportRepository.CreateSampleMasterReport();
            return View(report);
        }
        public ActionResult DetailPrint()
        {
            var report = ReportRepository.CreateSampleDetailReport();
            return View(report);
        }
        public ActionResult DetailEdit()
        {
            var report = ReportRepository.CreateSampleDetailReport();
            return View(report);
        }
    }
}