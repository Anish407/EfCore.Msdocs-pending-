## Ef Core MsDocs

## Scaffold command
~~~
  dotnet ef dbcontext scaffold "Data Source=LAPTOP-computerName\SQLEXPRESS;Initial Catalog=AdventureWorks2016;Trusted_Connection=true;TrustServerCertificate=True"  Microsoft.EntityFrameworkCore.SqlServer -o Database

~~~

### Other links
- <a href="./EfficientQuerying.md">EfficientQuerying</a>
- <a href="./PerformanceDiagnosis.md">Performance Diagnosis</a>


### References
- https://learn.microsoft.com/en-us/ef/core/performance/performance-diagnosis?tabs=microsoft-extensions-logging%2Cload-entities
- https://learn.microsoft.com/en-us/ef/core/performance/efficient-querying