using System;
using NUnit.Framework;

namespace FastGuid.Tests
{
	[TestFixture]
	public class ParsingSpec
	{
		[Test]
		public void TryParseExact_difference_for_not_strict_format()
		{
			// arrange
			var guidStrings = new []
			{
				"00000000-0x00-0x12-0000-000000000000",
				"00000000-0x00-+123-0000-000000000000",
			};

			foreach (var guidString in guidStrings)
			{
				// act
				var guidResult = Guid.TryParseExact(guidString, "D", out _);
				var uuidResult = Uuid.TryParseExact(guidString, "D", out _);

				// assert
				Assert.That(guidResult, Is.True);
				Assert.That(uuidResult, Is.False);
			}
		}

		[Test]
		public void Default_TryParseExact_results_should_be_the_same_for_Guid_and_Uuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString();

				// act
				var guidResult = Guid.TryParseExact(guidString, "D", out var parsedGuid);
				var uuidResult = Uuid.TryParseExact(guidString, "D", out var parsedUuid);

				// assert
				Assert.That(uuidResult, Is.EqualTo(guidResult));
				Assert.That(parsedUuid, Is.EqualTo(uuid));
				Assert.That(parsedUuid.ToString(), Is.EqualTo(parsedGuid.ToString()));
			});
		}

		[Test]
		public void Default_TryParseExact_results_should_return_false_for_invalid_guids()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString();
				var guidString1 = guidString.Substring(1);

				// act + assert
				var guidResult1 = Guid.TryParseExact(guidString1, "D", out _);
				var uuidResult1 = Uuid.TryParseExact(guidString1, "D", out _);
				Assert.That(uuidResult1, Is.EqualTo(guidResult1));

				AssertWithReplacement(guidString, "D");
			});

			var guidResult3 = Guid.TryParseExact(string.Empty, "D", out _);
			var uuidResult3 = Uuid.TryParseExact(string.Empty, "D", out _);

			Assert.That(uuidResult3, Is.EqualTo(guidResult3));
		}

		[Test]
		public void TryParseExactWithBraces_results_should_be_the_same_for_Guid_and_Uuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString("B");

				// act
				var guidResult = Guid.TryParseExact(guidString, "B", out var parsedGuid);
				var uuidResult = Uuid.TryParseExact(guidString, "B", out var parsedUuid);

				// assert
				Assert.That(uuidResult, Is.EqualTo(guidResult));
				Assert.That(parsedUuid, Is.EqualTo(uuid));
				Assert.That(parsedUuid.ToString(), Is.EqualTo(parsedGuid.ToString()));
			});
		}

		[Test]
		public void TryParseExactWithBraces_results_should_return_false_for_invalid_guids()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString("B");
				var guidString1 = guidString.Substring(1);

				// act + assert
				var guidResult1 = Guid.TryParseExact(guidString1, "B", out _);
				var uuidResult1 = Uuid.TryParseExact(guidString1, "B", out _);
				Assert.That(uuidResult1, Is.EqualTo(guidResult1));

				AssertWithReplacement(guidString, "B");
			});

			var guidResult3 = Guid.TryParseExact(string.Empty, "B", out _);
			var uuidResult3 = Uuid.TryParseExact(string.Empty, "B", out _);

			Assert.That(uuidResult3, Is.EqualTo(guidResult3));
		}

		[Test]
		public void TryParseExactDigitsOnly_results_should_be_the_same_for_Guid_and_Uuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString("N");

				// act
				var guidResult = Guid.TryParseExact(guidString, "N", out var parsedGuid);
				var uuidResult = Uuid.TryParseExact(guidString, "N", out var parsedUuid);

				// assert
				Assert.That(uuidResult, Is.EqualTo(guidResult));
				Assert.That(parsedUuid, Is.EqualTo(uuid));
				Assert.That(parsedUuid.ToString(), Is.EqualTo(parsedGuid.ToString()));
			});
		}

		[Test]
		public void TryParseExactDigitsOnly_results_should_return_false_for_invalid_guids()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString("N");
				var guidString1 = guidString.Substring(1);

				// act + assert
				var guidResult1 = Guid.TryParseExact(guidString1, "N", out _);
				var uuidResult1 = Uuid.TryParseExact(guidString1, "N", out _);
				Assert.That(uuidResult1, Is.EqualTo(guidResult1));

				AssertWithReplacement(guidString, "N");
			});

			var guidResult3 = Guid.TryParseExact(string.Empty, "N", out _);
			var uuidResult3 = Uuid.TryParseExact(string.Empty, "N", out _);

			Assert.That(uuidResult3, Is.EqualTo(guidResult3));
		}

		[Test]
		public void TryParseExactNested_results_should_be_the_same_for_Guid_and_Uuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString("X");

				// act
				var guidResult = Guid.TryParseExact(guidString, "X", out var parsedGuid);
				var uuidResult = Uuid.TryParseExact(guidString, "X", out var parsedUuid);

				// assert
				Assert.That(uuidResult, Is.EqualTo(guidResult));
				Assert.That(parsedUuid, Is.EqualTo(uuid));
				Assert.That(parsedUuid.ToString(), Is.EqualTo(parsedGuid.ToString()));
			});
		}

		[Test]
		public void TryParseExactNested_results_should_return_false_for_invalid_guids()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString("X");
				var guidString1 = guidString.Substring(1);

				// act + assert
				var guidResult1 = Guid.TryParseExact(guidString1, "X", out _);
				var uuidResult1 = Uuid.TryParseExact(guidString1, "X", out _);

				Assert.That(uuidResult1, Is.EqualTo(guidResult1));

				AssertWithReplacement(guidString, "X");
			});

			var guidResult3 = Guid.TryParseExact(string.Empty, "X", out _);
			var uuidResult3 = Uuid.TryParseExact(string.Empty, "X", out _);

			Assert.That(uuidResult3, Is.EqualTo(guidResult3));
		}

		private static void AssertWithReplacement(string guidString, string format)
		{
			for (int i = 0; i < guidString.Length; i++)
			{
				var guidString2 = guidString.Substring(0, i) + "!" + guidString.Substring(i, guidString.Length - i - 1);

				var guidResult2 = Guid.TryParseExact(guidString2, format, out _);
				var uuidResult2 = Uuid.TryParseExact(guidString2, format, out _);

				Assert.That(uuidResult2, Is.EqualTo(guidResult2));
			}
		}
	}
}