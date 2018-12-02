using System;
using System.IO;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CustomCoreClr;

namespace Benchmarks
{
	internal class CustomConfig : ManualConfig
	{
		public static bool ShowMemory;
		public static BenchmarkType BenchmarkType;

		public static Fork[] PathToForks =
		{
			new Fork
			{
				DisplayName = "original-3.0",
				PathToPackages = @"b:\dotnet\packages",
				CoreClrVersion = "3.0.0-preview-27230-0",
				PathToClr = @"b:\dotnet\coreclr\",
				CoreFxVersion = "4.6.0-dev.18601.1",
				PathToCoreFx = @"b:\dotnet\corefx\",
			},
			new Fork
			{
				DisplayName = "custom-3.0",
				PathToPackages = @"n:\github\packages",
				CoreClrVersion = "3.0.0-preview-27230-0",
				PathToClr = @"n:\github\coreclr\",
				CoreFxVersion = "4.6.0-dev.18601.1",
				PathToCoreFx = @"n:\github\corefx\",
			}
		};

		// TODO: move forks initialization to config or command line

		public struct Fork
		{
			public string CoreClrVersion;
			public string CoreFxVersion;
			public string PathToClr;
			public string PathToCoreFx;
			public string PathToPackages;
			public string DisplayName;
		}

		public CustomConfig()
		{
			foreach (var fork in PathToForks)
			{
				if (!Directory.Exists(fork.PathToPackages))
					Directory.CreateDirectory(fork.PathToPackages);

				Add(GetJob().With(
					CustomCoreClrToolchain.CreateBuilder()
						.UseCoreClrLocalBuild(fork.CoreClrVersion, Path.Combine(fork.PathToClr, @"bin\Product\Windows_NT.x64.Release\.nuget\pkg"),
							fork.PathToPackages)
						.UseCoreFxLocalBuild(fork.CoreFxVersion, Path.Combine(fork.PathToCoreFx, @"artifacts\packages\Release"))
						.DisplayName(fork.DisplayName)
						.ToToolchain()));
			}

			Add(GetJob());

			if (ShowMemory)
			{
				Add(new MemoryDiagnoser());
			}

			/*Add(new MemoryDiagnoser());
			Add(JitOptimizationsValidator.FailOnError);
			Add(RPlotExporter.Default);*/
		}

		private static Job GetJob()
		{
			switch (BenchmarkType)
			{
				case BenchmarkType.Default:
					return Job.Default;
				case BenchmarkType.Fast:
					return Job.ShortRun;
				case BenchmarkType.Thorough:
					return Job.LongRun;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}