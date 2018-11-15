#if UNIX
using System;
using System.Runtime.InteropServices;

namespace FastGuid
{
	partial struct Uuid
	{
		// "System.Native"
		public static unsafe Uuid NewUuid()
		{
			Uuid result;
			Interop.GetRandomBytes((byte*)&result, sizeof(Guid));

			// ReSharper disable InconsistentNaming
			const ushort VersionMask = 0xF000;
			const ushort RandomGuidVersion = 0x4000;

			const byte ClockSeqHiAndReservedMask = 0xC0;
			const byte ClockSeqHiAndReservedValue = 0x80;
			// ReSharper restore InconsistentNaming

			// TODO: run benchmarks compare with ExplicitLayout on Uuid (so no UuidMap will be needed)
			// Modify bits indicating the type of the GUID
			var map = (UuidMap*)&result;

			unchecked
			{
				// time_hi_and_version
				map->_c = (short)((map->_c & ~VersionMask) | RandomGuidVersion);
				// clock_seq_hi_and_reserved
				map->_d = (byte)((map->_d & ~ClockSeqHiAndReservedMask) | ClockSeqHiAndReservedValue);
			}

			return result;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct UuidMap
		{
			public int _a;
			public short _b;
			public short _c;
			public byte _d;
		}
	}
}
#endif