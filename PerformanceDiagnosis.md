## Performance Diagnosis

### Identifying slow database commands via logging

- At the end of the day, EF prepares and executes commands to be executed against your database; with relational database, that means executing SQL statements via the ADO.NET database API. If a certain query is taking too much time (e.g. because an index is missing), this can be seen discovered by inspecting command execution logs and observing how long they actually take.
- EF makes it very easy to capture command execution times, via either simple logging or Microsoft.Extensions.Logging:
### 1. Simple Logging
~~~ 
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True")
        .LogTo(Console.WriteLine, LogLevel.Information);
}
~~~
### 2. Microsoft.Extensions.Logging
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

### 3. Correlating database commands to LINQ queries

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

### 4. Inspecting query execution plans (Pending: learn more)
<p>
Once you've pinpointed a problematic query that requires optimization, the next step is usually analyzing the query's execution plan. When databases receive a SQL statement, they typically produce a plan of how that plan is to be executed; this sometimes requires complicated decision-making based on which indexes have been defined, how much data exists in tables, etc. (incidentally, the plan itself should usually be cached at the server for optimal performance). 
To get started on SQL Server, see the documentation on query execution plans. The typical analysis workflow would be to use SQL Server Management Studio, pasting the SQL of a slow query identified via one of the means above, and producing a graphical execution plan:
</p>

### 5. Event counters (Pending: learn more)
<p>The above sections focused on how to get information about your commands, and how these commands are executed in the database. In addition to that, EF exposes a set of event counters which provide more lower-level information on what's happening inside EF itself, and how your application is using it. These counters can be very useful for diagnosing specific performance issues and performance anomalies, such as query caching issues which cause constant recompilation, undisposed DbContext leaks, and others.</p>

### 6. Benchmarking with EF Core (Pending: learn more)
<p> When writing benchmarks, it's strongly recommended to use the well-known BenchmarkDotNet library, which handles many pitfalls users encounter when trying to write their own benchmarks: have you performed some warmup iterations? How many iterations does your benchmark actually run, and why?</p>