using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastGuid
{
	/// <summary>
	/// Special struct to parse hex to byte. Contains high 4 bits in <see cref="High"/> and low 4 bits in <see cref="Low"/>.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	internal struct Bits
	{
		public byte High;
		public byte Low;

		public Bits(byte low)
		{
			High = (byte)(low << 4);
			Low = low;
		}

		public static explicit operator Bits(byte value)
		{
			return new Bits { High = value, Low = value};
		}
	}
}