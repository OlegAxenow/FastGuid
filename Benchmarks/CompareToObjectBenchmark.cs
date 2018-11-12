using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class CompareToObjectBenchmark
	{
		private object _uuid1Object;
		private object _uuid2Object;

		private object _guid1Object;
		private object _guid2Object;
		
		private IComparable _uuid1;
		private IComparable _uuid2;
		private IComparable _guid1;
		private IComparable _guid2;

		[GlobalSetup]
		public void Setup()
		{
			var uuid1 = Uuid.NewUuid();
			var uuid2 = Uuid.NewUuid();
			
			_uuid1Object = uuid1;
			_uuid2Object = uuid2;
			_uuid1 = uuid1;
			_uuid2 = uuid2;

			var guid1 = Guid.NewGuid();
			var guid2 = Guid.NewGuid();
			
			_guid1Object = guid1;
			_guid2Object = guid2;
			_guid1 = guid1;
			_guid2 = guid2;
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