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
    public class Hello
    {
        public const string World = "Need to place a class here or the file is added to the csproj with subtype 'Designer', which is annoying if you don't have the DevExpress VS tooling installed";
    }
    public class ReportDataSource : ObjectDataSource, ITypedList
    {
        public Type ReturnType { get; private set; }

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
        public ReportDataSource(Type returnType, string name, IEnumerable<object> data = null)
        {
            this.ReturnType = returnType;
            this.Name = name;
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
            return this.Name;
        }
        public override void Fill(IEnumerable<IParameter> sourceParameters)
        {
            base.Fill(sourceParameters);
            try
            {
                IEnumerable<object> executionResult;
                if (this.ReturnType.Equals(typeof(CustomerDetail)))
                {
                    var customerIdParam = sourceParameters.Where(c => c.Name == "CustomerId").SingleOrDefault();
                    if (customerIdParam != null && customerIdParam.Value != null && !0.Equals(customerIdParam.Value))
                    {
                        executionResult = DataRepository.CustomerDetails((int)customerIdParam.Value);
                    }
                    else
                    {
                        executionResult = Enumerable.Empty<object>();
                    }
                }
                else if (this.ReturnType.Equals(typeof(Customer)))
                {
                    executionResult = DataRepository.Customers;
                }
                else
                    throw new NotImplementedException($"Unknown type: {this.ReturnType.Namespace}.{this.ReturnType.Namespace}");
                ((List<object>)this.DataSource).Clear();
                ((List<object>)this.DataSource).AddRange(executionResult);
            }
            catch (Exception x)
            {
                throw new DocumentCreationException($"Could not run query '{this.Name}' to populate report: {x.Message}");
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
            var serializer = new ReportDataSourceSerializer();
            var dto = (SerializableReportDataSource)serializer.Deserialize(value, typeof(SerializableReportDataSource).FullName, null);
            this.ReturnType = dto.ResolvedReturnType;
            this.DataSource = dto.Data;
            this.Name = dto.Name;
        }
        public override XElement SaveToXml()
        {
            var serializer = new ReportDataSourceSerializer();
            var serialized = serializer.Serialize(this, null);
            var el = new XElement("ReportDataSource");
            el.Add(new XCData(serialized));
            return el;
        }
    }
}