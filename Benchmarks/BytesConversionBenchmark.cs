using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class BytesConversionBenchmark
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
		public ValueTuple<Guid, Guid, Guid, Guid, Guid, Guid> GuidBytesCtor()
		{
			return (new Guid(_bytes), new Guid(_bytes), new Guid(_bytes), new Guid(_bytes), new Guid(_bytes), new Guid(_bytes));
		}

		[Benchmark]
		public ValueTuple<Uuid, Uuid, Uuid, Uuid, Uuid, Uuid> UuidBytesCtor()
		{
			return (new Uuid(_bytes), new Uuid(_bytes), new Uuid(_bytes), new Uuid(_bytes), new Uuid(_bytes), new Uuid(_bytes));
		}
	}
}