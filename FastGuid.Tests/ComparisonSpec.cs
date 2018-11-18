using System;
using NUnit.Framework;

namespace FastGuid.Tests
{
	[TestFixture]
	public class ComparisonSpec
	{
		[Test]
		public void Equality_operators_should_produce_same_results_for_Guid_and_Uuid()
		{
			TestEnvironment.IterateTwoPairs((pair1, pair2) =>
			{
				// act + assert
				Assert.That(pair1.Uuid == pair1.Uuid, Is.EqualTo(pair1.Guid == pair1.Guid));
				Assert.That(pair1.Uuid == pair2.Uuid, Is.EqualTo(pair1.Guid == pair2.Guid));

				Assert.That(pair1.Uuid != pair1.Uuid, Is.EqualTo(pair1.Guid != pair1.Guid));
				Assert.That(pair1.Uuid != pair2.Uuid, Is.EqualTo(pair1.Guid != pair2.Guid));
			});
		}

		[Test]
		public void Equals_should_produce_same_results_for_Guid_and_Uuid()
		{
			TestEnvironment.IterateTwoPairs((pair1, pair2) =>
			{
				// act + assert
				Assert.That(pair1.Uuid.Equals(pair1.Uuid), Is.EqualTo(pair1.Guid.Equals(pair1.Guid)));
				Assert.That(pair1.Uuid.Equals(pair2.Uuid), Is.EqualTo(pair1.Guid.Equals(pair2.Guid)));

			});
		}

		[Test]
		public void Equals_with_wrong_object_should_produce_same_results_for_Guid_and_Uuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// act + assert
				Assert.That(uuid.Equals(1), Is.EqualTo(guid.Equals(1)));
				Assert.That(uuid.Equals(null), Is.EqualTo(guid.Equals(null)));
			});
		}

		[Test]
		public void Equals_with_object_should_produce_same_results_for_Guid_and_Uuid()
		{
			TestEnvironment.IterateTwoPairs((pair1, pair2) =>
			{
				// act + assert
				Assert.That(pair1.Uuid.Equals((object)pair1.Uuid), Is.EqualTo(pair1.Guid.Equals((object)pair1.Guid)));
				Assert.That(pair1.Uuid.Equals((object)pair2.Uuid), Is.EqualTo(pair1.Guid.Equals((object)pair2.Guid)));

			});
		}

		[Test]
		public void Compare_should_produce_same_results_for_Guid_and_Uuid()
		{
			// arrange

			// special case for small difference in last bytes (07...8b and 08..8a)
			var guid1 = Guid.ParseExact("00000000-0000-0000-0000-010000000002", "d");
			var guid2 = Guid.ParseExact("00000000-0000-0000-0000-020000000001", "d");

			var uuid1 = new Uuid(guid1);
			var uuid2 = new Uuid(guid2);

			// act + assert

			Assert.That(uuid1.CompareTo(uuid2), Is.EqualTo(guid1.CompareTo(guid2)));
			Assert.That(uuid2.CompareTo(uuid1), Is.EqualTo(guid2.CompareTo(guid1)));

			TestEnvironment.IterateTwoPairs((pair1, pair2) =>
			{
				Assert.That(pair1.Uuid.CompareTo(pair1.Uuid), Is.EqualTo(pair1.Guid.CompareTo(pair1.Guid)));
				var actual = pair1.Uuid.CompareTo(pair2.Uuid);
				var expected = pair1.Guid.CompareTo(pair2.Guid);
				Assert.That(actual, Is.EqualTo(expected));
			});
		}

		[Test]
		public void Compare_with_wrong_object_should_produce_same_results_for_Guid_and_Uuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// act + assert
				Assert.That(uuid.CompareTo(null), Is.EqualTo(guid.CompareTo(null)));

				Assert.Throws<ArgumentException>(() => guid.CompareTo(1));
				Assert.Throws<ArgumentException>(() => uuid.CompareTo(1));
			});
		}

		[Test]
		public void Compare_with_object_should_produce_same_results_for_Guid_and_Uuid()
		{
			TestEnvironment.IterateTwoPairs((pair1, pair2) =>
			{
				// act + assert
				Assert.That(pair1.Uuid.CompareTo((object)pair1.Uuid), Is.EqualTo(pair1.Guid.CompareTo((object)pair1.Guid)));
				Assert.That(pair1.Uuid.CompareTo((object)pair2.Uuid), Is.EqualTo(pair1.Guid.CompareTo((object)pair2.Guid)));
			});
		}
	}
}