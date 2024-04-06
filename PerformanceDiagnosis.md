## Performance Diagnosis

### Identifying slow database commands via logging

- At the end of the day, EF prepares and executes commands to be executed against your database; with relational database, that means executing SQL statements via the ADO.NET database API. If a certain query is taking too much time (e.g. because an index is missing), this can be seen discovered by inspecting command execution logs and observing how long they actually take.
- EF makes it very easy to capture command execution times, via either simple logging or Microsoft.Extensions.Logging:
#### 1. Simple Logging
~~~ 
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True")
        .LogTo(Console.WriteLine, LogLevel.Information);
}
~~~
#### 2. Microsoft.Extensions.Logging
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


> Leaving command execution logging enabled in your production environment is usually a bad idea. The logging itself slows down your application, and may quickly create huge log files which can fill up your server's disk. It's recommended to only keep logging on for a short interval of time to gather data - while carefully monitoring your application - or to capture logging data on a pre-production system

#### 3. Correlating database commands to LINQ queries

<p>One problem with command execution logging is that it's sometimes difficult to correlate SQL queries and LINQ queries: the SQL commands executed by EF can look very different from the LINQ queries from which they were generated. To help with this difficulty, you may want to use EF's query tags feature, which allows you to inject a small, identifying comment into the SQL query</p>

~~~
var myLocation = new Point(1, 2);
var nearestPeople = (from f in context.People.TagWith("This is my spatial query!")
                     orderby f.Location.Distance(myLocation) descending
                     select f).Take(5).ToList();
~~~

~~~
-- This is my spatial query!

SELECT TOP(@__p_1) [p].[Id], [p].[Location]
FROM [People] AS [p]
ORDER BY [p].[Location].STDistance(@__myLocation_0) DESC
~~~