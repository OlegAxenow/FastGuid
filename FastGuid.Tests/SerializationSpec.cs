using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace FastGuid.Tests
{
	[TestFixture]
	public class SerializationSpec
	{
		[Test]
		public void Binary_serialization_results_should_be_the_same_for_Guid_and_Uuid()
		{
			// arrange
			var formatter = new BinaryFormatter { Binder = new CustomBinder() };

			TestEnvironment.Iterate((guid, uuid) =>
			{
				var guidArray = new byte[128];
				var uuidArray = new byte[128];
				var differentBytesCount = 0;
				byte guidByte = 0;
				byte uuidByte = 0;

				// act
				using(var guidStream = new MemoryStream(guidArray))
				{
					formatter.Serialize(guidStream, guid);
				}

				using (var uuidStream = new MemoryStream(uuidArray))
				{
					formatter.Serialize(uuidStream, uuid);
				}

				// assert

				for (int i = 0; i < guidArray.Length; i++)
				{
					var b = guidArray[i];

					if (b != uuidArray[i])
					{
						differentBytesCount++;
						guidByte = b;
						uuidByte = uuidArray[i];
					}
				}

				Assert.That(differentBytesCount, Is.EqualTo(1));
				Assert.That(uuidByte - guidByte, Is.EqualTo(1));
			});
		}

		private class CustomBinder : SerializationBinder
		{
			public virtual StreamingContext Context { get; set; }

			public virtual Type BindToType (string typeName) => Type.GetType (typeName);

			public sealed override Type BindToType (string assemblyName, string typeName) => BindToType (typeName);

			public sealed override void BindToName (Type serializedType, out string assemblyName, out string typeName)
			{
				if (serializedType.Name == "Guid" || serializedType.Name == "Uuid")
				{
					typeName = serializedType.Name == "Guid" ? "0" : "1";
					assemblyName = "TestAssembly";
				}
				else
				{
					base.BindToName(serializedType, out assemblyName, out typeName);
				}
			}
		}
	}
}