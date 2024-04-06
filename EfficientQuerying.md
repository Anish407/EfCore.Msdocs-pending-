
## Scaffold command
~~~
  dotnet ef dbcontext scaffold "Data Source=LAPTOP-computerName\SQLEXPRESS;Initial Catalog=AdventureWorks2016;Trusted_Connection=true;TrustServerCertificate=True"  Microsoft.EntityFrameworkCore.SqlServer -o Database

~~~

### Use indexes properly

<p>The main deciding factor in whether a query runs fast or not is whether it will properly utilize indexes where appropriate: databases are typically used to hold large amounts of data, and queries which traverse entire tables are typically sources of serious performance issues. Indexing issues aren't easy to spot, because it isn't immediately obvious whether a given query will use an index or not.</p>