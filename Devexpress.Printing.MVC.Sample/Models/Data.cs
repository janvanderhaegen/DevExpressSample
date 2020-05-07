using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Devexpress.Printing.MVC.Sample.Models
{
    public class DataRepository
    {
        private static IEnumerable<Customer> customers = Enumerable.Range(100, 400)
                        .Select(i => new Customer { Name = $"Customer {i}", Id = i })
                        .ToArray();
        private static IEnumerable<CustomerDetail> customerDetails = Enumerable.Range(0, 1337)
                        .Select(i =>
                        {
                            var index = i;
                            var customerId = 100 + (index % 400);
                            return new CustomerDetail
                            {
                                CustomerId = customerId,
                                DetailId = index,
                                PhoneNumber = string.Format("({0:###}) {1:###-####}", customerId, (1010101 + i)),
                                Description = $"Mobile phone of customer ({customerId})"
                            };
                        })
                        .ToArray();
        public static IEnumerable<Customer> Customers { get { return customers; } }
        public static IEnumerable<CustomerDetail> CustomerDetails(int customerId)
        {
            return customerDetails.Where(c => c.CustomerId == customerId);
        }
    }
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