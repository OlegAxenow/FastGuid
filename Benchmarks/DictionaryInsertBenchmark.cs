using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class DictionaryInsertBenchmark
	{
		private Uuid[] _uuids;
		private Guid[] _guids;

		[Params(1000)]
		public int TotalCount;

		[GlobalSetup]
		public void Setup()
		{
			_uuids = new Uuid[TotalCount];
			_guids = new Guid[TotalCount];

			for (int i = 0; i < TotalCount; i++)
			{
				_guids[i] = Guid.NewGuid();
				_uuids[i] = Uuid.NewUuid();
			}
		}

		[Benchmark(Baseline = true)]
		public void InsertGuid()
		{
			var guids = new Dictionary<Guid, bool>(TotalCount);
			for (int i = 0; i < TotalCount; i++)
			{
				guids.Add(_guids[i], true);
			}
		}

		[Benchmark]
		public void InsertUuid()
		{
			// TODO: why allocated memory bigger than for Guid? 3.89KB vs. 3.48KB for 100 items? may be GetHashCode too simple?
			var uuids = new Dictionary<Uuid, bool>(TotalCount);
			for (int i = 0; i < TotalCount; i++)
			{
				uuids.Add(_uuids[i], true);
			}
		}
	}
}