using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastGuid.Temp
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SimpleGuid : IEquatable<SimpleGuid>
	{
		private const int DigitsOnlyCharCount = 32;
		private const int DefaultCharCount = DigitsOnlyCharCount + 4;
		private const int BracesCharCount = DefaultCharCount + 2;
		private const int NestedCharCount = 68;

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

		// [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

		public override string ToString()
		{
			// TODO: implement faster version
			var span = MemoryMarshal.Cast<SimpleGuid, Guid>(MemoryMarshal.CreateReadOnlySpan(ref this, 1));
			return span[0].ToString();
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

		public static bool TryParseExact(string input, string format, out SimpleGuid result)
		{
			if (input == null)
			{
				result = default;
				return false;
			}

			if (format == null || format.Length == 0)
				return TryParseDefault(input, out result);

			// all acceptable format strings are of length 1
			if (format.Length != 1)
			{
				throw new ArgumentException(
					$"Format length should be 1, but was {format.Length}.", nameof(format));
			}

			switch (format[0])
			{
				case 'D':
				case 'd':
					return TryParseDefault(input, out result);
				/*case 'N':
				case 'n':
					return TryParseDigitsOnly(input, out result);
				case 'B':
				case 'b':
					return TryParseWithBraces(input, out result);
				case 'P':
				case 'p':
					return TryParseWithBraces(input, out result, '(', ')');
				case 'X':
				case 'x':
					return TryParseNested(input, out result);*/
				default:
					throw new ArgumentOutOfRangeException(nameof(format));
			}
		}

		private static unsafe bool TryParseDefault(string input, out SimpleGuid result)
		{
			result = new SimpleGuid();
			if (input.Length != DefaultCharCount)
			{
				return false;
			}

			// 03020100-0504-0706-0809-101112131415 or +0x20100-+504-0X06-+0X9-101112131415
			fixed (char* buffer = input)
			{
				if (buffer[8] != '-' || buffer[13] != '-' || buffer[18] != '-' || buffer[23] != '-')
					return false;

				fixed (Bits* pBits = StaticData.BitsFromHex)
				{
					byte* bytes = stackalloc byte[4];

					if (!TryParseHexStrict(pBits, buffer[0], buffer[1], ref bytes[3]))
					{
						if (!TryParseNotStrictHex(buffer, 0, pBits, ref bytes[3], ref bytes[2]))
							return false;
					}
					else
					{
						if (!TryParseHexStrict(pBits, buffer[2], buffer[3], ref bytes[2]))
							return false;
					}

					if (!TryParseHexStrict(pBits, buffer[4], buffer[5], ref bytes[1])) return false;
					if (!TryParseHexStrict(pBits, buffer[6], buffer[7], ref bytes[0])) return false;

					// TODO: may be struct can be made or static method depending on BitConverter.IsLittleEndian to speed up converting to int
					// TODO: make faster (may be 4 byte variables instead of array?)
					// TODO: check Unsafe.ReadUnaligned<T> and Unsafe.WriteUnaligned<T> (or MemoryMarshal.As) for real Guid
					result._a = BitConverter.ToInt32(new ReadOnlySpan<byte>(bytes, 4));

					// - 8
					if (!TryParseHexStrict(pBits, buffer[9], buffer[10], ref bytes[3]))
					{
						if (!TryParseNotStrictHex(buffer, 9, pBits, ref bytes[3], ref bytes[2]))
							return false;
					}
					else
					{
						if (!TryParseHexStrict(pBits, buffer[11], buffer[12], ref bytes[2]))
							return false;
					}

					// - 13
					if (!TryParseHexStrict(pBits, buffer[14], buffer[15], ref bytes[1]))
					{
						if (!TryParseNotStrictHex(buffer, 14, pBits, ref bytes[1], ref bytes[0]))
							return false;
					}
					else
					{
						if (!TryParseHexStrict(pBits, buffer[16], buffer[17], ref bytes[0]))
							return false;
					}

					var span = new ReadOnlySpan<byte>(bytes, 4);
					result._b = BitConverter.ToInt16(span.Slice(2, 2));
					result._c = BitConverter.ToInt16(span.Slice(0, 2));
					// - 18
					if (!TryParseHexStrict(pBits, buffer[19], buffer[20], ref result._d))
					{
						if (!TryParseNotStrictHex(buffer, 19, pBits, ref result._d, ref result._e))
							return false;
					}
					else
					{
						if (!TryParseHexStrict(pBits, buffer[21], buffer[22], ref result._e))
							return false;
					}

					// - 23
					if (!TryParseHexStrict(pBits, buffer[24], buffer[25], ref result._f))
					{
						if (!TryParseNotStrictHex(buffer, 24, pBits, ref result._f, ref result._g))
							return false;
					}
					else
					{
						if (!TryParseHexStrict(pBits, buffer[26], buffer[27], ref result._g))
							return false;
					}

					if (!TryParseHexStrict(pBits, buffer[28], buffer[29], ref result._h)) return false;
					if (!TryParseHexStrict(pBits, buffer[30], buffer[31], ref result._i)) return false;
					if (!TryParseHexStrict(pBits, buffer[32], buffer[33], ref result._j)) return false;
					if (!TryParseHexStrict(pBits, buffer[34], buffer[35], ref result._k)) return false;
				}

				return true;
			}
		}

		private static unsafe bool TryParseNotStrictHex(char* buffer, int startIndex,
			Bits* charToHexLookup, ref byte currentByte, ref byte nextByte)
		{
			// TODO: move to Number in CoreCLR?
			currentByte = 0;
			char first = buffer[startIndex];
			if (first == '+')
			{
				if (IsHexPrefix(buffer, startIndex + 1))
				{
					if (!TryParseHexStrict(charToHexLookup, '0', buffer[startIndex + 3], ref nextByte))
						return false;
				}
				else
				{
					ushort secondChar = buffer[startIndex + 1];
					if (!TryParseHexStrict(charToHexLookup, '0', secondChar, ref currentByte))
						return false;

					if (!TryParseHexStrict(charToHexLookup, buffer[startIndex + 2], buffer[startIndex + 3], ref nextByte))
						return false;
				}

				return true;
			}

			if (!IsHexPrefix(buffer, startIndex))
				return false;

			return TryParseHexStrict(charToHexLookup, buffer[startIndex + 2], buffer[startIndex + 3], ref nextByte);
		}

		private static unsafe bool IsHexPrefix(char* str, int i) =>
			str[i] == '0' &&
			(str[i + 1] | 0x20) == 'x';

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe bool TryParseHexStrict(Bits* charToHexLookup, ushort a, ushort b, ref byte result)
		{
			const int maxLowBits = 15;
			const int maxHighBits = 15 << 4;
			unchecked
			{
				if (a >= StaticData.BitsFromHexLength || b >= StaticData.BitsFromHexLength)
					return false;

				a = charToHexLookup[a].High;
				b = charToHexLookup[b].Low;

				int value = a + b;

				// for 255 we need to distinguish Bits overflow (from 255 + 0) and normal 255 value (from 240 + 15)
				if (value >= byte.MaxValue)
				{
					if (value == byte.MaxValue && a == maxHighBits && b == maxLowBits)
					{
						result = byte.MaxValue;
						return true;
					}

					return false;
				}

				result = (byte)value;
				return true;
			}
		}

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe bool TryParseHex(Bits* charToHexLookup, int a, int b, ref byte result)
		{
			const int plusChar = (int)'+';
			const int maxLowBits = 15;
			const int maxHighBits = 15 << 4;
			unchecked
			{
				// "+", "0x" and "+0x" will not produce false result
				if (a >= StaticData.BitsFromHexLength)
					return false;

				if (b >= StaticData.BitsFromHexLength)
				{

				}
				else
				{
					b = charToHexLookup[b].Low;
				}

				a = charToHexLookup[a].High;

				int value = a + b;

				// for 255 we need to distinguish Bits overflow (from 255 + 0) and normal 255 value (from 240 + 15)
				if (value >= byte.MaxValue)
				{
					if (value == byte.MaxValue && a == maxHighBits && b == maxLowBits)
					{
						result = byte.MaxValue;
						return true;
					}

					return false;
				}

				result = (byte)value;
				return true;
			}
		}*/
	}
}