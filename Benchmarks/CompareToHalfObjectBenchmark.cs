using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class CompareToHalfObjectBenchmark
	{
		private Uuid _uuid1;
		private Uuid _uuid2;

		private object _uuid1Object;
		private object _uuid2Object;

		private Guid _guid1;
		private Guid _guid2;

		private object _guid1Object;
		private object _guid2Object;

		[GlobalSetup]
		public void Setup()
		{
			_uuid1Object = _uuid1 = Uuid.NewUuid();
			_uuid2Object = _uuid2 = Uuid.NewUuid();

			_guid1Object = _guid1 = Guid.NewGuid();
			_guid2Object = _guid2 = Guid.NewGuid();
		}

		[Benchmark(Baseline = true)]
		public int GuidDifferentCompare()
		{
			return _guid1.CompareTo(_guid2Object) ^ _guid2.CompareTo(_guid1Object);
		}

		[Benchmark]
		public int GuidSameCompare()
		{
			return _guid1.CompareTo(_guid1Object) ^ _guid2.CompareTo(_guid2Object);
		}

		[Benchmark]
		public int UuidDifferentCompare()
		{
			return _uuid1.CompareTo(_uuid2Object) ^ _uuid2.CompareTo(_uuid1Object);
		}

		[Benchmark]
		public int UuidSameCompare()
		{
			return _uuid1.CompareTo(_uuid1Object) ^ _uuid2.CompareTo(_uuid2Object);
		}
	}
}