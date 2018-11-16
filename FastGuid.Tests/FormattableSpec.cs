using System;
using NUnit.Framework;

namespace FastGuid.Tests
{
	[TestFixture]
	public class FormattableSpec
	{
		private const int IterationCount = 32;
		private Guid[] _originalGuids;
		private Uuid[] _uuids;

		[OneTimeSetUp]
		public void Init()
		{
			_originalGuids = new Guid[IterationCount];
			_uuids = new Uuid[IterationCount];

			// ensure correctness for Empty
			_originalGuids[0] = Guid.Empty;
			_uuids[0] = Uuid.Empty;

			for (int i = 1; i < _originalGuids.Length; i++)
			{
				_originalGuids[i] = Guid.NewGuid();
				_uuids[i] = (Uuid)_originalGuids[i];
			}
		}

		[Test]
		public void Default_ToString_results_should_be_the_same_for_Guid_and_Uuid()
		{
			for (int i = 0; i < IterationCount; i++)
			{
				// arrange
				var guidString = _originalGuids[i].ToString();

				// act
				var uuidString = _uuids[i].ToString();
				var uuidStringDefault = _uuids[i].ToStringDefault();

				// assert
				Assert.That(uuidString, Is.EqualTo(guidString));
				Assert.That(uuidStringDefault, Is.EqualTo(guidString));
			}
		}

		[Test]
		public void ToStringWithBraces_results_should_be_the_same_for_Guid_and_Uuid()
		{
			for (int i = 0; i < IterationCount; i++)
			{
				// arrange
				var guidStringB = _originalGuids[i].ToString("B");
				var guidStringP = _originalGuids[i].ToString("P");

				// act
				var uuidStringB = _uuids[i].ToString("B");
				var uuidStringB2 = _uuids[i].ToStringWithBraces();

				var uuidStringP = _uuids[i].ToString("P");
				var uuidStringP2 = _uuids[i].ToStringWithBraces('(', ')');

				// assert
				Assert.That(uuidStringB, Is.EqualTo(guidStringB));
				Assert.That(uuidStringB2, Is.EqualTo(guidStringB));

				Assert.That(uuidStringP, Is.EqualTo(guidStringP));
				Assert.That(uuidStringP2, Is.EqualTo(guidStringP));
			}
		}

		[Test]
		public void ToStringDigitsOnly_results_should_be_the_same_for_Guid_and_Uuid()
		{
			for (int i = 0; i < IterationCount; i++)
			{
				// arrange
				var guidString = _originalGuids[i].ToString("N");

				// act
				var uuidString = _uuids[i].ToString("N");
				var uuidString2 = _uuids[i].ToStringDigitsOnly();

				// assert
				Assert.That(uuidString, Is.EqualTo(guidString));
				Assert.That(uuidString2, Is.EqualTo(guidString));
			}
		}

		[Test]
		public void ToStringNested_results_should_be_the_same_for_Guid_and_Uuid()
		{
			for (int i = 0; i < IterationCount; i++)
			{
				// arrange
				var guidString = _originalGuids[i].ToString("X");

				// act
				var uuidString = _uuids[i].ToString("X");
				var uuidString2 = _uuids[i].ToStringNested();

				// assert
				Assert.That(uuidString, Is.EqualTo(guidString));
				Assert.That(uuidString2, Is.EqualTo(guidString));
			}
		}
	}
}