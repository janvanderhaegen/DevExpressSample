using DevExpress.XtraReports.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Devexpress.Printing.MVC.Sample.Models
{
    public class ReportDataSourceSerializer : IDataSerializer
    {
        public const string Name = "ReportDataSourceSerializer";

        public bool CanSerialize(object data, object extensionProvider)
        {
            return data is ReportDataSource;
        }

        public string Serialize(object data, object extensionProvider)
        {
            var source = data as ReportDataSource;
            var dto = new SerializableReportDataSource();
            dto.Name = source.Name;
            dto.ReturnType = source.ReturnType == null ? null : source.ReturnType.AssemblyQualifiedName;
            //Limit data while saving
            dto.Data = source.Data.Take(150).ToArray();
            var convert = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
            return convert;
        }

        public bool CanDeserialize(string value, string typeName, object extensionProvider)
        {
            return typeName == typeof(ReportDataSource).FullName;
        }

        public object Deserialize(string value, string typeName, object extensionProvider)
        {
            var des = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableReportDataSource>(value);

            var returnType = string.IsNullOrEmpty(des.ReturnType) ? null : Type.GetType(des.ReturnType);
            des.ResolvedReturnType = returnType;
            var list = new List<object>(des.Data.Count());
            if (returnType != null)
            {
                foreach (var item in des.Data)
                {
                    var str = item.ToString();
                    var newItem = Newtonsoft.Json.JsonConvert.DeserializeObject(str, returnType);
                    list.Add(newItem);
                }
            }
            des.Data = list;
            if (typeName.Equals(typeof(SerializableReportDataSource).FullName))
            {
                return des;
            }
            var d = new ReportDataSource(des.ResolvedReturnType, des.Name, des.Data);
            return d;
        }
    }
    public class SerializableReportDataSource
    {
        public string ReturnType { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public Type ResolvedReturnType { get; set; }
        public IEnumerable<object> Data { get; set; }
        public string Name { get; set; }
    }
}