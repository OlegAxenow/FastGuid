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
		/// Iterates <paramref name="iterationDelegate"/> for all <see cref="OriginalGuids"/> and <see cref="EquivalentUuids"/> pairs.
		/// </summary>
		public static void Iterate(AssertIteration iterationDelegate)
		{
			if (iterationDelegate == null)
				throw new ArgumentNullException(nameof(iterationDelegate));

			for (int i = 0; i < IterationCount; i++)
			{
				iterationDelegate(OriginalGuids[i], EquivalentUuids[i]);
			}
		}

		public delegate void AssertIteration(Guid originalGuid, Uuid equivalentUuid);
	}
}