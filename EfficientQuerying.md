## Performance Diagnosis

#### Identifying slow database commands via logging

- At the end of the day, EF prepares and executes commands to be executed against your database; with relational database, that means executing SQL statements via the ADO.NET database API. If a certain query is taking too much time (e.g. because an index is missing), this can be seen discovered by inspecting command execution logs and observing how long they actually take.
- EF makes it very easy to capture command execution times, via either simple logging or Microsoft.Extensions.Logging:
##### Simple Logging
~~~ 
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True")
        .LogTo(Console.WriteLine, LogLevel.Information);
}
~~~
##### Microsoft.Extensions.Logging
~~~ 
private static ILoggerFactory ContextLoggerFactory
    => LoggerFactory.Create(b => b.AddConsole().AddFilter("", LogLevel.Information));

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True")
        .UseLoggerFactory(ContextLoggerFactory);
}
~~~
When the logging level is set at LogLevel.Information, EF emits a log message for each command execution with the time taken:

-  If a certain command takes more than expected, you've found a possible culprit for a performance issue, and can now focus on it to understand why it's running slowly. Command logging can also reveal cases where unexpected database roundtrips are being made; this would show up as multiple commands where only one is expected.

~~~
Leaving command execution logging enabled in your production environment is usually a bad idea. The logging itself slows down your application, and may quickly create huge log files which can fill up your server's disk. It's recommended to only keep logging on for a short interval of time to gather data - while carefully monitoring your application - or to capture logging data on a pre-production system
~~~

## Scaffold command
~~~
  dotnet ef dbcontext scaffold "Data Source=LAPTOP-computerName\SQLEXPRESS;Initial Catalog=AdventureWorks2016;Trusted_Connection=true;TrustServerCertificate=True"  Microsoft.EntityFrameworkCore.SqlServer -o Database

~~~

### Use indexes properly
