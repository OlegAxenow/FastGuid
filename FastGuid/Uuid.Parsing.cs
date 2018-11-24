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
				case 'N':
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
					return TryParseNested(input, out result);
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

		private static unsafe bool TryParseWithBraces(string input, out Uuid result,
			char leftBrace = StaticData.LeftBrace, char rightBrace = StaticData.RightBrace)
		{
			result = new Uuid();
			if (input.Length != BracesCharCount)
			{
				return false;
			}

			fixed (char* buffer = input)
			{
				if (buffer[0] != leftBrace || buffer[BracesCharCount-1] != rightBrace ||
					buffer[9] != '-' || buffer[14] != '-' || buffer[19] != '-' || buffer[24] != '-')
					return false;

				fixed (Bits* pBits = StaticData.BitsFromHex)
				{
					if (!TryParseHex(pBits, buffer[1], buffer[2], ref result._byte03)) return false;
					if (!TryParseHex(pBits, buffer[3], buffer[4], ref result._byte02)) return false;
					if (!TryParseHex(pBits, buffer[5], buffer[6], ref result._byte01)) return false;
					if (!TryParseHex(pBits, buffer[7], buffer[8], ref result._byte00)) return false;
					// - 9
					if (!TryParseHex(pBits, buffer[10], buffer[11], ref result._byte05)) return false;
					if (!TryParseHex(pBits, buffer[12], buffer[13], ref result._byte04)) return false;
					// - 14
					if (!TryParseHex(pBits, buffer[15], buffer[16], ref result._byte07)) return false;
					if (!TryParseHex(pBits, buffer[17], buffer[18], ref result._byte06)) return false;
					// - 19
					if (!TryParseHex(pBits, buffer[20], buffer[21], ref result._byte08)) return false;
					if (!TryParseHex(pBits, buffer[22], buffer[23], ref result._byte09)) return false;
					// - 24
					if (!TryParseHex(pBits, buffer[25], buffer[26], ref result._byte10)) return false;
					if (!TryParseHex(pBits, buffer[27], buffer[28], ref result._byte11)) return false;
					if (!TryParseHex(pBits, buffer[29], buffer[30], ref result._byte12)) return false;
					if (!TryParseHex(pBits, buffer[31], buffer[32], ref result._byte13)) return false;
					if (!TryParseHex(pBits, buffer[33], buffer[34], ref result._byte14)) return false;
					if (!TryParseHex(pBits, buffer[35], buffer[36], ref result._byte15)) return false;
				}

				return true;
			}
		}

		private static unsafe bool TryParseDigitsOnly(string input, out Uuid result)
		{
			result = new Uuid();
			if (input.Length != DigitsOnlyCharCount)
			{
				return false;
			}

			fixed (char* buffer = input)
			{
				fixed (Bits* pBits = StaticData.BitsFromHex)
				{
					if (!TryParseHex(pBits, buffer[0], buffer[1], ref result._byte03)) return false;
					if (!TryParseHex(pBits, buffer[2], buffer[3], ref result._byte02)) return false;
					if (!TryParseHex(pBits, buffer[4], buffer[5], ref result._byte01)) return false;
					if (!TryParseHex(pBits, buffer[6], buffer[7], ref result._byte00)) return false;
					// -
					if (!TryParseHex(pBits, buffer[8], buffer[9], ref result._byte05)) return false;
					if (!TryParseHex(pBits, buffer[10], buffer[11], ref result._byte04)) return false;
					// -
					if (!TryParseHex(pBits, buffer[12], buffer[13], ref result._byte07)) return false;
					if (!TryParseHex(pBits, buffer[14], buffer[15], ref result._byte06)) return false;
					// -
					if (!TryParseHex(pBits, buffer[16], buffer[17], ref result._byte08)) return false;
					if (!TryParseHex(pBits, buffer[18], buffer[19], ref result._byte09)) return false;
					// -
					if (!TryParseHex(pBits, buffer[20], buffer[21], ref result._byte10)) return false;
					if (!TryParseHex(pBits, buffer[22], buffer[23], ref result._byte11)) return false;
					if (!TryParseHex(pBits, buffer[24], buffer[25], ref result._byte12)) return false;
					if (!TryParseHex(pBits, buffer[26], buffer[27], ref result._byte13)) return false;
					if (!TryParseHex(pBits, buffer[28], buffer[29], ref result._byte14)) return false;
					if (!TryParseHex(pBits, buffer[30], buffer[31], ref result._byte15)) return false;
				}

				return true;
			}
		}

		private static unsafe bool TryParseNested(string input, out Uuid result)
		{
			result = new Uuid();
			if (input.Length != NestedCharCount)
			{
				return false;
			}

			fixed (char* buffer = input)
			{
				uint* dwordBuffer = (uint*)(buffer);
				// started from 1th char because of braces and commas
				uint* dwordBufferFrom1 = (uint*)(buffer + 1);

				// First part of string and indexes for 418272ed-6d32-4775-9657-55d7739fd6f6:
				//  {  0  x  4  1  8  2  7  2  e  d  ,  0  x  6  d  3  2  ,  0  x  4  7  7  5  ,  {  0  x  9  6  ,
				// 00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31
				if (buffer[0] != StaticData.LeftBrace || buffer[26] != StaticData.LeftBrace ||
				    buffer[66] != StaticData.RightBrace || buffer[67] != StaticData.RightBrace ||
				    buffer[11] != StaticData.Comma || buffer[18] != StaticData.Comma ||
				    buffer[25] != StaticData.Comma || buffer[31] != StaticData.Comma ||
				    buffer[36] != StaticData.Comma || buffer[41] != StaticData.Comma ||
				    buffer[46] != StaticData.Comma || buffer[51] != StaticData.Comma ||
				    buffer[56] != StaticData.Comma || buffer[61] != StaticData.Comma ||
				    dwordBufferFrom1[9] != StaticData.HexPrefix || dwordBufferFrom1[13] != StaticData.HexPrefix ||
				    dwordBuffer[16] != StaticData.HexPrefix || dwordBufferFrom1[18] != StaticData.HexPrefix ||
				    dwordBuffer[21] != StaticData.HexPrefix || dwordBufferFrom1[23] != StaticData.HexPrefix ||
				    dwordBuffer[26] != StaticData.HexPrefix || dwordBufferFrom1[28] != StaticData.HexPrefix ||
				    dwordBuffer[31] != StaticData.HexPrefix)
					return false;

				fixed (Bits* pBits = StaticData.BitsFromHex)
				{
					if (!TryParseHex(pBits, buffer[3], buffer[4], ref result._byte03)) return false;
					if (!TryParseHex(pBits, buffer[5], buffer[6], ref result._byte02)) return false;
					if (!TryParseHex(pBits, buffer[7], buffer[8], ref result._byte01)) return false;
					if (!TryParseHex(pBits, buffer[9], buffer[10], ref result._byte00)) return false;
					// - 9
					if (!TryParseHex(pBits, buffer[14], buffer[15], ref result._byte05)) return false;
					if (!TryParseHex(pBits, buffer[16], buffer[17], ref result._byte04)) return false;
					// - 14
					if (!TryParseHex(pBits, buffer[21], buffer[22], ref result._byte07)) return false;
					if (!TryParseHex(pBits, buffer[23], buffer[24], ref result._byte06)) return false;
					if (!TryParseHex(pBits, buffer[29], buffer[30], ref result._byte08)) return false;

					// Second part of string and indexes for 418272ed-6d32-4775-9657-55d7739fd6f6:
					//  0  x  5  7  ,  0  x  5  5  ,  0  x  d  7  ,  0  x  7  3  ,  0  x  9  f  ,  0  x  d  6  ,  0  x  f  6  }  }
					// 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 66 67
					if (!TryParseHex(pBits, buffer[34], buffer[35], ref result._byte09)) return false;
					if (!TryParseHex(pBits, buffer[39], buffer[40], ref result._byte10)) return false;
					if (!TryParseHex(pBits, buffer[44], buffer[45], ref result._byte11)) return false;
					if (!TryParseHex(pBits, buffer[49], buffer[50], ref result._byte12)) return false;
					if (!TryParseHex(pBits, buffer[54], buffer[55], ref result._byte13)) return false;
					if (!TryParseHex(pBits, buffer[59], buffer[60], ref result._byte14)) return false;
					if (!TryParseHex(pBits, buffer[64], buffer[65], ref result._byte15)) return false;
				}

				return true;
			}
		}

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