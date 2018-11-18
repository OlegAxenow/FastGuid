using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class CompareToBenchmark
	{
		private Uuid _uuid1;
		private Uuid _uuid2;
		private Uuid _uuid3;

		private Guid _guid1;
		private Guid _guid2;
		private Guid _guid3;

		[GlobalSetup]
		public void Setup()
		{
			_guid1 = Guid.NewGuid();
			_guid2 = Guid.NewGuid();
			_guid3 = Guid.NewGuid();
			
			_uuid1 = new Uuid(_guid1);
			_uuid2 = new Uuid(_guid2);
			_uuid3 = new Uuid(_guid3);
		}

		[Benchmark(Baseline = true)]
		public int GuidDifferentCompare()
		{
			return _guid1.CompareTo(_guid2) ^ _guid2.CompareTo(_guid3) ^ _guid3.CompareTo(_guid1);
		}

		[Benchmark]
		public int GuidSameCompare()
		{
			return _guid1.CompareTo(_guid1) ^ _guid2.CompareTo(_guid2) ^ _guid3.CompareTo(_guid3);
		}

		[Benchmark]
		public int UuidDifferentCompare()
		{
			return _uuid1.CompareTo(_uuid2) ^ _uuid2.CompareTo(_uuid3) ^ _uuid3.CompareTo(_uuid1);
		}

		[Benchmark]
		public int UuidSameCompare()
		{
			return _uuid1.CompareTo(_uuid1) ^ _uuid2.CompareTo(_uuid2) ^ _uuid3.CompareTo(_uuid3);
		}
	}
}