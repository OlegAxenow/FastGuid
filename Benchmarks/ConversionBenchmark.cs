using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class ConversionBenchmark
	{
		private Guid _guid1;

		[GlobalSetup]
		public void Setup()
		{
			_guid1 = Guid.NewGuid();
		}

		[Benchmark(Baseline = true)]
		public Uuid Baseline()
		{
			Uuid id = new Uuid();

			for (int i = 0; i < 2; i++ )
			{
				id = new Uuid();
			}

			return id;
		}

		[Benchmark]
		public Uuid Constructor()
		{
			return new Uuid(_guid1);
		}

		[Benchmark]
		public Uuid ImplicitConversion()
		{
			return _guid1;
		}
	}
}