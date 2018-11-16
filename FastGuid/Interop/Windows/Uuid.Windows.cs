#if WINDOWS
using System;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace FastGuid
{
	partial struct Uuid
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Uuid NewUuid()
		{
			int hr = Interop.CoCreateGuid(out var result);

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
	}
}
#endif