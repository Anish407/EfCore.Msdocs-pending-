// See https://aka.ms/new-console-template for more information

using EfCore.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true);

var config = builder.Build();
try
{

    var connectionString = config["ConnectionString"];
    ServiceCollection services = new ServiceCollection();
    services.AddDbContext<AdventureWorks2016Context>(op => op.UseSqlServer(connectionString));
    IServiceProvider serviceProvider = services.BuildServiceProvider();

    var context = serviceProvider.GetRequiredService<AdventureWorks2016Context>();

    var data = context.Addresses.ToList();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}


Console.ReadLine();