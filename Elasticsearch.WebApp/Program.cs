using Elasticsearch.Net;
using Nest;

var builder = WebApplication.CreateBuilder(args);

var nodes = new Uri[]
            {
                new Uri("http://localhost:9200/"),
            };

var connectionPool = new StaticConnectionPool(nodes);
var connectionSettings = new ConnectionSettings(connectionPool).DisableDirectStreaming();
connectionSettings.BasicAuthentication("elastic", "b=Ey5mn4Vw3Hwp3o1t4i");
var elasticClient = new ElasticClient(connectionSettings.DefaultIndex("productdetails"));

builder.Services.AddSingleton(elasticClient);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
