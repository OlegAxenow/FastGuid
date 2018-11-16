using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Running;
using CommandLine;

namespace Benchmarks
{
	class Program
	{
		private static readonly Type[] Benchmarks =
		{
			typeof(CompareToBenchmark), typeof(CompareToObjectBenchmark), typeof(CompareToHalfObjectBenchmark),
			typeof(EqualsBenchmark), typeof(EqualsObjectBenchmark), typeof(EqualsHalfObjectBenchmark),
			typeof(GetHashCodeBenchmark), typeof(NewIdBenchmark),
			typeof(DictionarySearchBenchmark), typeof(DictionaryInsertBenchmark),
			typeof(BytesConversionBenchmark), typeof(GuidConversionBenchmark),
			typeof(ToStringBenchmark)
		};

		private static readonly Dictionary<string, Type> BenchmarkDictionary;

		static Program()
		{
			BenchmarkDictionary = Benchmarks.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
		}

		static void Main(string[] args)
		{
#if DEBUG
			CustomConfig.BenchmarkType = BenchmarkType.Default;
			CustomConfig.ShowMemory = false;
			var bench = new DictionaryInsertBenchmark
			{
				TotalCount = 100
			};
			bench.Setup();
			bench.InsertUuid();
#else
			var parser = new Parser(with =>
				with.HelpWriter = new HelpWriter());
			parser.ParseArguments<Options>(args)
				.WithParsed(o =>
				{
					CustomConfig.ShowMemory = o.ShowMemory;
					CustomConfig.BenchmarkType = o.Type;

					if (o.BenchmarkName.EndsWith(".cs"))
						o.BenchmarkName = o.BenchmarkName.Substring(0, o.BenchmarkName.Length - ".cs".Length);

					if (!BenchmarkDictionary.TryGetValue(o.BenchmarkName, out Type benchmark))
					{
						Console.Error.WriteLine($"Not found benchmark '{o.BenchmarkName}'.");
						HelpWriter.ShowAvailableBenchmarks();
						return;
					}

					BenchmarkRunner.Run(benchmark);
				});
#endif
		}

		public class HelpWriter : TextWriter
		{
			public override Encoding Encoding => Console.OutputEncoding;

			public override void Write(char value)
			{
				Console.Error.Write(value);
			}

			public override void Close()
			{
				ShowAvailableBenchmarks();

				base.Close();
			}

			public static void ShowAvailableBenchmarks()
			{
				Console.Error.WriteLine("Available benchmarks:");
				foreach (var type in Benchmarks.OrderBy(x => x.Name))
				{
					Console.Error.WriteLine(type.Name);
				}
			}
		}

		public class Options
		{
			[Option('n', "name", Required = true, HelpText = "One of the benchmarks listed below")]
			public string BenchmarkName { get; set; }

			[Option('m', "memory", HelpText = "Add memory diagnostics")]
			public bool ShowMemory { get; set; }

			[Option('t', "type", HelpText = "Type of tests (Fast, Default, Thorough)")]
			public BenchmarkType Type { get; set; }
		}
	}
}