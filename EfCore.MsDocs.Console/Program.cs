using EfCore.Infra;

var stupidRepository = new StupidRepository();


//GetAllData(context);

await stupidRepository.CorrelatingDatabaseCommands();


Console.ReadLine();


#region AddDbProvider

// var builder = new ConfigurationBuilder()
//     .AddJsonFile($"appsettings.json", true, true); // not needed, but will use this in the future sometime
//
// var config = builder.Build();
// var connectionString = config["ConnectionString"];
// ServiceCollection services = new ServiceCollection();
// services.AddDbContext<AdventureWorks2016Context>(op =>
//     op.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information)); // Log sql queries to the console
// IServiceProvider serviceProvider = services.BuildServiceProvider();
//
// var context = serviceProvider.GetRequiredService<AdventureWorks2016Context>();

#endregion