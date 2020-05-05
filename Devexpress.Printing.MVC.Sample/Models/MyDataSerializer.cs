using DevExpress.XtraReports.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Devexpress.Printing.MVC.Sample.Models
{
    public class MyDataSerializer : IDataSerializer
    {
        public const string Name = "MyDataSerializer";

        public bool CanSerialize(object data, object extensionProvider)
        {
            return data is ReportDataSource;
        }

        public string Serialize(object data, object extensionProvider)
        {
            var source = data as ReportDataSource;
            var dto = new ReportDataSourceDTO();
            dto.QueryName = source.QueryName;
            dto.ReturnType = source.ReturnType == null ? null : source.ReturnType.AssemblyQualifiedName;
            dto.Data = source.Data.Take(150).ToArray();
            dto.QueryConfigurationId = source.QueryConfigurationId;
            dto.UserName = source.UserName;
            dto.TenantId = source.TenantId;
            var convert = Newtonsoft.Json.JsonConvert.SerializeObject(dto);
            return convert;
        }

        public bool CanDeserialize(string value, string typeName, object extensionProvider)
        {
            return typeName == typeof(ReportDataSource).FullName;
        }

        public object Deserialize(string value, string typeName, object extensionProvider)
        {
            var des = Newtonsoft.Json.JsonConvert.DeserializeObject<ReportDataSourceDTO>(value);

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
            if (typeName.Equals(typeof(ReportDataSourceDTO).FullName))
            {
                return des;
            }
            var d = new ReportDataSource(des.ResolvedReturnType, des.QueryConfigurationId, des.QueryName, des.UserName, des.TenantId, des.Data);
            return d;
        }
    }
}