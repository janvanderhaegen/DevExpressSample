using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.DataAccess.Sql;
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
                var sqlDataSource = new SqlDataSource();
                sqlDataSource.ConnectionName = "sql";
                sqlDataSource.Name = "sqlDataSource1";
                var table = new DevExpress.DataAccess.Sql.Table()
                {
                    Name = "Customers"
                };
                var query = new DevExpress.DataAccess.Sql.SelectQuery()
                {
                    Name = "Customers"
                };
                query.Tables.Add(table);
                query.Columns.Add(new Column()
                {
                    Expression = new ColumnExpression
                    {
                        Table = table,
                        ColumnName = "CustomerId"
                    }
                });
                query.Columns.Add(new Column()
                {
                    Expression = new ColumnExpression
                    {
                        Table = table,
                        ColumnName = "Name"
                    }
                });
                sqlDataSource.Queries.Add(query);
                sqlDataSource.ResultSchemaSerializable = "PERhdGFTZXQgTmFtZT0ic3FsRGF0YVNvdXJjZTEiPjxWaWV3IE5hbWU9IkN1c3RvbWVycyI+PEZpZWxkI" +
    "E5hbWU9IkN1c3RvbWVySWQiIFR5cGU9IkludDMyIiAvPjxGaWVsZCBOYW1lPSJOYW1lIiBUeXBlPSJTd" +
    "HJpbmciIC8+PC9WaWV3PjwvRGF0YVNldD4=";
                report.DataSource = sqlDataSource;
                report.DataMember = "Customers";
            }
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
                        Expression = $"[Customers.Name]",
                        PropertyName = "Text"
                    });
                    valueLabel.Location = new Point(0, 0);
                    detailBand.Controls.Add(valueLabel);

                    var subReport = new XRSubreport();
                    subReport.ReportSourceUrl = "CustomerDetails";
                    subReport.ParameterBindings.Add(new ParameterBinding(
                        parameterName: "CustomerId",
                        dataSource: null,
                        dataMember: "Customers.CustomerId"
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

                var sqlDataSource = new SqlDataSource();
                sqlDataSource.ConnectionName = "sql";
                sqlDataSource.Name = "SP_Customer_Details";
                var storedProc = new DevExpress.DataAccess.Sql.StoredProcQuery()
                {
                    Name = "SP_Customer_Details",
                    StoredProcName = "SP_Customer_Details"
                };
                storedProc.Parameters.Add(new DevExpress.DataAccess.Sql.QueryParameter
                {
                    Name = "@CustomerId",
                    Type = typeof(DevExpress.DataAccess.Expression),
                    Value = new DevExpress.DataAccess.Expression("?CustomerId", typeof(int))

                });
                sqlDataSource.Queries.Add(storedProc);
                sqlDataSource.ResultSchemaSerializable = "PERhdGFTZXQgTmFtZT0ic3FsRGF0YVNvdXJjZTEiPjxWaWV3IE5hbWU9IlNQX0N1c3RvbWVyX0RldGFpbHMiPjxGaWVsZCBOYW1lPSJDdXN0b21lcklkIiBUeXBlPSJJbnQzMiIgLz48RmllbGQgTmFtZT0iRGV0YWlsSWQiIFR5cGU9IkludDMyIiAvPjxGaWVsZCBOYW1lPSJQaG9uZU51bWJlciIgVHlwZT0iU3RyaW5nIiAvPjxGaWVsZCBOYW1lPSJEZXNjcmlwdGlvbiIgVHlwZT0iU3RyaW5nIiAvPjwvVmlldz48L0RhdGFTZXQ+";
                report.DataSource = sqlDataSource;
                report.DataMember = "SP_Customer_Details";
            }
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
                        Expression = $"[SP_Customer_Details.CustomerId]",
                        PropertyName = "Text"
                    });
                    detailBand.Controls.Add(customerIdLabel);

                    var detailIdLabel = new XRLabel() { Location = new Point(200, 0) };
                    detailIdLabel.ExpressionBindings.Add(new ExpressionBinding
                    {
                        EventName = "BeforePrint",
                        Expression = $"[SP_Customer_Details.DetailId]",
                        PropertyName = "Text"
                    });
                    detailBand.Controls.Add(detailIdLabel);


                    var phoneNumberLabel = new XRLabel() { Location = new Point(300, 0) };
                    phoneNumberLabel.ExpressionBindings.Add(new ExpressionBinding
                    {
                        EventName = "BeforePrint",
                        Expression = $"[SP_Customer_Details.PhoneNumber]",
                        PropertyName = "Text"
                    });
                    detailBand.Controls.Add(phoneNumberLabel);

                    var descriptionLabel = new XRLabel() { Location = new Point(400, 0) };
                    descriptionLabel.ExpressionBindings.Add(new ExpressionBinding
                    {
                        EventName = "BeforePrint",
                        Expression = $"[SP_Customer_Details.Description]",
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