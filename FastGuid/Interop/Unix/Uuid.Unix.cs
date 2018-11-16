#if UNIX
using System;

// ReSharper disable once CheckNamespace
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

			// Modify bits indicating the type of the GUID

			unchecked
			{
				// time_hi_and_version
				result._c = (short)((result._c & ~VersionMask) | RandomGuidVersion);
				// clock_seq_hi_and_reserved
				result._d = (byte)((result._d & ~ClockSeqHiAndReservedMask) | ClockSeqHiAndReservedValue);
			}

			return result;
		}
	}
}
#endif