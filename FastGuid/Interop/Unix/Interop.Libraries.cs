#if UNIX
// ReSharper disable once CheckNamespace
internal static partial class Interop
{
	/// <summary>
	/// Subset of https://github.com/dotnet/corefx/blob/master/src/Common/src/CoreLib/Interop/Unix/Interop.Libraries.cs
	/// </summary>
	internal static class Libraries
	{
		internal const string SystemNative = "System.Native";
	}
}
#endif