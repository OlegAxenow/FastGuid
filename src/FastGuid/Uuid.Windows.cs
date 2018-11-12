#if WINDOWS || DEBUG
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FastGuid
{
	partial struct Uuid
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Uuid NewUuid()
		{
			int hr = CoCreateGuid(out var result);

			// CoCreateGuid should not return an error, throw just to be on the safe side
			if (hr != 0)
				throw new UuidException(hr);

			return result;
		}

		private class UuidException : Exception
		{
			public UuidException(int hResult)
			{
				HResult = hResult;
			}
		}

		[DllImport("ole32.dll")]
		private static extern int CoCreateGuid(out Uuid uuid);
	}
}
#endif