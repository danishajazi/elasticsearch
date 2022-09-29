using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.ConsoleApp.Entities
{
    internal class Customer
    {
        public Customer()
        {
            Orders = new List<Order>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Birthday { get; set; }

        public string Address { get; set; }

        public List<Order> Orders { get; set; }
    }
}
