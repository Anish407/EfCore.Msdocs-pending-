using BenchmarkDotNet.Attributes;
using EfCore.Infra;

namespace Benchmark.EfCore.Benchmarks;

[MemoryDiagnoser]
public partial class StupidRepositoryBenchMark
{
    
    
    public async Task CorrelatingDatabaseCommands()
    {
        var repo = new StupidRepository();
         await repo.CorrelatingDatabaseCommands();
    }
    
    [Benchmark]
    public async Task GetAllData()
    {
        var repo = new StupidRepository();
        await repo.GetAllData();
    }
}