using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using FastGuid;

namespace Benchmarks
{
	[Config(typeof(CustomConfig))]
	[MinColumn, MaxColumn]
	public class DictionarySearchBenchmark
	{
		private Uuid[] _uuidToMatch;
		private Guid[] _guidToMatch;

		private Uuid[] _uuidToNotMatch;
		private Uuid[] _uuidToNotMatch2;
		private Guid[] _guidToNotMatch;
		private Guid[] _guidToNotMatch2;

		private Dictionary<Uuid, bool> _uuids;
		private Dictionary<Guid, bool> _guids;

		[Params(1000)]
		public int TotalCount;

		[Params(10, 100)]
		public int MatchCount;

		[GlobalSetup]
		public void Setup()
		{
			_uuidToMatch = new Uuid[MatchCount];
			_guidToMatch = new Guid[MatchCount];

			_uuidToNotMatch = new Uuid[MatchCount];
			_guidToNotMatch = new Guid[MatchCount];
			_uuidToNotMatch2 = new Uuid[MatchCount];
			_guidToNotMatch2 = new Guid[MatchCount];

			_uuids = new Dictionary<Uuid, bool>(TotalCount);
			_guids = new Dictionary<Guid, bool>(TotalCount);

			for (int i = 0; i < MatchCount; i++)
			{
				var guid = Guid.NewGuid();
				var uuid = Uuid.NewUuid();
				_guidToMatch[i] = guid;
				_uuidToMatch[i] = uuid;
				_guids.Add(guid, true);
				_uuids.Add(uuid, true);

				_uuidToNotMatch[i] = Uuid.NewUuid();
				_guidToNotMatch[i] = Guid.NewGuid();
				_uuidToNotMatch2[i] = Uuid.NewUuid();
				_guidToNotMatch2[i] = Guid.NewGuid();
			}

			for (int i = 0; i < TotalCount - MatchCount; i++)
			{
				_guids.Add(Guid.NewGuid(), false);
				_uuids.Add(Uuid.NewUuid(), false);
			}
		}

		[Benchmark(Baseline = true)]
		public void SearchGuid()
		{
			for (int i = 0; i < MatchCount; i++)
			{
				if (!_guids.ContainsKey(_guidToMatch[i])
				    || _guids.ContainsKey(_guidToNotMatch[i]))
				{
					throw new InvalidOperationException();
				}
			}
		}

		[Benchmark]
		public void MissGuid()
		{
			for (int i = 0; i < MatchCount; i++)
			{
				if (_guids.ContainsKey(_guidToNotMatch[i]) || _guids.ContainsKey(_guidToNotMatch2[i]))
				{
					throw new InvalidOperationException();
				}
			}
		}

		[Benchmark]
		public void SearchUuid()
		{
			for (int i = 0; i < MatchCount; i++)
			{
				if (!_uuids.ContainsKey(_uuidToMatch[i])
				    || _uuids.ContainsKey(_uuidToNotMatch[i]))
				{
					throw new InvalidOperationException();
				}
			}
		}

		[Benchmark]
		public void MissUuid()
		{
			for (int i = 0; i < MatchCount; i++)
			{
				if (_uuids.ContainsKey(_uuidToNotMatch[i]) || _uuids.ContainsKey(_uuidToNotMatch2[i]))
				{
					throw new InvalidOperationException();
				}
			}
		}
	}
}