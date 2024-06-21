using EfCore.Infra.Database;

namespace EfCore.Infra;

public class PersonRepositoryWithProperty:GenericRepositoryWithProperty<Person>
{
    public PersonRepositoryWithProperty(AdventureWorks2016Context context):base(context)
    {
    }

    public async Task<Person> GetPerson()
    {
      var person =await   FirstOrDefaultAsync(i => i.BusinessEntityId == 1);
      return person;
    }
}

public class PersonRepositoryWithoutProperty:GenericRepository<Person>
{
    public PersonRepositoryWithoutProperty(AdventureWorks2016Context context):base(context)
    {
    }

    public async Task<Person> GetPerson()
    {
        var person = await FirstOrDefaultAsync(i => i.BusinessEntityId == 1);
        return person;
    }
}