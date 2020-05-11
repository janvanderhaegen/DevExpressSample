using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Devexpress.Printing.MVC.Sample.Models
{
    public class GenericDataRetriever<T>
    {
        public IEnumerable<T> Execute(string queryName)
        {
            return new DataRepository().Execute(queryName).OfType<T>();
        }
        public IEnumerable<T> Execute(string queryName, object arg1)
        {
            return new DataRepository().Execute(queryName, arg1).OfType<T>();
        }
        public IEnumerable<T> Execute(string queryName, object arg1, object arg2)
        {
            return new DataRepository().Execute(queryName, arg1, arg2).OfType<T>();
        }
    }
}