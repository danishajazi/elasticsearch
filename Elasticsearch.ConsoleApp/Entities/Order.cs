using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elasticsearch.ConsoleApp.Entities
{
    internal class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string OrderDetails { get; set; }
    }
}
