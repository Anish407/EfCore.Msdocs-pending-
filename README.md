## Ef Core MsDocs

## Scaffold command
~~~
  dotnet ef dbcontext scaffold "Data Source=LAPTOP-computerName\SQLEXPRESS;Initial Catalog=AdventureWorks2016;Trusted_Connection=true;TrustServerCertificate=True"  Microsoft.EntityFrameworkCore.SqlServer -o Database

~~~

### Other links
- <a href="./Docs/EfficientQuerying.md">EfficientQuerying</a>
- <a href="./Docs/PerformanceDiagnosis.md">Performance Diagnosis</a>


### References
- https://learn.microsoft.com/en-us/ef/core/performance/performance-diagnosis?tabs=microsoft-extensions-logging%2Cload-entities
- https://learn.microsoft.com/en-us/ef/core/performance/efficient-querying
- https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.connectionstring?view=dotnet-plat-ext-8.0
- https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/connection-string-syntax
- https://learn.microsoft.com/en-us/sql/relational-databases/performance/monitor-and-tune-for-performance?view=sql-server-ver16 (monitor Sql server performance)
- https://learn.microsoft.com/en-us/sql/relational-databases/performance/execution-plans?view=sql-server-ver16 -- query execution plan
- https://blog.nashtechglobal.com/entity-framework-core-internals-query-pipelines/ -- view all previous blogs in this series
