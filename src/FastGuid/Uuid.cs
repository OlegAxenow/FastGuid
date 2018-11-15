using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastGuid
{
	[Serializable]
	// [StructLayout(LayoutKind.Explicit, Pack = 1)]
	[StructLayout(LayoutKind.Sequential)]
	[DebuggerDisplay("{" + nameof(_first8Bytes) + "}-{" + nameof(_second8Bytes) + "}")]
	public partial struct Uuid : IComparable, IComparable<Uuid>, IEquatable<Uuid>
	{
		public static readonly Uuid Empty;

		// [FieldOffset(0)]
		private ulong _first8Bytes;
		// [FieldOffset(8)]
		private ulong _second8Bytes;

		// [FieldOffset(0)] private int _firstInt;

		public unsafe Uuid(Guid guid)
		{
			/*fixed (Uuid* pThis = &this)
			{
				// This skips the C# definite assignment rule that all fields of the struct must be assigned before the constructor exits.
			}*/
			// TODO: compare performance with "this = *(Uuid*)&guid;" again; try to use Span<T>.CopyTo or ref Guid?
			// this = *(Uuid*)&guid;
			var pointer = (ulong*)&guid;
			// _firstInt = 0;
			_first8Bytes = pointer[0];
			_second8Bytes = pointer[1];
		}

		public Uuid(ulong first8Bytes, ulong second8Bytes)
		{
			_first8Bytes = first8Bytes;
			_second8Bytes = second8Bytes;
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
			if (_first8Bytes < other._first8Bytes)
				return -1;

			if (_first8Bytes > other._first8Bytes)
				return 1;

			if (other._second8Bytes != _second8Bytes)
				return _second8Bytes < other._second8Bytes ? -1 : 1;

			return 0;
		}

		public int CompareTo(object value)
		{
			if (value == null)
				return 1;
			if (!(value is Uuid))
				throw new ArgumentException($"Argument must be Uuid", nameof(value));

			var other = (Uuid)value;

			if (_first8Bytes < other._first8Bytes)
				return -1;

			if (_first8Bytes > other._first8Bytes)
				return 1;

			if (other._second8Bytes != _second8Bytes)
				return _second8Bytes < other._second8Bytes ? -1 : 1;

			return 0;
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