﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastGuid
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	[DebuggerDisplay("{" + nameof(ToString) + "()}")]
	public partial struct Uuid : IComparable, IComparable<Uuid>, IEquatable<Uuid>
	{
		public static readonly Uuid Empty;

		[FieldOffset(0), NonSerialized]
		private ulong _first8Bytes;
		[FieldOffset(8), NonSerialized]
		private ulong _second8Bytes;

		// fields for comparison
		[FieldOffset(0), NonSerialized] private uint _aUnsigned;
		[FieldOffset(4), NonSerialized] private ushort _bUnsignded;
		[FieldOffset(6), NonSerialized] private ushort _cUnsigned;

		public unsafe Uuid(byte[] bytes)
		{
			if (bytes.Length != 16)
				throw new ArgumentException($"Uuid constructor accepts only 16 bytes, but was {bytes.Length}", nameof(bytes));
			fixed (byte* p = bytes)
			{
				this = *(Uuid*)p;
			}
		}

		public Uuid(ReadOnlySpan<byte> bytes) : this()
		{
			if (bytes.Length != 16)
				throw new ArgumentException($"Uuid constructor accepts only 16 bytes, but was {bytes.Length}", nameof(bytes));
			// TODO: try to use something else like in ctor with byte[] arg

			_byte00 = bytes[0];
			_byte01 = bytes[1];
			_byte02 = bytes[2];
			_byte03 = bytes[3];
			_byte04 = bytes[4];
			_byte05 = bytes[5];
			_byte06 = bytes[6];
			_byte07 = bytes[7];
			_byte08 = bytes[8];
			_byte09 = bytes[9];
			_byte10 = bytes[10];
			_byte11 = bytes[11];
			_byte12 = bytes[12];
			_byte13 = bytes[13];
			_byte14 = bytes[14];
			_byte15 = bytes[15];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Uuid other)
		{
			return _second8Bytes == other._second8Bytes && _first8Bytes == other._first8Bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			// if (obj == null || !(obj is Uuid)) return false;
			if (!(obj is Uuid)) return false;
			Uuid other = (Uuid)obj;
			return _second8Bytes == other._second8Bytes && _first8Bytes == other._first8Bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(Uuid other)
		{
			if (_aUnsigned < other._aUnsigned)
				return -1;

			if (_aUnsigned > other._aUnsigned)
				return 1;

			if (_bUnsignded < other._bUnsignded)
				return -1;

			if (_bUnsignded > other._bUnsignded)
				return 1;

			if (_cUnsigned < other._cUnsigned)
				return -1;

			if (_cUnsigned > other._cUnsigned)
				return 1;

			if (other._second8Bytes == _second8Bytes)
				return 0;

			return CompareToLast8Bytes(ref other);
		}

		private int CompareToLast8Bytes(ref Uuid other)
		{
			if (_d != other._d)
				return _d < other._d ? -1 : 1;
			if (_e != other._e)
				return _e < other._e ? -1 : 1;
			if (_f != other._f)
				return _f < other._f ? -1 : 1;
			if (_g != other._g)
				return _g < other._g ? -1 : 1;

			if (_h != other._h)
				return _h < other._h ? -1 : 1;
			if (_i != other._i)
				return _i < other._i ? -1 : 1;
			if (_j != other._j)
				return _j < other._j ? -1 : 1;
			if (_k != other._k)
				return _k < other._k ? -1 : 1;
			Debug.Assert(false, "should not be here because of 'if (other._second8Bytes == _second8Bytes) return 0;'");
			return 0;
		}

		public int CompareTo(object value)
		{
			if (value == null)
				return 1;
			if (!(value is Uuid))
				throw new ArgumentException($"Argument must be Uuid", nameof(value));

			var other = (Uuid)value;

			if (_aUnsigned < other._aUnsigned)
				return -1;

			if (_aUnsigned > other._aUnsigned)
				return 1;

			if (_bUnsignded < other._bUnsignded)
				return -1;

			if (_bUnsignded > other._bUnsignded)
				return 1;

			if (_cUnsigned < other._cUnsigned)
				return -1;

			if (_cUnsigned > other._cUnsigned)
				return 1;

			if (other._second8Bytes == _second8Bytes)
				return 0;

			return CompareToLast8Bytes(ref other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (int)_second8Bytes ^ (int)_first8Bytes;
				// return (int)_first ^ (int)_second ^ (int)(_first>>32) ^ (int)(_second>>32);
			}
		}

		public static bool operator ==(Uuid left, Uuid right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Uuid left, Uuid right)
		{
			return !Equals(left, right);
		}
	}
}