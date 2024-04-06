// See https://aka.ms/new-console-template for more information

using Benchmark.EfCore.Benchmarks;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<StupidRepositoryBenchMark>();
Console.ReadLine();