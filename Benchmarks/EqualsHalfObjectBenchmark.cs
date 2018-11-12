using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class EqualsHalfObjectBenchmark
	{
		private Uuid _uuid1;
		private Uuid _uuid2;

		private object _uuid1Object;
		private object _uuid2Object;

		private Guid _guid1;
		private Guid _guid2;

		private object _guid1Object;
		private object _guid2Object;

		private object _nullObject;

		[GlobalSetup]
		public void Setup()
		{
			_uuid1Object = _uuid1 = Uuid.NewUuid();
			_nullObject = _uuid2Object = _uuid2 = Uuid.NewUuid();

			_guid1Object = _guid1 = Guid.NewGuid();
			_guid2Object = _guid2 = Guid.NewGuid();

			_nullObject = null;
		}

		[Benchmark(Baseline = true)]
		public bool GuidDifferentEquals()
		{
			return _guid1.Equals(_guid2Object) || _guid2.Equals(_guid1Object);
		}

		[Benchmark]
		public bool GuidSameEquals()
		{
			return _guid1.Equals(_guid1Object) && _guid2.Equals(_guid2Object);
		}

		[Benchmark]
		public bool GuidNullEquals()
		{
			return _guid1.Equals(_nullObject) || _guid2.Equals(_nullObject);
		}

		[Benchmark]
		public bool UuidDifferentEquals()
		{
			return _uuid1.Equals(_uuid2Object) || _uuid2.Equals(_uuid1Object);
		}

		[Benchmark]
		public bool UuidSameEquals()
		{
			return _uuid1.Equals(_uuid1Object) && _uuid2.Equals(_uuid2Object);
		}

		[Benchmark]
		public bool UuidNullEquals()
		{
			return _uuid1.Equals(_nullObject) || _uuid2.Equals(_nullObject);
		}
	}
}