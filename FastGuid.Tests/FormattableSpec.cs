using NUnit.Framework;

namespace FastGuid.Tests
{
	[TestFixture]
	public class FormattableSpec
	{
		[Test]
		public void Default_ToString_results_should_be_the_same_for_Guid_and_Uuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString();

				// act + assert
				Assert.That(uuid.ToString(), Is.EqualTo(guidString));
				Assert.That(uuid.ToStringDefault(), Is.EqualTo(guidString));
			});
		}

		[Test]
		public void ToStringWithBraces_results_should_be_the_same_for_Guid_and_Uuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidStringB = guid.ToString("B");
				var guidStringP = guid.ToString("P");

				// act + assert
				Assert.That(uuid.ToString("B"), Is.EqualTo(guidStringB));
				Assert.That(uuid.ToStringWithBraces(), Is.EqualTo(guidStringB));

				Assert.That(uuid.ToString("P"), Is.EqualTo(guidStringP));
				Assert.That(uuid.ToStringWithBraces('(', ')'), Is.EqualTo(guidStringP));
			});
		}

		[Test]
		public void ToStringDigitsOnly_results_should_be_the_same_for_Guid_and_Uuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString("N");

				// act + assert
				Assert.That(uuid.ToString("N"), Is.EqualTo(guidString));
				Assert.That(uuid.ToStringDigitsOnly(), Is.EqualTo(guidString));
			});
		}

		[Test]
		public void ToStringNested_results_should_be_the_same_for_Guid_and_Uuid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString("X");

				// act + assert
				Assert.That(uuid.ToString("X"), Is.EqualTo(guidString));
				Assert.That(uuid.ToStringNested(), Is.EqualTo(guidString));
			});
		}
	}
}