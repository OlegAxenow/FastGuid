using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastGuid.Temp
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SimpleGuid : IEquatable<SimpleGuid>
	{
		// ReSharper disable FieldCanBeMadeReadOnly.Local
		private int _a; // Do not rename (binary serialization)
		private short _b; // Do not rename (binary serialization)
		private short _c; // Do not rename (binary serialization)
		private byte _d; // Do not rename (binary serialization)
		private byte _e; // Do not rename (binary serialization)
		private byte _f; // Do not rename (binary serialization)
		private byte _g; // Do not rename (binary serialization)
		private byte _h; // Do not rename (binary serialization)
		private byte _i; // Do not rename (binary serialization)
		private byte _j; // Do not rename (binary serialization)
		private byte _k; // Do not rename (binary serialization)
		// ReSharper restore FieldCanBeMadeReadOnly.Local

		public SimpleGuid(Guid guid) : this()
		{
			var span = MemoryMarshal.Cast<Guid, SimpleGuid>(MemoryMarshal.CreateReadOnlySpan(ref guid, 1));
			this = span[0];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(SimpleGuid other)
		{
			/*StructForEquals thisStruct = new StructForEquals(ref this);
			StructForEquals otherStruct = new StructForEquals(ref other);*/

			/*StructForEquals thisStruct = new StructForEquals();
			StructForEquals otherStruct = new StructForEquals();
			thisStruct._uuid = this;
			otherStruct._uuid = other;*/

			var spanThis = MemoryMarshal.Cast<SimpleGuid, StructForEquals>(MemoryMarshal.CreateReadOnlySpan(ref this, 1));
			var spanOther = MemoryMarshal.Cast<SimpleGuid, StructForEquals>(MemoryMarshal.CreateReadOnlySpan(ref other, 1));
			return spanThis[0]._second8Bytes == spanOther[0]._second8Bytes && spanThis[0]._first8Bytes == spanOther[0]._first8Bytes;
		}

		[StructLayout(LayoutKind.Explicit, Pack = 1)]
		private struct StructForEquals
		{
			[FieldOffset(0), NonSerialized]
			public ulong _first8Bytes;
			[FieldOffset(8), NonSerialized]
			public ulong _second8Bytes;

			[FieldOffset(0), NonSerialized]
			public SimpleGuid _simpleGuid;

			public StructForEquals(ref SimpleGuid simpleGuid) : this()
			{
				_simpleGuid = simpleGuid;
			}
		}
	}
}