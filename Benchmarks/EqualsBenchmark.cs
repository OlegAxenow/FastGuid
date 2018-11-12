using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class EqualsBenchmark
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
		public bool GuidDifferentEquals()
		{
			return _guid1.Equals(_guid2) || _guid2.Equals(_guid1);
		}

		[Benchmark]
		public bool GuidSameEquals()
		{
			return _guid1.Equals(_guid1) && _guid2.Equals(_guid2);
		}

		[Benchmark]
		public bool UuidDifferentEquals()
		{
			return _uuid1.Equals(_uuid2) || _uuid2.Equals(_uuid1);
		}

		[Benchmark]
		public bool UuidSameEquals()
		{
			return _uuid1.Equals(_uuid1) && _uuid2.Equals(_uuid2);
		}
	}
}