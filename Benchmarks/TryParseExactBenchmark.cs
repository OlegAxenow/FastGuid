using System;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class TryParseExactBenchmark
	{
		private string _stringD;
		private string _stringB;
		private string _stringN;
		private string _stringX;

		[GlobalSetup]
		public void Setup()
		{
			var guid = Guid.NewGuid();

			_stringD = guid.ToString("D");
			_stringB = guid.ToString("B");
			_stringN = guid.ToString("N");
			_stringX = guid.ToString("X");
		}

		[Benchmark(Baseline = true)]
		public bool GuidTryParseExactD()
		{
			return Guid.TryParseExact(_stringD, "D", out _);
		}

		/*[Benchmark]
		public bool GuidTryParseExactB()
		{
			return Guid.TryParseExact(_stringB, "B", out _);
		}

		[Benchmark]
		public bool GuidTryParseExactN()
		{
			return Guid.TryParseExact(_stringN, "N", out _);
		}

		[Benchmark]
		public bool GuidTryParseExactX()
		{
			return Guid.TryParseExact(_stringX, "X", out _);
		}*/

		[Benchmark]
		public bool UuidTryParseExactD()
		{
			return Uuid.TryParseExact(_stringD, "D", out _);
		}

		/*[Benchmark]
		public bool UuidTryParseExactB()
		{
			return Uuid.TryParseExact(_stringB, "B", out _);
		}

		[Benchmark]
		public bool UuidTryParseExactN()
		{
			return Uuid.TryParseExact(_stringN, "N", out _);
		}

		[Benchmark]
		public bool UuidTryParseExactX()
		{
			return Uuid.TryParseExact(_stringX, "X", out _);
		}*/
	}
}