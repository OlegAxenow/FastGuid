using System;
using NUnit.Framework;

namespace FastGuid.Tests
{
	[TestFixture]
	public class ImmutabilitySpec
	{
		[Test]
		public void Changed_bytes_should_not_affect_uuid()
		{
			// arrange
			var bytes = Guid.NewGuid().ToByteArray();

			// act
			var uuid = new Uuid(bytes);
			var snapshot = uuid.ToString();
			bytes[0] = 0;

			// assert
			Assert.That(uuid.ToString(), Is.EqualTo(snapshot));
		}
	}
}