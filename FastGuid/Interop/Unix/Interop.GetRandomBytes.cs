#if UNIX
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
internal partial class Interop
{
	internal unsafe class Sys
	{
		[DllImport(Libraries.SystemNative, EntryPoint = "SystemNative_GetNonCryptographicallySecureRandomBytes")]
		internal static extern void GetNonCryptographicallySecureRandomBytes(byte* buffer, int length);
	}

	internal static unsafe void GetRandomBytes(byte* buffer, int length)
	{
		Sys.GetNonCryptographicallySecureRandomBytes(buffer, length);
	}
}
#endif