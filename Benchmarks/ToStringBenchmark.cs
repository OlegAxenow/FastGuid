using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class ToStringBenchmark
	{
		private Guid _guid1;
		private Uuid _uuid1;

		[GlobalSetup]
		public void Setup()
		{
			_guid1 = Guid.NewGuid();
			_uuid1 = (Uuid)_guid1;
		}

		[Benchmark(Baseline = true)]
		public string GuidToStringD()
		{
			return _guid1.ToString("D");
		}

		[Benchmark]
		public string GuidToStringB()
		{
			return _guid1.ToString("B");
		}

		[Benchmark]
		public string GuidToStringN()
		{
			return _guid1.ToString("N");
		}

		[Benchmark]
		public string GuidToStringX()
		{
			return _guid1.ToString("X");
		}

		[Benchmark]
		public string UuidToStringD()
		{
			return _uuid1.ToString("D");
		}

		[Benchmark]
		public string UuidToStringB()
		{
			return _uuid1.ToString("B");
		}

		[Benchmark]
		public string UuidToStringN()
		{
			return _uuid1.ToString("N");
		}

		[Benchmark]
		public string UuidToStringX()
		{
			return _uuid1.ToString("X");
		}
	}
}