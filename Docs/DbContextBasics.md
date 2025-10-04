## DbContext Lifetime, Configuration, and Initialization
The lifetime of a DbContext begins when the instance is created and ends when the instance is disposed. A DbContext instance is designed to be used for a single unit-of-work. 
This means that the lifetime of a DbContext instance is usually very short.

> To quote Martin Fowler from the link above, "A Unit of Work keeps track of everything you do during a business transaction that can affect the database. When you're done, it figures out everything that needs to be done to alter the database as a result of your work."

> A typical unit-of-work when using Entity Framework Core (EF Core) involves:

 - Creation of a DbContext instance
 - Tracking of entity instances by the context. Entities become tracked by
 - Being returned from a query
 - Being added or attached to the context
 - Changes are made to the tracked entities as needed to implement the business rule
 - SaveChanges or SaveChangesAsync is called. EF Core detects the changes made and writes them to the database.
 - The DbContext instance is disposed

## > [! Important]
> - It is important to dispose the DbContext after use. This ensures any:
> - Unmanaged resources are freed.
> - Events or other hooks are unregistered. Unregistering prevents memory leaks when the instance remains referenced.
> - DbContext is Not thread-safe. Don't share contexts between threads. Make sure to await all async calls before continuing to use the context instance.

> ###  An InvalidOperationException thrown by EF Core code can put the context into an unrecoverable state. Such exceptions indicate a program error and are not designed to be recovered from.

### DbContext in dependency injection for ASP.NET Core

In ASP.NET Core applications, the DbContext is typically registered with the built-in dependency injection (DI) container. The recommended lifetime for a DbContext in a web application is Scoped. This means that a new instance of the DbContext is created for each HTTP request and disposed at the end of the request.
This is done by calling the `AddDbContext` method in the `ConfigureServices` method of the `Startup` class:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<YourDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
}
``` 
In this example, `YourDbContext` is registered with a Scoped lifetime. The connection string is retrieved from the configuration.

### Basic DbContext initialization with 'new', DbContext in other application types    
In non-web applications, such as console applications or desktop applications, the DbContext can be created and disposed of manually. It is important to ensure that the DbContext is disposed of properly to avoid memory leaks and other issues. Here is an example of how to use a DbContext in a console application:       
```csharp
using (var context = new YourDbContext())
{
    // Use the context here
}
``` 
In this example, the DbContext is created using the `new` keyword and disposed of using a `using` statement. This ensures that the DbContext is disposed of properly when it is no longer needed.
This pattern also makes it easy to pass configuration like the connection string via the DbContext constructor. For example:

```public class ApplicationDbContext : DbContext
{
private readonly string _connectionString;

    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
```
In addition, OnConfiguring is always called regardless of how the context is constructed. This means OnConfiguring can be used to perform additional configuration even when AddDbContext is being used.


### Configuring the DbContext
The DbContext can be configured in several ways, including:
- Overriding the `OnConfiguring` method in the DbContext class
- Using the `DbContextOptions` parameter in the DbContext constructor
- Using the `AddDbContext` method in ASP.NET Core applications
  - Using a factory method to create the DbContext
  - Using a connection string from configuration
  - Using different database providers (e.g., SQL Server, SQLite, PostgresSQL, etc.)
  - Enabling lazy loading, change tracking, and other options
  - Configuring logging and diagnostics

### Configuring the database provider
Each DbContext instance must be configured to use one and only one database provider. (Different instances of a DbContext subtype can be used with different database providers, but a single instance must only use one.) A database provider is configured using a specific Use* call. For example, to use the SQL Server database provider:

```
public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=Test;ConnectRetryCount=0");
    }
}
```
These Use* methods are extension methods implemented by the database provider. This means that the database provider NuGet package must be installed before the extension method can be used.



### Initializing the DbContext
The DbContext can be initialized in several ways, including:
- Using the `Database.EnsureCreated` method to create the database if it does not exist
- Using the `Database.Migrate` method to apply any pending migrations to the database
- Using the `DbContext.Database.ExecuteSqlRaw` method to execute raw SQL commands
- Seeding the database with initial data using the `ModelBuilder` in the `OnModelCreating` method or using a custom initializer

### Use a DbContext factory
In scenarios where you need to create DbContext instances manually, such as in background services or multithreaded applications, consider using a DbContext factory. This approach helps manage the lifetime of DbContext instances and ensures they are created with the correct configuration.
You can register a factory for your DbContext in the DI container like this:
```csharp
services.AddDbContextFactory<YourDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
```
The ApplicationDbContext class must expose a public constructor with a DbContextOptions<ApplicationDbContext> parameter. 
```
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
```
Then, you can inject the factory into your services and create DbContext instances as needed:
```csharp
public class YourService
{
    private readonly IDbContextFactory<YourDbContext> _contextFactory;  
    public YourService(IDbContextFactory<YourDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task DoWorkAsync()
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            // ...
        }
    }
}
```

### DbContextOptions versus DbContextOptions<TContext>
When configuring a DbContext, you can use either `DbContextOptions` or `DbContextOptions<TContext>`. The main difference between the two is that `DbContextOptions<TContext>` is a generic type that is specific to a particular DbContext type, while `DbContextOptions` is a non-generic type that can be used with any DbContext type.
Using `DbContextOptions<TContext>` provides better type safety and allows for more specific configuration options for the DbContext type. It is generally recommended to use `DbContextOptions<TContext>` when configuring a DbContext.

```
public sealed class SealedApplicationDbContext : DbContext
{
    public SealedApplicationDbContext(DbContextOptions<SealedApplicationDbContext> contextOptions)
        : base(contextOptions)
    {
    }
}
```
This ensures that the correct options for the specific DbContext subtype are resolved from dependency injection, even when multiple DbContext subtypes are registered.

> [!Note] 
> Your DbContext does not need to be sealed, but sealing is best practice to do so for classes not designed to be inherited from.

### Avoiding DbContext threading issues
Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This includes both parallel execution of async queries and any explicit concurrent use from multiple threads. Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute in parallel.

When EF Core detects an attempt to use a DbContext instance concurrently, you'll see an InvalidOperationException with a message like this:

> A second operation started on this context before a previous operation completed. This is usually caused by different threads using the same instance of DbContext, however instance members are not guaranteed to be thread safe.

When concurrent access goes undetected, it can result in undefined behavior, application crashes and data corruption.

### Asynchronous operation pitfalls
Asynchronous methods enable EF Core to initiate operations that access the database in a non-blocking way. 
But if a caller does not await the completion of one of these methods, and proceeds to perform other operations on the DbContext, the state of the DbContext can be, (and very likely will be) corrupted.

### Implicitly sharing DbContext instances via dependency injection
The AddDbContext extension method registers DbContext types with a scoped lifetime by default.

This is safe from concurrent access issues in most ASP.NET Core applications because there is only one thread executing each client request at a given time, and because each request gets a separate dependency injection scope (and therefore a separate DbContext instance).

> Any code that explicitly executes multiple threads in parallel should ensure that DbContext instances aren't ever accessed concurrently.

> Using dependency injection, this can be achieved by either registering the context as scoped, and creating scopes (using IServiceScopeFactory) for each thread, or by registering the DbContext as transient (using the overload of AddDbContext which takes a ServiceLifetime parameter).


### Summary
In summary, the DbContext is a crucial part of Entity Framework Core that manages the connection to the database and tracks changes to entities. It is important to manage the lifetime of the DbContext properly, especially in web applications, to ensure that resources are used efficiently and that the application performs well.    
For more information, see the following resources:
- [DbContext lifetime, configuration, and initialization](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/)
- [Dependency injection in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- [Working with DbContext](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/)
- [EF Core logging and diagnostics](https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/)
- [EF Core migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)