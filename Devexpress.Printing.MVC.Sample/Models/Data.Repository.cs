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
        public IEnumerable<object> Execute(string queryName)
        {
            if (queryName.Equals(typeof(Customer).Name))
                return customers;
            throw new NotImplementedException($"Unidentified query: '{queryName}'");
        }
        public IEnumerable<object> Execute(string queryName, object arg1)
        {
            if (queryName.Equals(typeof(CustomerDetail).Name))
            {
                var customerId = (int)arg1;
                var customersDetails = customerDetails.Where(c => c.CustomerId == customerId).ToArray();
                return customersDetails;
            }
            throw new NotImplementedException($"Unidentified query: '{queryName}'");
        }
        public IEnumerable<object> Execute(string queryName, object arg1, object arg2)
        {
            throw new NotImplementedException($"Unidentified query: '{queryName}'");
        }
        //ETC 
    }
}