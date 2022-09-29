using Elasticsearch.ConsoleApp.Entities;
using Nest;

namespace Elasticsearch.ConsoleApp.Data
{
    public class CustomersData
    {
        private ElasticClient _client;
        public CustomersData(ElasticClient client)
        {
            _client = client;
        }

        public void InsertDataInElasticSearch()
        {
            Customer customer = GetSingleDataToInsert();
            _client.IndexDocument(customer); //Single data

            IEnumerable<Customer> multipleData = CustomersData.GetMultipleDataToInsert();
            _client.IndexMany(multipleData); // Multiple data
        }

        
        private static Customer GetSingleDataToInsert()
        {
            return new Customer
            {
                Id = 1,
                Name = "Kashif",
                Age = 35,
                Birthday = "14th July",
                Address = "D2/2A, Classic Appartment Shaheen Bagh New Delhi",
                Orders = GetOrderDataData(1)
            };
        }

        private static IEnumerable<Customer> GetMultipleDataToInsert()
        {
            return new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    Name = "Kashif",
                    Age = 35,
                    Birthday = "14th July",
                    Address = "D2/2A, Classic Appartment Shaheen Bagh New Delhi",
                    Orders = GetOrderDataData(1)
                },
                new Customer
                {
                    Id = 2,
                    Name = "Danish",
                    Age = 33,
                    Birthday = "10th Oct",
                    Address= "G-12/248-A Sanagam Vihar New Delhi",
                    Orders = GetOrderDataData(2)
                },
                new Customer
                {
                    Id = 3,
                    Name = "Kashif Reza",
                    Age = 35,
                    Birthday = "14th July",
                    Address = "9 3rd Flr Sabir Appartment Nizamuddin New Delhi",
                    Orders = GetOrderDataData(3)
                },

                new Customer
                {
                    Id = 4,
                    Name = "Danish Ajazi",
                    Age = 33,
                    Birthday = "10th Oct",
                    Address = "Dharampur Samastipur Bihar",
                    Orders = GetOrderDataData(4)
                }
            };
        }

        private static List<Order> GetOrderDataData(int customerId)
        {
            return new List<Order>
            {
                new Order{
                    Id = 1,
                    CustomerId = customerId,
                    OrderDetails = $"Order 1 of customer {customerId}"
                },
                new Order
                {
                    Id= 2,
                    CustomerId = customerId,
                    OrderDetails = $"Order 2 of customer {customerId}",
                }
            };
        }
    }
}
