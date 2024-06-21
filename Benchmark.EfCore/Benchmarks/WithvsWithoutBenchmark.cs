using BenchmarkDotNet.Attributes;
using EfCore.Infra;
using EfCore.Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Benchmark.EfCore.Benchmarks;

public class WithvsWithoutBenchmark
{


    public PersonRepositoryWithProperty PersonRepoWithProperty { get; set; }
    public PersonRepositoryWithoutProperty PersonRepoWithoutProperty { get; set; }
    [GlobalSetup]
    public void Setup()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddScoped<PersonRepositoryWithProperty>();
        services.AddScoped<PersonRepositoryWithoutProperty>();
        services.AddDbContext<AdventureWorks2016Context>(op =>
            op.UseSqlServer("\"Data Source=LAPTOP-FSA8LOOJ\\\\SQLEXPRESS;Initial Catalog=AdventureWorks2016;Trusted_Connection=true;TrustServerCertificate=True\"")
                .LogTo(Console.WriteLine, LogLevel.Information)); // Log sql queries to the console
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        PersonRepoWithProperty = serviceProvider.GetRequiredService<PersonRepositoryWithProperty>();
        PersonRepoWithoutProperty = serviceProvider.GetRequiredService<PersonRepositoryWithoutProperty>();
    }

    [Benchmark]
    public async Task FirstOrDefaultWithoutProperty()
    {
        _ = await PersonRepoWithoutProperty.GetPerson();
    }
    
    [Benchmark]
    public async Task FirstOrDefaultWithProperty()
    {
        _ = await PersonRepoWithoutProperty.GetPerson();
    }
}