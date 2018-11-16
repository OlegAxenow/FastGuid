using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class GuidConversionBenchmark
	{
		private Guid _guid1;
		private byte[] _bytes;

		[GlobalSetup]
		public void Setup()
		{
			_guid1 = Guid.NewGuid();
			_bytes = _guid1.ToByteArray();
		}

		[Benchmark(Baseline = true)]
		public ValueTuple<Guid, Guid, Guid> BaseLine_GuidBytesCtor3Times()
		{
			return (new Guid(_bytes), new Guid(_bytes), new Guid(_bytes));
		}

		[Benchmark]
		public ValueTuple<Uuid, Uuid, Uuid, Uuid, Uuid, Uuid, Uuid> UuidCtorFromGuid7Times()
		{
			return (new Uuid(_guid1), new Uuid(_guid1), new Uuid(_guid1), new Uuid(_guid1), new Uuid(_guid1), new Uuid(_guid1), new Uuid(_guid1));
		}

		[Benchmark]
		public ValueTuple<Uuid, Uuid, Uuid, Uuid, Uuid, Uuid, Uuid> UuidCtorRefFromGuid7Times()
		{
			return (new Uuid(ref _guid1), new Uuid(ref _guid1), new Uuid(ref _guid1),
				new Uuid(ref _guid1), new Uuid(ref _guid1), new Uuid(ref _guid1), new Uuid(ref _guid1));
		}

		[Benchmark]
		public ValueTuple<Uuid, Uuid, Uuid, Uuid, Uuid, Uuid, Uuid> UuidExplicitFromGuid7Times()
		{
			return ((Uuid)_guid1, (Uuid)_guid1, (Uuid)_guid1, (Uuid)_guid1, (Uuid)_guid1, (Uuid)_guid1, (Uuid)_guid1);
		}
	}
}