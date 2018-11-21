using System;
using System.Runtime.CompilerServices;

namespace FastGuid
{
	// Performance notes:
	// 1. No access to internal "string.GetRawStringData()" so just use "fixed (char* buffer = result)".
	// 2. TODO: write other notes
	//
	// Logic notes:
	// Bytes are in a different order than you might expect
	// For: 35 91 8b c9 - 19 6d - 40 ea  - 97 79  - 88 9d 79 b7 53 f0
	// Get: C9 8B 91 35   6D 19   EA 40    97 79    88 9D 79 B7 53 F0
	// Idx:  0  1  2  3    4  5    6  7     8  9    10 11 12 13 14 15
	public partial struct Uuid
	{

		public static bool TryParseExact(string input, string format, out Uuid result)
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
					return ToStringDigitsOnly();
				case 'B':
				case 'b':
					return ToStringWithBraces();
				case 'P':
				case 'p':
					return ToStringWithBraces('(', ')');
				case 'X':
				case 'x':
					return ToStringNested();*/
				default:
					throw new ArgumentOutOfRangeException(nameof(format));
			}
		}

		private static unsafe bool TryParseDefault(string input, out Uuid result)
		{
			result = new Uuid();
			if (input.Length != DefaultCharCount)
			{
				return false;
			}

			fixed (char* buffer = input)
			{
				if (buffer[8] != '-' || buffer[13] != '-' || buffer[18] != '-' || buffer[23] != '-')
					return false;

				fixed (Bits* pBits = StaticData.BitsFromHex)
				{
					if (!TryParseHex(pBits, buffer[0], buffer[1], ref result._byte03)) return false;
					if (!TryParseHex(pBits, buffer[2], buffer[3], ref result._byte02)) return false;
					if (!TryParseHex(pBits, buffer[4], buffer[5], ref result._byte01)) return false;
					if (!TryParseHex(pBits, buffer[6], buffer[7], ref result._byte00)) return false;
					// - 8
					if (!TryParseHex(pBits, buffer[9], buffer[10], ref result._byte05)) return false;
					if (!TryParseHex(pBits, buffer[11], buffer[12], ref result._byte04)) return false;
					// - 13
					if (!TryParseHex(pBits, buffer[14], buffer[15], ref result._byte07)) return false;
					if (!TryParseHex(pBits, buffer[16], buffer[17], ref result._byte06)) return false;
					// - 18
					if (!TryParseHex(pBits, buffer[19], buffer[20], ref result._byte08)) return false;
					if (!TryParseHex(pBits, buffer[21], buffer[22], ref result._byte09)) return false;
					// - 23
					if (!TryParseHex(pBits, buffer[24], buffer[25], ref result._byte10)) return false;
					if (!TryParseHex(pBits, buffer[26], buffer[27], ref result._byte11)) return false;
					if (!TryParseHex(pBits, buffer[28], buffer[29], ref result._byte12)) return false;
					if (!TryParseHex(pBits, buffer[30], buffer[31], ref result._byte13)) return false;
					if (!TryParseHex(pBits, buffer[32], buffer[33], ref result._byte14)) return false;
					if (!TryParseHex(pBits, buffer[34], buffer[35], ref result._byte15)) return false;
				}

				return true;
			}
		}

		/// <summary>
		/// Method from Oleg
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static unsafe bool TryParseHex(Bits* pBits, ushort a, ushort b, ref byte result)
		{
			unchecked
			{
				if ((a | b) > 256) return false;

				a = pBits[a].High;
				b = pBits[b].Low;

				int value = a + b;
				if (value >= 256) return false;

				result = (byte)value;
				return true;
			}
		}
	}
}