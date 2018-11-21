namespace FastGuid
{
	/// <summary>
	/// Special struct to parse hex to byte. Contains high 4 bits in <see cref="High"/> and low 4 bits in <see cref="Low"/>.
	/// </summary>
	/// <remarks> <see cref="ushort"/> used because of overflow checking.</remarks>
	internal struct Bits
	{
		public ushort High;
		public ushort Low;

		public Bits(ushort low)
		{
			High = (ushort)(low * 16);
			Low = low;
		}

		public static explicit operator Bits(int value)
		{
			return new Bits { High = (ushort)value, Low = (ushort)value};
		}
	}
}