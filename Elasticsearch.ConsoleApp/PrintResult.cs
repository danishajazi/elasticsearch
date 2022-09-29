using Elasticsearch.ConsoleApp.Entities;
using Nest;

namespace Elasticsearch.ConsoleApp
{
    internal class PrintResult
    {
        public static void DisplaySearchResult(ISearchResponse<Customer> results)
        {
            if (results.Documents.Count > 0)
            {
                Console.WriteLine($"Total number of records: {results.Documents.Count} ");

                foreach (var customer in results.Documents)
                {
                    Console.WriteLine($"***{customer.Name}***");
                    Console.WriteLine($"Customer.Id:{customer.Id} Customer.Name:{customer.Name} Address:{customer.Address}");
                    //Console.WriteLine("**Orders**");
                    //foreach (var order in customer.Orders)
                    //{
                    //    Console.WriteLine($"Order.Id:{order.Id} Order.CustomerId:{order.CustomerId} Order.OrderDetails:{order.OrderDetails}");
                    //}

                    Console.WriteLine("\n");
                }
            }
            else
            {
                Console.WriteLine("Result not found \n");
            }

        }
    }
}
