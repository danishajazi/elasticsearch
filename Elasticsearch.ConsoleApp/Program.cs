// See https://aka.ms/new-console-template for more information
using Elasticsearch.ConsoleApp;
using Elasticsearch.ConsoleApp.Data;
using Elasticsearch.ConsoleApp.Entities;
using Nest;

Console.WriteLine("Hello, World!");

var _client = ElasticSearchClient.GetElasticSearchClient();

new CustomersData(_client).InsertDataInElasticSearch();

Search searchQuery = new Search(_client);

ISearchResponse<Customer> results;

Console.WriteLine("Displaying All result");
results = searchQuery.GetAll();

PrintResult.DisplaySearchResult(results);

Console.WriteLine("\n");
Console.WriteLine("Enter text to search");
string searchKeyword = Console.ReadLine();
Console.WriteLine("\n");

results = searchQuery.GetAllBySearchText(searchKeyword);

PrintResult.DisplaySearchResult(results);

results = searchQuery.GetByWildcardSearchOnOneFiled(searchKeyword);

PrintResult.DisplaySearchResult(results);

results = searchQuery.GetByWildcardSearchInMultipleFileds(searchKeyword);

PrintResult.DisplaySearchResult(results);

results = searchQuery.GetAllDataWithPaging(skip: 5, take:5);

PrintResult.DisplaySearchResult(results);

results = searchQuery.GetAllDataWithSorting();

PrintResult.DisplaySearchResult(results);

