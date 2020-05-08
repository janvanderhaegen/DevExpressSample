using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.IO;
using DevExpress.DataAccess.ObjectBinding;

namespace Devexpress.Printing.MVC.Sample.Models
{
    [DisplayName("Customers")]
    [HighlightedClass]
    public class AllCustomersDataSource : List<Customer>
    {
        [HighlightedMember]
        public AllCustomersDataSource() : base(DataRepository.Customers)
        {

        }
    }

    [DisplayName("Repo")]
    [HighlightedClass]
    public class CustomerDetailsDataRepo
    {
        [HighlightedMember]
        public IEnumerable<CustomerDetail> Details(int customerId)
        {
            return DataRepository.CustomerDetails(customerId);
        }
    }
}