using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;

namespace Benchmarks
{
	internal class CustomConfig : ManualConfig
	{
		public static bool ShowMemory;
		public static BenchmarkType BenchmarkType;

		public CustomConfig()
		{
			switch (BenchmarkType)
			{
				case BenchmarkType.Default:
					break;
				case BenchmarkType.Fast:
					Add(Job.Core.WithLaunchCount(1).WithWarmupCount(2).WithIterationCount(5));
					break;
				case BenchmarkType.Thorough:
					Add(Job.Core.WithLaunchCount(1).WithWarmupCount(15).WithIterationCount(25));
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (ShowMemory)
			{
				Add(new MemoryDiagnoser());
			}

			/*Add(new MemoryDiagnoser());
			Add(JitOptimizationsValidator.FailOnError);
			Add(RPlotExporter.Default);*/
		}
	}
}