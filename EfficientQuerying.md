
## Scaffold command
~~~
  dotnet ef dbcontext scaffold "Data Source=LAPTOP-computerName\SQLEXPRESS;Initial Catalog=AdventureWorks2016;Trusted_Connection=true;TrustServerCertificate=True"  Microsoft.EntityFrameworkCore.SqlServer -o Database

~~~

### Use indexes properly

- The main deciding factor in whether a query runs fast or not is whether it will properly utilize indexes where appropriate: databases are typically used to hold large amounts of data, 
and queries which traverse entire tables are typically sources of serious performance issues. 
Indexing issues aren't easy to spot, because it isn't immediately obvious whether a given query will use an index or not.
- A good way to spot indexing issues is to first pinpoint a slow query, and then examine its query plan via your database's favorite tool; see the performance diagnosis page for more information on how to do that. The query plan displays whether the query traverses the entire table, or uses an index.

### Project only properties you need
- Querying entity instances can frequently pull back more data than necessary from your database.
- If you need to project out more than one column, project out to a C# anonymous type with the properties you want. Note that this technique is very useful for read-only queries, but things get more complicated if you need to update the fetched blogs, since EF's change tracking only works with entity instances. It's possible to perform updates without loading entire entities by attaching a modified Blog instance and telling EF which properties have changed.

### Limit the resultset size

- Since the number of rows returned depends on actual data in your database, it's impossible to know how much data will be loaded from the database, how much memory will be taken up by the results, and how much additional load will be generated when processing these results (e.g. by sending them to a user browser over the network). Crucially, test databases frequently contain little data, so that everything works well while testing, but performance problems suddenly appear when the query starts running on real-world data and many rows are returned.
~~~
var blogs25 = context.Posts
    .Where(p => p.Title.StartsWith("A"))
    .Take(25)
    .ToList();
~~~

### Avoid cartesian explosion when loading related entities
In relational databases, all related entities are loaded by introducing JOINs in single query.
~~~
SELECT [b].[BlogId], [b].[OwnerId], [b].[Rating], [b].[Url], [p].[PostId], [p].[AuthorId], [p].[BlogId], [p].[Content], [p].[Rating], [p].[Title]
FROM [Blogs] AS [b]
LEFT JOIN [Post] AS [p] ON [b].[BlogId] = [p].[BlogId]
ORDER BY [b].[BlogId], [p].[PostId]
~~~

If a typical blog has multiple related posts, rows for these posts will duplicate the blog's information. This duplication leads to the so-called "cartesian explosion" problem. As more one-to-many relationships are loaded, the amount of duplicated data may grow and adversely affect the performance of your application.
EF allows avoiding this effect via the use of "split queries", which load the related entities via separate queries. For more information, read the documentation on split and single queries.