using EfCore.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Infra;

public class StupidRepository
{
    
    public async Task CorrelatingDatabaseCommands()
    {
        var result =  await ExecuteDbOperation( context =>
        {
            var query = (from p in context.People
                join pwd in context.Passwords 
                    on p.BusinessEntityId equals pwd.BusinessEntityId
                group p by p.BusinessEntityId into g
                select new
                {
                    g.Key,
                    Count =  g.Count()
                }).TagWith("Join and Group");

            var r =  query.ToListAsync();

            return r;

        });
    }
    public async Task<List<Address>> GetAllData()
    {
        var result = await ExecuteDbOperation(context => context.Addresses.ToListAsync());
        
        return result;
    }

    public async Task<TResponse> ExecuteDbOperation<TResponse>(Func<AdventureWorks2016Context, Task<TResponse>> dbOperation)
    {
        try
        {
            using (var context = new AdventureWorks2016Context())
            {
                return  await dbOperation(context);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}, stackTrace:{e.StackTrace}, InnerException:{e.InnerException}");
            throw;
        }
    }
}