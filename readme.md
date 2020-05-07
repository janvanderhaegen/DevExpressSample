# Devexpress: End User Report Designer sample
This sample is a very simple and hardcoded version of what we have in production.
Made it only to troubleshoot issues and be able to share those with the DevExpress team.

It features two reports in a master-detail setting.
Both reports use a custom DataSource (ObjectDataSource) that connect to a data-repository.  

## Basic Setup
### Controllers & Views
Links to print/edit the master and the detail report.

### Models - Data
A (hardcoded) data repository. 
  -  Customers (all). Consumed by the data source of the master report.
  -  CustomerDetails (by customer id). Consumed by the data source of the detail report.

### Models - ReportDataSource
Each report's data source is set to an instance of this custom DevExpress.DataAccess.ObjectBinding.ObjectDataSource.

### Models - ReportDataSourceSerializer
Custom DevExpress.XtraReports.Native.IDataSerializer that serializes the ReportDataSource using NewtonSoft.json.

### Models - ReportRepository
Hardcoded repo that creates the sample layout for the reports.

### Models - ReportsStore
We use a custom DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension to store the reports (in-memory for this example).