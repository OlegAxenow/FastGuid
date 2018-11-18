using System;
using NUnit.Framework;

namespace FastGuid.Tests
{
	[TestFixture]
	public class ConstructorsAndConversionSpec
	{
		[Test]
		public void Uuid_constructors_should_correctly_accepts_Guid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString();

				// act + assert
				Assert.That(new Uuid(guid).ToString(), Is.EqualTo(guidString));
				Assert.That(new Uuid(ref guid).ToString(), Is.EqualTo(guidString));
			});
		}

		[Test]
		public void Uuid_constructor_should_correctly_accepts_byte_array()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString();
				var guidBytes = guid.ToByteArray();

				// act + assert
				Assert.That(new Uuid(guidBytes).ToString(), Is.EqualTo(guidString));
			});
		}

		[Test]
		public void Uuid_constructor_should_correctly_accepts_span_of_bytes()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString();
				var guidSpan = new ReadOnlySpan<byte>(guid.ToByteArray());

				// act + assert
				Assert.That(new Uuid(guidSpan).ToString(), Is.EqualTo(guidString));
			});
		}

		[Test]
		public void Uuid_explicit_operators_should_correctly_converts_to_and_from_Guid()
		{
			TestEnvironment.Iterate((guid, uuid) =>
			{
				// arrange
				var guidString = guid.ToString();

				// act + assert
				Assert.That(((Uuid)guid).ToString(), Is.EqualTo(guidString));
				Assert.That(((Guid)((Uuid)guid)).ToString(), Is.EqualTo(guidString));
			});
		}
	}
}