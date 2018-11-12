using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastGuid
{
	[Serializable]
	// [StructLayout(LayoutKind.Explicit)]
	[StructLayout(LayoutKind.Sequential)]
	[DebuggerDisplay("{" + nameof(_first) + "}-{" + nameof(_second) + "}")]
	public partial struct Uuid : IComparable, IComparable<Uuid>, IEquatable<Uuid>
	{
		public static readonly Uuid Empty;

		// [FieldOffset(0)]
		private ulong _first;
		// [FieldOffset(8)]
		private ulong _second;

		// [FieldOffset(0)] private int _firstInt;

		public unsafe Uuid(Guid guid)
		{
			// TODO: try to use Span<T>.CopyTo?
			// this = *(Uuid*)&guid;
			var pointer = (ulong*)&guid;
			// _firstInt = 0;
			_first = pointer[0];
			_second = pointer[1];
		}

		public Uuid(ulong first, ulong second)
		{
			_first = first;
			_second = second;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe implicit operator Uuid(Guid guid)
		{
			// return *(Uuid*)&guid;
			var pointer = (ulong*)&guid;
			return new Uuid(pointer[0], pointer[1]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe implicit operator Guid(Uuid uuid)
		{
			return *(Guid*)&uuid;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Uuid other)
		{
			return _second == other._second && _first == other._first;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			// if (obj == null || !(obj is Uuid)) return false;
			if (!(obj is Uuid)) return false;
			Uuid other = (Uuid)obj;
			return _second == other._second && _first == other._first;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(Uuid other)
		{
			if (_first < other._first)
				return -1;

			if (_first > other._first)
				return 1;

			if (other._second != _second)
				return _second < other._second ? -1 : 1;

			return 0;
		}

		public int CompareTo(object value)
		{
			if (value == null)
				return 1;
			if (!(value is Uuid))
				throw new ArgumentException($"Argument must be Uuid", nameof(value));

			var other = (Uuid)value;

			if (_first < other._first)
				return -1;

			if (_first > other._first)
				return 1;

			if (other._second != _second)
				return _second < other._second ? -1 : 1;

			return 0;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (int)_second ^ (int)_first;
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