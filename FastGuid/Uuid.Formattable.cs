using System;
using System.Runtime.InteropServices;

namespace FastGuid
{
	// Performance notes:
	// 1. No access to internal "string.FastAllocateString" (System.Guid use it), but char.MinValue
	// allow to skip extra code inside the "string(char, int)".
	// 2. No access to internal "string.GetRawStringData()" so just use "fixed (char* buffer = result)".
	// 3. "uint[]" operations faster than "char[]" or even "ValueTuple<char, char>[]" (benchmarks compared).
	//
	// Logic notes:
	// Bytes are in a different order than you might expect
	// For: 35 91 8b c9 - 19 6d - 40 ea  - 97 79  - 88 9d 79 b7 53 f0
	// Get: C9 8B 91 35   6D 19   EA 40    97 79    88 9D 79 B7 53 F0
	// Idx:  0  1  2  3    4  5    6  7     8  9    10 11 12 13 14 15
	public partial struct Uuid : IFormattable
	{
		private const int DigitsOnlyCharCount = 32;
		private const int DefaultCharCount = DigitsOnlyCharCount + 4;
		private const int BracesCharCount = DefaultCharCount + 2;
		private const int NestedCharCount = 68;

		[FieldOffset(0), NonSerialized] private readonly byte _byte00;
		[FieldOffset(1), NonSerialized] private readonly byte _byte01;
		[FieldOffset(2), NonSerialized] private readonly byte _byte02;
		[FieldOffset(3), NonSerialized] private readonly byte _byte03;

		[FieldOffset(4), NonSerialized] private readonly byte _byte04;
		[FieldOffset(5), NonSerialized] private readonly byte _byte05;

		[FieldOffset(6), NonSerialized] private readonly byte _byte06;
		[FieldOffset(7), NonSerialized] private readonly byte _byte07;

		[FieldOffset(8), NonSerialized] private readonly byte _byte08;
		[FieldOffset(9), NonSerialized] private readonly byte _byte09;

		[FieldOffset(10), NonSerialized] private readonly byte _byte10;
		[FieldOffset(11), NonSerialized] private readonly byte _byte11;
		[FieldOffset(12), NonSerialized] private readonly byte _byte12;
		[FieldOffset(13), NonSerialized] private readonly byte _byte13;
		[FieldOffset(14), NonSerialized] private readonly byte _byte14;
		[FieldOffset(15), NonSerialized] private readonly byte _byte15;

		public override string ToString()
		{
			return ToStringDefault();
		}

		public string ToString(string format, IFormatProvider formatProvider = null)
		{
			if (format == null || format.Length == 0)
				return ToStringDefault();

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
					return ToStringDefault();
				case 'N':
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
					return ToStringNested();
				default:
					throw new ArgumentOutOfRangeException(nameof(format));
			}
		}

		/// <summary>
		/// Converts to string with "D" format like "fd8bc35a-5c1f-42bc-b496-dac9f7621bcc".
		/// </summary>
		public unsafe string ToStringDefault()
		{
			unchecked
			{
				var result = new string(char.MinValue, DefaultCharCount);

				fixed (char* buffer = result)
				{
					uint* dwordBuffer = (uint*)buffer;

					// second buffer starts from 9th char because of hyphens
					uint* dwordBufferFrom9 = (uint*)(buffer + 9);

					fixed (uint* hexDwords = StaticData.HexDwords)
					{
						buffer[8] = '-';
						buffer[13] = '-';
						buffer[18] = '-';
						buffer[23] = '-';

						dwordBuffer[3] = hexDwords[_byte00];
						dwordBuffer[2] = hexDwords[_byte01];
						dwordBuffer[1] = hexDwords[_byte02];
						dwordBuffer[0] = hexDwords[_byte03];

						dwordBufferFrom9[1] = hexDwords[_byte04];
						dwordBufferFrom9[0] = hexDwords[_byte05];

						dwordBuffer[8] = hexDwords[_byte06];
						dwordBuffer[7] = hexDwords[_byte07];

						dwordBufferFrom9[5] = hexDwords[_byte08];
						dwordBufferFrom9[6] = hexDwords[_byte09];

						dwordBuffer[12] = hexDwords[_byte10];
						dwordBuffer[13] = hexDwords[_byte11];
						dwordBuffer[14] = hexDwords[_byte12];
						dwordBuffer[15] = hexDwords[_byte13];
						dwordBuffer[16] = hexDwords[_byte14];
						dwordBuffer[17] = hexDwords[_byte15];
					}

					return result;
				}
			}
		}

		/// <summary>
		/// Converts to string with "N" format like "fd8bc35a5c1f42bcb496dac9f7621bcc".
		/// </summary>
		public unsafe string ToStringDigitsOnly()
		{
			unchecked
			{
				var result = new string(char.MinValue, DigitsOnlyCharCount);

				fixed (char* buffer = result)
				{
					uint* dwordBuffer = (uint*)buffer;

					fixed (uint* hexDwords = StaticData.HexDwords)
					{
						dwordBuffer[3] = hexDwords[_byte00];
						dwordBuffer[2] = hexDwords[_byte01];
						dwordBuffer[1] = hexDwords[_byte02];
						dwordBuffer[0] = hexDwords[_byte03];

						dwordBuffer[5] = hexDwords[_byte04];
						dwordBuffer[4] = hexDwords[_byte05];

						dwordBuffer[7] = hexDwords[_byte06];
						dwordBuffer[6] = hexDwords[_byte07];

						dwordBuffer[8] = hexDwords[_byte08];
						dwordBuffer[9] = hexDwords[_byte09];

						dwordBuffer[10] = hexDwords[_byte10];
						dwordBuffer[11] = hexDwords[_byte11];
						dwordBuffer[12] = hexDwords[_byte12];
						dwordBuffer[13] = hexDwords[_byte13];
						dwordBuffer[14] = hexDwords[_byte14];
						dwordBuffer[15] = hexDwords[_byte15];
					}

					return result;
				}
			}
		}

		/// <summary>
		/// Converts to string with "B" format like "{fd8bc35a-5c1f-42bc-b496-dac9f7621bcc}"
		/// or "P" format like "(fd8bc35a-5c1f-42bc-b496-dac9f7621bcc)".
		/// </summary>
		public unsafe string ToStringWithBraces(char leftBrace = StaticData.LeftBrace, char rightBrace = StaticData.RightBrace)
		{
			unchecked
			{
				var result = new string(char.MinValue, BracesCharCount);

				fixed (char* buffer = result)
				{
					// started from 1th char because of braces
					uint* dwordBufferFrom1 = (uint*)(buffer + 1);

					// second buffer starts from 10th char because of hyphens and +1 for braces
					uint* dwordBufferFrom10 = (uint*)(buffer + 10);

					fixed (uint* hexDwords = StaticData.HexDwords)
					{
						buffer[0] = leftBrace;
						buffer[9] = '-';
						buffer[14] = '-';
						buffer[19] = '-';
						buffer[24] = '-';
						buffer[37] = rightBrace;

						dwordBufferFrom1[3] = hexDwords[_byte00];
						dwordBufferFrom1[2] = hexDwords[_byte01];
						dwordBufferFrom1[1] = hexDwords[_byte02];
						dwordBufferFrom1[0] = hexDwords[_byte03];

						dwordBufferFrom10[1] = hexDwords[_byte04];
						dwordBufferFrom10[0] = hexDwords[_byte05];

						dwordBufferFrom1[8] = hexDwords[_byte06];
						dwordBufferFrom1[7] = hexDwords[_byte07];

						dwordBufferFrom10[5] = hexDwords[_byte08];
						dwordBufferFrom10[6] = hexDwords[_byte09];

						dwordBufferFrom1[12] = hexDwords[_byte10];
						dwordBufferFrom1[13] = hexDwords[_byte11];
						dwordBufferFrom1[14] = hexDwords[_byte12];
						dwordBufferFrom1[15] = hexDwords[_byte13];
						dwordBufferFrom1[16] = hexDwords[_byte14];
						dwordBufferFrom1[17] = hexDwords[_byte15];
					}

					return result;
				}
			}
		}

		/// <summary>
		/// Converts to string with "X" format like "{0x418272ed,0x6d32,0x4775,{0x96,0x57,0x55,0xd7,0x73,0x9f,0xd6,0xf6}}"
		/// (this is for 418272ed-6d32-4775-9657-55d7739fd6f6).
		/// </summary>
		public unsafe string ToStringNested()
		{
			unchecked
			{
				var result = new string(char.MinValue, NestedCharCount);

				fixed (char* buffer = result)
				{
					uint* dwordBuffer = (uint*)(buffer);
					// started from 1th char because of braces and commas
					uint* dwordBufferFrom1 = (uint*)(buffer + 1);

					// First part of string and indexes for 418272ed-6d32-4775-9657-55d7739fd6f6:
					//  {  0  x  4  1  8  2  7  2  e  d  ,  0  x  6  d  3  2  ,  0  x  4  7  7  5  ,  {  0  x  9  6  ,
					// 00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31
					fixed (uint* hexDwords = StaticData.HexDwords)
					{
						buffer[0] = StaticData.LeftBrace;

						dwordBufferFrom1[0] = StaticData.HexPrefix;
						dwordBufferFrom1[4] = hexDwords[_byte00];
						dwordBufferFrom1[3] = hexDwords[_byte01];
						dwordBufferFrom1[2] = hexDwords[_byte02];
						dwordBufferFrom1[1] = hexDwords[_byte03];
						buffer[11] = StaticData.Comma;

						dwordBuffer[6] = StaticData.HexPrefix;
						dwordBuffer[8] = hexDwords[_byte04];
						dwordBuffer[7] = hexDwords[_byte05];
						buffer[18] = StaticData.Comma;

						dwordBufferFrom1[9] = StaticData.HexPrefix;
						dwordBufferFrom1[11] = hexDwords[_byte06];
						dwordBufferFrom1[10] = hexDwords[_byte07];
						buffer[25] = StaticData.Comma;

						buffer[26] = StaticData.LeftBrace;
						dwordBufferFrom1[13] = StaticData.HexPrefix;
						dwordBufferFrom1[14] = hexDwords[_byte08];
						buffer[31] = StaticData.Comma;

						// Second part of string and indexes for 418272ed-6d32-4775-9657-55d7739fd6f6:
						//  0  x  5  7  ,  0  x  5  5  ,  0  x  d  7  ,  0  x  7  3  ,  0  x  9  f  ,  0  x  d  6  ,  0  x  f  6  }  }
						// 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 66 67

						dwordBuffer[16] = StaticData.HexPrefix;
						dwordBuffer[17] = hexDwords[_byte09];
						buffer[36] = StaticData.Comma;

						dwordBufferFrom1[18] = StaticData.HexPrefix;
						dwordBufferFrom1[19] = hexDwords[_byte10];
						buffer[41] = StaticData.Comma;

						dwordBuffer[21] = StaticData.HexPrefix;
						dwordBuffer[22] = hexDwords[_byte11];
						buffer[46] = StaticData.Comma;

						dwordBufferFrom1[23] = StaticData.HexPrefix;
						dwordBufferFrom1[24] = hexDwords[_byte12];
						buffer[51] = StaticData.Comma;

						dwordBuffer[26] = StaticData.HexPrefix;
						dwordBuffer[27] = hexDwords[_byte13];
						buffer[56] = StaticData.Comma;

						dwordBufferFrom1[28] = StaticData.HexPrefix;
						dwordBufferFrom1[29] = hexDwords[_byte14];
						buffer[61] = StaticData.Comma;

						dwordBuffer[31] = StaticData.HexPrefix;
						dwordBuffer[32] = hexDwords[_byte15];
						buffer[66] = StaticData.RightBrace;
						buffer[67] = StaticData.RightBrace;
					}

					return result;
				}
			}
		}
	}
}