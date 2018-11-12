using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class NewIdBenchmark
	{
		[Benchmark(Baseline = true)]
		public Guid NewGuid()
		{
			return Guid.NewGuid();
		}

		[Benchmark]
		public Uuid NewUuid()
		{
			return Uuid.NewUuid();
		}
	}
}