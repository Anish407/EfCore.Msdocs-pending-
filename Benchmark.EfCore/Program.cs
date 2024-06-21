// See https://aka.ms/new-console-template for more information

using Benchmark.EfCore.Benchmarks;
using BenchmarkDotNet.Running;
using EfCore.Infra.Database;
using Microsoft.Extensions.Logging;

ILogger<Product> logger= new Logger<Product>(null);
logger.LogInformation("");
var summary = BenchmarkRunner.Run<StupidRepositoryBenchMark>();
Console.ReadLine();