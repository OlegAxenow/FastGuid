using System;
using BenchmarkDotNet.Attributes;
using FastGuid;
using FastGuid.Temp;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class EqualsBenchmark
	{
		private SimpleGuid _simpleGuid1;
		private SimpleGuid _simpleGuid2;
		private SimpleGuid _simpleGuid3;

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

			_simpleGuid1 = new SimpleGuid(_guid1);
			_simpleGuid2 = new SimpleGuid(_guid2);
			_simpleGuid3 = new SimpleGuid(_guid3);
		}

		[Benchmark(Baseline = true)]
		public bool GuidDifferentEquals()
		{
			return _guid1.Equals(_guid2) || _guid2.Equals(_guid3) || _guid3.Equals(_guid1);
		}

		[Benchmark]
		public bool GuidSameEquals()
		{
			return _guid1.Equals(_guid1) && _guid2.Equals(_guid2) && _guid3.Equals(_guid3);
		}

		[Benchmark]
		public bool UuidDifferentEquals()
		{
			return _uuid1.Equals(_uuid2) || _uuid2.Equals(_uuid3) || _uuid3.Equals(_uuid1);
		}

		[Benchmark]
		public bool UuidSameEquals()
		{
			return _uuid1.Equals(_uuid1) && _uuid2.Equals(_uuid2) && _uuid3.Equals(_uuid3);
		}

		[Benchmark]
		public bool SimpleGuidDifferentEquals()
		{
			return _simpleGuid1.Equals(_simpleGuid2) || _simpleGuid2.Equals(_simpleGuid3) || _simpleGuid3.Equals(_simpleGuid1);
		}

		[Benchmark]
		public bool SimpleGuidSameEquals()
		{
			return _simpleGuid1.Equals(_simpleGuid1) && _simpleGuid2.Equals(_simpleGuid2) && _simpleGuid3.Equals(_simpleGuid3);
		}
	}
}