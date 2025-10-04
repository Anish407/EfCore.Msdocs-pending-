// See https://aka.ms/new-console-template for more information

using Benchmark.EfCore.Benchmarks;
using BenchmarkDotNet.Running;
using EfCore.Infra.Database;
using Microsoft.Extensions.Logging;

var summary = BenchmarkRunner.Run<WithvsWithoutBenchmark>();
Console.ReadLine();


public interface IProduct
{
    public int Name { get; set; }
}

public class Product
{
    
}