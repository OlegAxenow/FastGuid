## FastGuid

To make ```System.Guid``` faster. Prototype stage.
Project at the prototype stage. I hope in the end I will issue a pull request and it will be accepted.

## Story

We actively used GUID on my work.
We recently discussed with @force-net that ```System.Guid``` has too many fields,
which can adversely affect the performance of some methods.

So, I create struct with two ```ulong``` fields with only basic methods and run several benchmarks.

## Specific details

1. To avoid collisions, I decide to make ```Uuid``` struct instead of ```Guid``` struct with different namespace.
2. Some methods use ```MethodImplOptions.AggressiveInlining``` because relatively small.
3. Some methods use ```_second``` field first. This is because "sequential" identifiers can be used in some projects.

## Benchmarks

You can run benchmarks yourself from Benchmark folder. For Windows you can use ```run.cmd```.
Of course, methods with ```System.Object``` is not fast by default, so, here the two interesting benchmarks' results for typed methods.

Environment:

``` ini

BenchmarkDotNet=v0.11.2, OS=Windows 10.0.17134.345 (1803/April2018Update/Redstone4)
Intel Core i7-2600 CPU 3.40GHz (Sandy Bridge), 1 CPU, 8 logical and 4 physical cores
Frequency=3312790 Hz, Resolution=301.8604 ns, Timer=TSC
.NET Core SDK=2.1.301
  [Host]     : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.1 (CoreCLR 4.6.26606.02, CoreFX 4.6.26606.05), 64bit RyuJIT
```

Notes:

* I use two benchmarks for each struct to see the difference between comparing the same and different identifiers.
* "Guid" means calling original ```System.Guid``` methods, "Uuid" -- ```FastGuid.Uuid``` methods.

### CompareTo

|               Method |      Mean |     Error |    StdDev |       Min |       Max | Ratio | RatioSD |
|--------------------- |----------:|----------:|----------:|----------:|----------:|------:|--------:|
| GuidDifferentCompare |  5.598 ns | 0.0326 ns | 0.0305 ns |  5.567 ns |  5.657 ns |  1.00 |    0.00 |
|      GuidSameCompare | 16.125 ns | 0.0353 ns | 0.0331 ns | 16.080 ns | 16.194 ns |  2.88 |    0.02 |
| UuidDifferentCompare |  1.883 ns | 0.0068 ns | 0.0064 ns |  1.873 ns |  1.896 ns |  0.34 |    0.00 |
|      UuidSameCompare |  3.429 ns | 0.0096 ns | 0.0080 ns |  3.417 ns |  3.443 ns |  0.61 |    0.00 |

### Equals

|              Method |     Mean |     Error |    StdDev |      Min |      Max | Ratio |
|-------------------- |---------:|----------:|----------:|---------:|---------:|------:|
| GuidDifferentEquals | 3.760 ns | 0.0172 ns | 0.0153 ns | 3.740 ns | 3.786 ns |  1.00 |
|      GuidSameEquals | 6.624 ns | 0.0119 ns | 0.0100 ns | 6.610 ns | 6.643 ns |  1.76 |
| UuidDifferentEquals | 1.095 ns | 0.0147 ns | 0.0123 ns | 1.078 ns | 1.110 ns |  0.29 |
|      UuidSameEquals | 1.352 ns | 0.0039 ns | 0.0033 ns | 1.345 ns | 1.357 ns |  0.36 |


## Roadmap

* Implement and test other methods.
* Add unit-tests.
* Discuss this with .NET Core team on GitHub.
* Issue a pull request if it makes sense ot refine and release this project as third-party library.

## Requirements and dependencies

License: [MIT](http://opensource.org/licenses/MIT).

```FastGuid``` project does not depend on third-party libraries.

```Benchmarks``` project has external dependencies:

* BenchmarkDotNet
* CommandLineParser