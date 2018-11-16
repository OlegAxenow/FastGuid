using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastGuid
{
	public partial struct Uuid
	{
		[FieldOffset(0), NonSerialized]
		private readonly Guid _guid;

		// ReSharper disable FieldCanBeMadeReadOnly.Local
		[FieldOffset(0)] private int _a;   // Do not rename (binary serialization)
		[FieldOffset(4)] private short _b; // Do not rename (binary serialization)
		[FieldOffset(6)] private short _c; // Do not rename (binary serialization)
		[FieldOffset(8)] private byte _d;  // Do not rename (binary serialization)
		[FieldOffset(9)] private byte _e;  // Do not rename (binary serialization)
		[FieldOffset(10)] private byte _f;  // Do not rename (binary serialization)
		[FieldOffset(11)] private byte _g;  // Do not rename (binary serialization)
		[FieldOffset(12)] private byte _h;  // Do not rename (binary serialization)
		[FieldOffset(13)] private byte _i;  // Do not rename (binary serialization)
		[FieldOffset(14)] private byte _j;  // Do not rename (binary serialization)
		[FieldOffset(15)] private byte _k;  // Do not rename (binary serialization)
		// ReSharper restore FieldCanBeMadeReadOnly.Local

		public Uuid(Guid guid) : this()
		{
			_guid = guid;
		}

		public Uuid(ref Guid guid) : this()
		{
			_guid = guid;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Uuid(Guid guid)
		{
			return new Uuid(ref guid);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator Guid(Uuid uuid)
		{
			return uuid._guid;
		}
	}
}