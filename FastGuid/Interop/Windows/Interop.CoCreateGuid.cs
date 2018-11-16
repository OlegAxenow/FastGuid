#if WINDOWS
using System.Runtime.InteropServices;
using FastGuid;

// ReSharper disable once CheckNamespace
internal partial class Interop
{
	[DllImport(Libraries.Ole32)]
	internal static extern int CoCreateGuid(out Uuid guid);
}
#endif