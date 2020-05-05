using DevExpress.Data;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Devexpress.Printing.MVC.Sample.Models
{
    public class ReportDataSourceDTO
    {
        public string ReturnType { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public Type ResolvedReturnType { get; set; }
        public IEnumerable<object> Data { get; set; }
        public string QueryName { get; set; }
        public Guid QueryConfigurationId { get; set; }
        public string UserName { get; set; }
        public string TenantId { get; set; }
    }

    public class ReportDataSource : ObjectDataSource, ITypedList
    {
        public Type ReturnType { get; private set; }
        public string QueryName { get; private set; }
        public Guid QueryConfigurationId { get; private set; }
        public string UserName { get; set; }
        public string TenantId { get; set; }

        public IEnumerable<object> Data
        {
            get
            {

                return (IEnumerable<object>)this.DataSource;
            }
        }

        public ReportDataSource()
        {

        }
        public ReportDataSource(Type returnType, Guid queryConfigurationId, string queryName, string userName, string tenantId, IEnumerable<object> data = null)
        {
            this.ReturnType = returnType;
            this.QueryName = queryName;
            this.UserName = userName;
            this.TenantId = tenantId;
            this.QueryConfigurationId = queryConfigurationId;
            this.Name = queryConfigurationId.ToString();
            if (data != null)
                this.DataSource = new List<object>(data);
            else
                this.DataSource = new List<object>();
        }
        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            var desc = TypeDescriptor.GetProperties(this.ReturnType);
            return desc;
        }

        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            return QueryName;
        }
        public override void Fill(IEnumerable<IParameter> sourceParameters)
        {
            try
            {
                var executionResult = Repository.Customers;
                ((List<object>)this.DataSource).Clear();
                ((List<object>)this.DataSource).AddRange(executionResult);
            }
            catch (Exception x)
            {
                var message = "Could not run " + (string.IsNullOrEmpty(this.QueryName) ? "one or more queries" : $"query '{this.QueryName}'") + ": " + x.Message;
                throw new DocumentCreationException($"Could not run query '{this.QueryConfigurationId}' to populate report: {x.Message}");
            }
        }


        protected override string GetDataMember()
        {
            return base.GetDataMember();
        }
        public override void LoadFromXml(XElement element)
        {
            var cdata = element.FirstNode as XCData;
            var value = cdata.Value;
            var serializer = new MyDataSerializer();
            var dto = (ReportDataSourceDTO)serializer.Deserialize(value, typeof(ReportDataSourceDTO).FullName, null);
            this.ReturnType = dto.ResolvedReturnType;
            this.QueryName = dto.QueryName;
            this.DataSource = dto.Data;
            this.QueryConfigurationId = dto.QueryConfigurationId;
            this.UserName = dto.UserName;
            this.TenantId = dto.TenantId;
            this.Name = dto.QueryName;
        }
        public override XElement SaveToXml()
        {
            if (string.IsNullOrEmpty(this.UserName) && System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();

            var serializer = new MyDataSerializer();
            var serialized = serializer.Serialize(this, null);
            var el = new XElement("ReportDataSource");
            el.Add(new XCData(serialized));
            return el;
        }
    }
}