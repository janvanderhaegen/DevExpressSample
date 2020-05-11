# Devexpress: End User Report Designer sample
This sample is a very simple and hardcoded version of what we have in production.
Made it only to troubleshoot issues and be able to share those with the DevExpress team.

It features two reports in a master-detail setting.
Both reports use an ObjectDataSource that connects to a method on a generic "data retriever".  
In reality, we have a system where users can add 'data sources' (we call those queries) at runtime.
This means that the assembly that is hosting the DevExpress report designer, report data store, etc, does not have a link to the models at compile time.

## Basic Setup
### Controllers & Views
Links to print/edit the master and the detail report.

### Models - Data.Models & Data.Repository
Models: Customer & CustomerDetail DTOs.
Repo: hardcoded data repo.
In reality, these classes are in assemblies that are not referenced at compile time.

### Models - Data.Retriever
This class is responsible for retrieving the data (executing the query). 
It is generic, the real type of T at runtime will be either Customer or CustomerDetail.
  
### Models - Report.Repo
Hardcoded repo that creates the sample layout for the reports.

### Models - Report.Store
We use a custom DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension to store the reports (in-memory for this example, database in reality).
