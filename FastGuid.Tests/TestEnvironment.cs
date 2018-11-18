using System;

namespace FastGuid.Tests
{
	/// <summary>
	///  Simplifies tests when equivalent (Guid,Uuid) pair should be used.
	/// </summary>
	public static class TestEnvironment
	{
		private const int IterationCount = 32;
		private static readonly Guid[] OriginalGuids;
		private static readonly Uuid[] EquivalentUuids;

		static TestEnvironment()
		{
			OriginalGuids = new Guid[IterationCount];
			EquivalentUuids = new Uuid[IterationCount];

			// ensure correctness for Empty
			OriginalGuids[0] = Guid.Empty;
			EquivalentUuids[0] = Uuid.Empty;

			for (int i = 1; i < OriginalGuids.Length; i++)
			{
				OriginalGuids[i] = Guid.NewGuid();
				EquivalentUuids[i] = (Uuid)OriginalGuids[i];
			}
		}

		/// <summary>
		/// Iterates <paramref name="pairDelegate"/> for all <see cref="OriginalGuids"/> and <see cref="EquivalentUuids"/> pairs.
		/// </summary>
		public static void Iterate(AssertPair pairDelegate)
		{
			if (pairDelegate == null)
				throw new ArgumentNullException(nameof(pairDelegate));

			for (int i = 0; i < IterationCount; i++)
			{
				pairDelegate(OriginalGuids[i], EquivalentUuids[i]);
			}
		}

		/// <summary>
		/// Iterates <paramref name="twoPairsDelegate"/> for all nearest pairs from <see cref="OriginalGuids"/> and <see cref="EquivalentUuids"/>.
		/// </summary>
		public static void IterateTwoPairs(AssertTwoPairs twoPairsDelegate)
		{
			if (twoPairsDelegate == null)
				throw new ArgumentNullException(nameof(twoPairsDelegate));

			for (int i = 0; i < IterationCount - 1; i++)
			{
				twoPairsDelegate(new Pair(OriginalGuids[i], EquivalentUuids[i]),
					new Pair(OriginalGuids[i + 1], EquivalentUuids[i + 1]));

				twoPairsDelegate(new Pair(OriginalGuids[i + 1], EquivalentUuids[i + 1]),
					new Pair(OriginalGuids[i], EquivalentUuids[i]));
			}
		}

		public delegate void AssertPair(Guid originalGuid, Uuid equivalentUuid);

		public delegate void AssertTwoPairs(Pair pair1, Pair pair2);

		public struct Pair
		{
			public Pair(Guid guid, Uuid uuid)
			{
				Guid = guid;
				Uuid = uuid;
			}

			public Guid Guid;
			public Uuid Uuid;
		}
	}
}