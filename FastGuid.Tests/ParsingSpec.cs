using System;
using FastGuid.Temp;
using NUnit.Framework;

namespace FastGuid.Tests
{
	[TestFixture]
	public class ParsingSpec
	{
		[Test]
		public void TryParseExactD_difference_for_not_strict_format()
		{
			// arrange
			string[] nonStrictPrefixes = { "+", "0x", "0X", "+0x", "+0X" };
			int[] blockIndexes = { 0, 9, 14, 19, 24 };

			var invalidStrings = new []
			{
				"00x000000-0000-0000-0000-000000000000",
				"00+000000-0000-0000-0000-000000000000",
			};

			foreach (var guidString in invalidStrings)
			{
				// act
				var guidResult = Guid.TryParseExact(guidString, "D", out _);

				var uuidResult = Uuid.TryParseExact(guidString, "D", out _);
				var simpleGuidResult = SimpleGuid.TryParseExact(guidString, "D", out _);

				// assert
				Assert.That(guidResult, Is.False);
				Assert.That(uuidResult, Is.False);
				Assert.That(simpleGuidResult, Is.False);
			}

			TestEnvironment.Iterate((guid, uuid) =>
			{
				foreach (var prefix in nonStrictPrefixes)
				{
					foreach (var blockIndex in blockIndexes)
					{
						var originString = guid.ToString();
						var guidString = originString.Remove(blockIndex, prefix.Length).Insert(blockIndex, prefix);

						// act
						var guidResult = Guid.TryParseExact(guidString, "D", out var parsedGuid);
						var uuidResult = Uuid.TryParseExact(guidString, "D", out _);
						var simpleGuidResult = SimpleGuid.TryParseExact(guidString, "D", out var simpleGuid);

						// assert
						Assert.That(guidResult, Is.True);
						Assert.That(uuidResult, Is.False);
						Assert.That(simpleGuidResult, Is.True);
						Assert.That(simpleGuid.ToString(), Is.EqualTo(parsedGuid.ToString()));
					}
				}

			});
		}

		[Test]
		public void TryParseExactX_false_for_0()
		{
			// arrange
			var guidStrings = new []
			{
				"{0X418272ed,0x6d32,0x4775,{0x96,0x57,0x55,0xd7,0x73,0x9f,0xd6,0xf6}}",
				"{0X418272ed,0X6d32,0X4775,{0X96,0X57,0X55,0Xd7,0X73,0X9f,0Xd6,0Xf6}}",
			};

			foreach (var guidString in guidStrings)
			{
				// act
				var guidResult = Guid.TryParseExact(guidString, "D", out _);
				var uuidResult = Uuid.TryParseExact(guidString, "D", out _);

				// assert
				Assert.That(guidResult, Is.False);
				Assert.That(uuidResult, Is.False);
			}
		}

		[Test]
		public void Default_TryParseExact_results_should_be_the_same_for_Guid_Uuid_and_SimpleGuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString();

				// act
				var guidResult = Guid.TryParseExact(guidString, "D", out var parsedGuid);
				var uuidResult = Uuid.TryParseExact(guidString, "D", out var parsedUuid);
				var simpleGuidResult = SimpleGuid.TryParseExact(guidString, "D", out var parsedSimpleGuid);

				// assert
				Assert.That(uuidResult, Is.EqualTo(guidResult));
				Assert.That(simpleGuidResult, Is.EqualTo(guidResult));
				Assert.That(parsedUuid, Is.EqualTo(uuid));
				Assert.That(parsedUuid.ToString(), Is.EqualTo(parsedGuid.ToString()));

				Assert.That(parsedSimpleGuid.ToString(), Is.EqualTo(parsedGuid.ToString()));
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
				var simpleGuidResult1 = SimpleGuid.TryParseExact(guidString1, "D", out _);
				Assert.That(uuidResult1, Is.EqualTo(guidResult1));
				Assert.That(simpleGuidResult1, Is.EqualTo(guidResult1));

				AssertWithReplacement(guidString, "D", true);
			});

			var guidResult3 = Guid.TryParseExact(string.Empty, "D", out _);
			var uuidResult3 = Uuid.TryParseExact(string.Empty, "D", out _);
			var simpleGuidResult3 = SimpleGuid.TryParseExact(string.Empty, "D", out _);

			Assert.That(uuidResult3, Is.EqualTo(guidResult3));
			Assert.That(simpleGuidResult3, Is.EqualTo(guidResult3));
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

		private static void AssertWithReplacement(string guidString, string format, bool checkSimpleGuid = false)
		{
			char[] invalidChars = { '!', 'Z', Char.MaxValue };

			foreach (var invalidChar in invalidChars)
			{
				for (int i = 0; i < guidString.Length; i++)
				{
					var guidString2 = guidString.Substring(0, i) + invalidChar + guidString.Substring(i, guidString.Length - i - 1);

					var guidResult2 = Guid.TryParseExact(guidString2, format, out _);
					var uuidResult2 = Uuid.TryParseExact(guidString2, format, out _);

					Assert.That(uuidResult2, Is.EqualTo(guidResult2));

					if (checkSimpleGuid)
					{
						var simpleGuidResult2 = SimpleGuid.TryParseExact(guidString2, format, out _);
						Assert.That(simpleGuidResult2, Is.EqualTo(guidResult2));
					}
				}
			}

		}
	}
}