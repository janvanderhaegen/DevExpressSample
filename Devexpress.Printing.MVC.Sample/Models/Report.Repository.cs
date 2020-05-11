using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Devexpress.Printing.MVC.Sample.Models
{
    public static class ReportRepository
    {
        public static XtraReport CreateSampleMasterReport()
        {
            var report = new XtraReport();
            using (StreamWriter sw = new StreamWriter(new MemoryStream()))
            {
                sw.Write(ReportRepository.SampleMasterReportLayout);
                sw.Flush();
                report.LoadLayoutFromXml(sw.BaseStream);
            }
            var objectDataSource = new ObjectDataSource();
            objectDataSource.BeginInit();
            objectDataSource.Name = "Query: Customers";
            objectDataSource.DataSource = typeof(GenericDataRetriever<Customer>);
            objectDataSource.Constructor = new ObjectConstructorInfo();
            objectDataSource.DataMember = nameof(GenericDataRetriever<Customer>.Execute);
            objectDataSource.Parameters.Add(new Parameter
            {
                Name = "queryName",
                Type = typeof(string),
                Value = typeof(Customer).Name
            });
            objectDataSource.EndInit();
            report.DataSource = objectDataSource;
            return report;
        }

        private static string sampleMasterReportLayout;
        public static string SampleMasterReportLayout
        {
            get
            {
                if (sampleMasterReportLayout == null)
                {
                    var report = new XtraReport();
                    var topMarginBand = new TopMarginBand();
                    topMarginBand.Height = 10;
                    report.Bands.Add(topMarginBand);


                    var reportHeaderBand = new ReportHeaderBand();
                    reportHeaderBand.HeightF = 45;
                    report.Bands.Add(reportHeaderBand);
                    var reportTitleLabel = new XRLabel();
                    reportTitleLabel.Text = "Sample Customers Report";
                    reportTitleLabel.Location = new Point(10, 10);
                    reportTitleLabel.HeightF = 35;
                    reportTitleLabel.WidthF = 610;
                    reportTitleLabel.Font = new Font(reportTitleLabel.Font.FontFamily, 20, FontStyle.Bold);
                    reportTitleLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
                    reportHeaderBand.Controls.Add(reportTitleLabel);


                    var pageHeaderBand = new PageHeaderBand();
                    pageHeaderBand.HeightF = 45;
                    pageHeaderBand.Controls.Add(new XRLabel()
                    {
                        Location = new Point(0, 0),
                        Text = "Name"
                    });
                    pageHeaderBand.Controls.Add(new XRLabel()
                    {
                        Location = new Point(100, 0),
                        Text = "Details"
                    });
                    report.Bands.Add(pageHeaderBand);


                    var detailBand = new DetailBand() { Height = 25 };
                    var valueLabel = new XRLabel();
                    valueLabel.ExpressionBindings.Add(new ExpressionBinding
                    {
                        EventName = "BeforePrint",
                        Expression = $"[Name]",
                        PropertyName = "Text"
                    });
                    valueLabel.Location = new Point(0, 0);
                    detailBand.Controls.Add(valueLabel);

                    var subReport = new XRSubreport();
                    subReport.ReportSourceUrl = "CustomerDetails";
                    subReport.ParameterBindings.Add(new ParameterBinding(
                        parameterName: "CustomerId",
                        dataSource: null,
                        dataMember: "Id"
                        ));
                    subReport.Location = new Point(100, 0);
                    subReport.CanShrink = true;
                    detailBand.Controls.Add(subReport);

                    report.Bands.Add(detailBand);


                    report.ExportOptions.PrintPreview.DefaultFileName =
                        report.Name =
                        report.DisplayName = "Customers";


                    var str = new MemoryStream();
                    report.SaveLayoutToXml(str);
                    str.Position = 0;
                    var sr = new StreamReader(str);
                    var myStr = sr.ReadToEnd();
                    sampleMasterReportLayout = myStr;
                }
                return sampleMasterReportLayout;
            }
            set { sampleMasterReportLayout = value; }
        }



        public static XtraReport CreateSampleDetailReport()
        {
            var report = new XtraReport();
            using (StreamWriter sw = new StreamWriter(new MemoryStream()))
            {
                sw.Write(ReportRepository.SampleDetailReportLayout);
                sw.Flush();
                report.LoadLayoutFromXml(sw.BaseStream);
            }
            var objectDataSource = new ObjectDataSource();
            objectDataSource.BeginInit();
            objectDataSource.Name = "Customer details";
            objectDataSource.DataSource = typeof(GenericDataRetriever<CustomerDetail>);
            objectDataSource.Constructor = new ObjectConstructorInfo();
            objectDataSource.DataMember = nameof(GenericDataRetriever<CustomerDetail>.Execute);
            objectDataSource.Parameters.Add(new Parameter
            {
                Name = "queryName",
                Type = typeof(string),
                Value = typeof(CustomerDetail).Name
            });
            objectDataSource.Parameters.Add(new Parameter
            {
                Name = "arg1",
                Type = typeof(DevExpress.DataAccess.Expression),
                Value = new DevExpress.DataAccess.Expression("?CustomerId", typeof(int))
            });
            objectDataSource.EndInit();
            report.DataSource = objectDataSource;
            return report;
        }

        private static string sampleDetailReportLayout;
        public static string SampleDetailReportLayout
        {
            get
            {
                if (sampleDetailReportLayout == null)
                {
                    var report = new XtraReport();
                    report.Parameters.Add(new DevExpress.XtraReports.Parameters.Parameter
                    {
                        AllowNull = false,
                        Description = "Customer Id",
                        Name = "CustomerId",
                        Type = typeof(int),
                        MultiValue = false,
                        Visible = true
                    });

                    var pageHeaderBand = new PageHeaderBand();
                    pageHeaderBand.HeightF = 45;
                    pageHeaderBand.Controls.Add(new XRLabel
                    {
                        Text = "Parameter (C-ID)",
                        Location = new Point(0, 0)
                    });
                    pageHeaderBand.Controls.Add(new XRLabel
                    {
                        Text = "Customer Id",
                        Location = new Point(100, 0)
                    });
                    pageHeaderBand.Controls.Add(new XRLabel
                    {
                        Text = "Detail Id",
                        Location = new Point(200, 0)
                    });
                    pageHeaderBand.Controls.Add(new XRLabel
                    {
                        Text = "Phone",
                        Location = new Point(300, 0)
                    });
                    pageHeaderBand.Controls.Add(new XRLabel
                    {
                        Text = "Description",
                        Location = new Point(400, 0)
                    });
                    report.Bands.Add(pageHeaderBand);


                    var detailBand = new DetailBand() { Height = 25 };
                    var parameterCustomerIdLabel = new XRLabel() { Location = new Point(0, 0) };
                    parameterCustomerIdLabel.ExpressionBindings.Add(new ExpressionBinding
                    {
                        EventName = "BeforePrint",
                        Expression = $"?CustomerId",
                        PropertyName = "Text"
                    });
                    detailBand.Controls.Add(parameterCustomerIdLabel);

                    var customerIdLabel = new XRLabel() { Location = new Point(100, 0) };
                    customerIdLabel.ExpressionBindings.Add(new ExpressionBinding
                    {
                        EventName = "BeforePrint",
                        Expression = $"[CustomerId]",
                        PropertyName = "Text"
                    });
                    detailBand.Controls.Add(customerIdLabel);

                    var detailIdLabel = new XRLabel() { Location = new Point(200, 0) };
                    detailIdLabel.ExpressionBindings.Add(new ExpressionBinding
                    {
                        EventName = "BeforePrint",
                        Expression = $"[DetailId]",
                        PropertyName = "Text"
                    });
                    detailBand.Controls.Add(detailIdLabel);


                    var phoneNumberLabel = new XRLabel() { Location = new Point(300, 0) };
                    phoneNumberLabel.ExpressionBindings.Add(new ExpressionBinding
                    {
                        EventName = "BeforePrint",
                        Expression = $"[PhoneNumber]",
                        PropertyName = "Text"
                    });
                    detailBand.Controls.Add(phoneNumberLabel);

                    var descriptionLabel = new XRLabel() { Location = new Point(400, 0) };
                    descriptionLabel.ExpressionBindings.Add(new ExpressionBinding
                    {
                        EventName = "BeforePrint",
                        Expression = $"[Description]",
                        PropertyName = "Text"
                    });
                    detailBand.Controls.Add(descriptionLabel);
                    report.Bands.Add(detailBand);


                    report.ExportOptions.PrintPreview.DefaultFileName =
                        report.Name =
                        report.DisplayName = "CustomerDetails";

                    var str = new MemoryStream();
                    report.SaveLayoutToXml(str);
                    str.Position = 0;
                    var sr = new StreamReader(str);
                    var myStr = sr.ReadToEnd();
                    sampleDetailReportLayout = myStr;
                }
                return sampleDetailReportLayout;
            }
            set { sampleDetailReportLayout = value; }
        }
    }
}