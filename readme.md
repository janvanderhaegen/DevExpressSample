[DevExpress Issue: t887677](https://supportcenter.devexpress.com/ticket/details/t887677/reporting-master-detail-detail-report-objectdatasource-fill-is-no-longer-called-for)

# Devexpress: End User Report Designer sample
This sample is a very simple and hardcoded version of what we have in production.
Made it only to troubleshoot issues and be able to share those with the DevExpress team.

It features two reports in a master-detail setting.
Both reports use a standard ObjectDataSource connect to a List of Objects (Customer or CustomerDetail).  

## Basic Setup
### Controllers & Views
Links to print/edit the master and the detail report.

### Models - Data
A (hardcoded) data repository. 
  -  Customers (all). Consumed by the data source of the master report.
  -  CustomerDetails (by customer id). Consumed by the data source of the detail report.

### Models - ReportDataSource
See the Fishes.txt example ;-)

### Models - ReportRepository
Hardcoded repo that creates the sample layout for the reports.

### Models - ReportsStore
We use a custom DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension to store the reports (in-memory for this example).
