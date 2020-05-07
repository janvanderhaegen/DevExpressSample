using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Devexpress.Printing.MVC.Sample.Models
{
    public class ReportsStore : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        public override byte[] GetData(string url)
        {
            try
            {
                var report = url == "Customers" ? ReportRepository.CreateSampleMasterReport() : ReportRepository.CreateSampleDetailReport();
                using (var ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    return ms.GetBuffer();
                }
            }
            catch (Exception x)
            {
                throw propagateExceptionToDevExpressClient("read report: ", x);
            }
        }
        public override bool CanSetData(string url)
        {
            return true;
        }
        public override bool IsValidUrl(string url)
        {
            return GetUrls().ContainsKey(url);
        }
        public override void SetData(XtraReport report, string url)
        {
            try
            {
                var layoutStream = new MemoryStream();
                report.SaveLayoutToXml(layoutStream);
                layoutStream.Position = 0;
                string layout = null;
                using (var sr = new StreamReader(layoutStream))
                {
                    layout = sr.ReadToEnd();
                }
                if (url == "Customers")
                {
                    ReportRepository.SampleMasterReportLayout = layout;
                }
                else
                {
                    ReportRepository.SampleDetailReportLayout = layout;
                }
            }
            catch (Exception x)
            {
                throw propagateExceptionToDevExpressClient("save report", x);
            }
        }
        public override Dictionary<string, string> GetUrls()
        {
            var d = new Dictionary<string, string>(1);
            d["Customers"] = "Customers";
            d["CustomerDetails"] = "CustomerDetails";
            return d;
        }
        public override void SetData(XtraReport report, Stream stream)
        {
            try
            {
                base.SetData(report, stream);
            }
            catch (Exception x)
            {
                throw propagateExceptionToDevExpressClient("save report", x);
            }
        }
        //Save as is executed instead of save...
        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            try
            {
                var datasource = report.DataSource as ReportDataSource;
                if (datasource != null)
                {
                    report.DataSource = null;
                }
                var layoutStream = new MemoryStream();
                report.SaveLayoutToXml(layoutStream);
                layoutStream.Position = 0;
                string layout = null;
                using (var sr = new StreamReader(layoutStream))
                {
                    layout = sr.ReadToEnd();
                }
                if (defaultUrl == "Customers")
                {
                    ReportRepository.SampleMasterReportLayout = layout;
                }
                else
                {
                    ReportRepository.SampleDetailReportLayout = layout;
                }
                return defaultUrl;
            }
            catch (Exception x)
            {
                throw propagateExceptionToDevExpressClient("save report", x);

            }
        }
        internal static Exception propagateExceptionToDevExpressClient(string whatYouTried, Exception x)
        {
            var message = x.Message;
            message = $"Failed to {whatYouTried}: {x.Message}";
            return new Exception(message, x.InnerException);
        }
    }
}