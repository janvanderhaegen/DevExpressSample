using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Devexpress.Printing.MVC.Sample.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class CustomerDetail
    {
        public int CustomerId { get; set; }
        public int DetailId { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
    }
}