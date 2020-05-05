using DevExpress.DataAccess.Json;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Devexpress.Printing.MVC.Sample.Models
{
    public static class Repository
    {
        private static Guid queryId = Guid.NewGuid();
        public static XtraReport CreateSampleReport()
        {
            var report = new XtraReport();
            using (StreamWriter sw = new StreamWriter(new MemoryStream()))
            {
                sw.Write(Repository.SampleReportLayout);
                sw.Flush();
                report.LoadLayoutFromXml(sw.BaseStream);
            }
            report.Extensions[DevExpress.XtraReports.Native.SerializationService.Guid] = MyDataSerializer.Name;

            //var jsonDataSource = new JsonDataSource();
            //jsonDataSource.JsonSource = new UriJsonSource(new Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json"));
            //jsonDataSource.Fill();
            //report.DataSource = jsonDataSource;

            report.DataSource = new ReportDataSource(typeof(Customer), queryId, "Query Name", "User Name", "Tenant Id", Customers);

            //migrateDataBindingsToExpressions(report);

            return report;
        }

        private static void migrateDataBindingsToExpressions(XtraReport report)
        {
            var allControls = report.AllControls<XRControl>().ToArray();
            foreach (var control in allControls)
            {
                if (control.DataBindings != null && control.DataBindings.Any())
                {
                    foreach (var binding in control.DataBindings.OfType<XRBinding>().ToArray())
                    {
                        control.ExpressionBindings.Add(new ExpressionBinding
                        {
                            EventName = "BeforePrint",
                            Expression = $"[{binding.DataMember}]",
                            PropertyName = binding.PropertyName
                        });
                    }
                    control.DataBindings.Clear();
                }
            }
        }

        private static string sampleReportLayout;
        public static string SampleReportLayout
        {
            get
            {
                if (sampleReportLayout == null)
                {
                    var report = new XtraReport();
                    var topMarginBand = report.Bands.GetBandByType(typeof(TopMarginBand));
                    if (topMarginBand == null)
                    {
                        topMarginBand = new TopMarginBand();
                        topMarginBand.Height = 10;
                        report.Bands.Add(topMarginBand);
                    }

                    var reportHeaderBand = report.Bands.GetBandByType(typeof(ReportHeaderBand));
                    if (reportHeaderBand == null)
                    {
                        reportHeaderBand = new ReportHeaderBand();
                        reportHeaderBand.HeightF = 45;
                        report.Bands.Add(reportHeaderBand);
                    }
                    XRLabel reportTitleLabel = new XRLabel();
                    reportTitleLabel.Text = "Sample Customers Report";
                    reportTitleLabel.Location = new Point(10, 10);
                    reportTitleLabel.HeightF = 35;
                    reportTitleLabel.WidthF = 610;
                    reportTitleLabel.Font = new Font(reportTitleLabel.Font.FontFamily, 20, FontStyle.Bold);
                    reportTitleLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    reportHeaderBand.Controls.Add(reportTitleLabel);


                    var pageHeaderBand = report.Bands.GetBandByType(typeof(PageHeaderBand));
                    if (pageHeaderBand == null)
                    {
                        pageHeaderBand = new PageHeaderBand();
                        pageHeaderBand.HeightF = 45;
                        XRLabel pageHeaderLabel = new XRLabel();
                        pageHeaderLabel.Text = "Name";
                        pageHeaderLabel.Location = new Point(0, 0);
                        pageHeaderBand.Controls.Add(pageHeaderLabel);
                        report.Bands.Add(pageHeaderBand);
                    }

                    Band detailBand = new DetailBand();
                    detailBand.Height = 30;
                    XRLabel valueLabel = new XRLabel();
                    valueLabel.DataBindings.Add(new XRBinding("Text", null, "Name"));
                    valueLabel.Location = new Point(0, 0);
                    detailBand.Controls.Add(valueLabel);
                    report.Bands.Add(detailBand);


                    report.ExportOptions.PrintPreview.DefaultFileName = "Customers Sample Report";
                    report.Name = "Customers Sample Report";
                    report.DisplayName = "Customers Sample Report";


                    var str = new MemoryStream();
                    report.SaveLayoutToXml(str);
                    str.Position = 0;
                    var sr = new StreamReader(str);
                    var myStr = sr.ReadToEnd();
                    sampleReportLayout = myStr;
                }
                return sampleReportLayout;
            }
            set { sampleReportLayout = value; }
        }

        private static IEnumerable<Customer> customers;
        public static IEnumerable<Customer> Customers
        {
            get
            {
                if (customers == null)
                {
                    customers = Enumerable.Range(0, 500)
                        .Select(i => new Customer { Name = $"Customer {1 + i}" })
                        .ToArray();
                }
                return customers;
            }
        }

    }
    public class Customer
    {
        public string Name { get; set; }
    }
}