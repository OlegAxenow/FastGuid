using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	// TODO: why Uuid slower by 6% for single Equals call and by 20% for twice Equals call?
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class EqualsObjectBenchmark
	{
		private object _uuid1Object;
		private object _uuid2Object;

		private object _guid1Object;
		private object _guid2Object;

		private object _nullObject;

		[GlobalSetup]
		public void Setup()
		{
			_uuid1Object = Uuid.NewUuid();
			_nullObject = _uuid2Object = Uuid.NewUuid();

			_guid1Object = Guid.NewGuid();
			_guid2Object = Guid.NewGuid();

			_nullObject = null;
		}

		[Benchmark(Baseline = true)]
		public bool GuidDifferentEquals()
		{
			return _guid1Object.Equals(_guid2Object) || _guid2Object.Equals(_guid1Object);
		}

		[Benchmark]
		public bool GuidSameEquals()
		{
			return _guid1Object.Equals(_guid1Object) && _guid2Object.Equals(_guid2Object);
		}

		[Benchmark]
		public bool GuidNullEquals()
		{
			return _guid1Object.Equals(_nullObject) || _guid2Object.Equals(_nullObject);
		}

		[Benchmark]
		public bool UuidDifferentEquals()
		{
			return _uuid1Object.Equals(_uuid2Object) || _uuid2Object.Equals(_uuid1Object);
		}

		[Benchmark]
		public bool UuidSameEquals()
		{
			return _uuid1Object.Equals(_uuid1Object) && _uuid2Object.Equals(_uuid2Object);
		}

		[Benchmark]
		public bool UuidNullEquals()
		{
			return _uuid1Object.Equals(_nullObject) || _uuid2Object.Equals(_nullObject);
		}
	}
}