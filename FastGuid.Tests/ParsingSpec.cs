using System;
using NUnit.Framework;

namespace FastGuid.Tests
{
	[TestFixture]
	public class ParsingSpec
	{
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

		// TODO: write tests for invalid strings to parse
	}
}