# Devexpress: End User Report Designer sample
This sample is a very simple and hardcoded version of what we have in production.
Made it only to troubleshoot issues and be able to share those with the DevExpress team.

## Basic Setup
### HomeController
Basic configuration (static extensions: serializer, etc)
Returns the end user report designer on a model made from a hardcoded layout (string).

### Models - Repository
This one creates the report layout by working with DevExpress classes and then calling SaveLayoutToXML on the report.
Also provides the test data (500 "customers").

### Models - ReportDataStore
We use a custom DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.

### Models - ReportDataSource
Each report's data source is set to an instance of this custom DevExpress.DataAccess.ObjectBinding.ObjectDataSource.

### Models - MyDataSerializer
Custom DevExpress.XtraReports.Native.IDataSerializer that serializes the ReportDataSource using NewtonSoft.json.