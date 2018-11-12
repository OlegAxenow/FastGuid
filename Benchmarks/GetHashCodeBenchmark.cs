using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class GetHashCodeBenchmark
	{
		private Uuid _uuid1;
		private Uuid _uuid2;

		private Guid _guid1;
		private Guid _guid2;

		[GlobalSetup]
		public void Setup()
		{
			_uuid1 = Uuid.NewUuid();
			_uuid2 = Uuid.NewUuid();

			_guid1 = Guid.NewGuid();
			_guid2 = Guid.NewGuid();
		}

		[Benchmark(Baseline = true)]
		public int GuidGetHashCode()
		{
			return _guid1.GetHashCode() ^ _guid2.GetHashCode();
		}

		[Benchmark]
		public int UuidGetHashCode()
		{
			// if just return _uuid1.GetHashCode() - benchmark will be failed because of too fast
			return _uuid1.GetHashCode() ^ _uuid2.GetHashCode();
		}
	}
}