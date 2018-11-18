## FastGuid

To make ```System.Guid``` faster.

Project at the prototype stage. I hope in the end I will issue a pull request and it will be accepted.

## Story

We actively used GUID at work.
We recently discussed with @force-net that ```System.Guid``` has too many fields,
which can adversely affect the performance of some methods.

So, I created a struct with two ```ulong``` fields with only basic methods and run several benchmarks.
I liked the results and I decided to continue optimizations...

## Specific details

1. To avoid collisions, I decide to use ```Uuid``` name for struct instead of ```Guid``` name.
2. Some methods use ```MethodImplOptions.AggressiveInlining``` because relatively small.
3. Some methods use ```_second``` field first. This is because "sequential" identifiers can be used in some projects.

## Benchmarks

Of course, methods with ```System.Object``` is not fast by default, so, here the benchmarks' results for some typed methods.
You can run these and other benchmarks yourself from Benchmark folder. For Windows you can use ```run.cmd```.

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

* "Guid" prefix used for original ```System.Guid``` methods, "Uuid" — for ```FastGuid.Uuid``` methods.
* For ```CompareTo``` and ```Equals``` two benchmarks used for each struct to see the difference between comparing the same and different identifiers.
* For ```CompareTo``` and ```Equals``` appropriate methods called twice like ```_guid1.CompareTo(_guid2) ^ _guid2.CompareTo(_guid1)```, because single call too fast.
* For ```ToString``` last letter means one of the [standard guid formats](https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=netcore-2.1) — D, B, N or X.

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

### ToString

|        Method |     Mean |     Error |    StdDev |      Min |      Max | Ratio |
|-------------- |---------:|----------:|----------:|---------:|---------:|------:|
| GuidToStringD | 59.02 ns | 0.1131 ns | 0.1058 ns | 58.82 ns | 59.21 ns |  1.00 |
| GuidToStringB | 59.08 ns | 0.1472 ns | 0.1377 ns | 58.87 ns | 59.35 ns |  1.00 |
| GuidToStringN | 61.67 ns | 0.0938 ns | 0.0783 ns | 61.56 ns | 61.81 ns |  1.05 |
| GuidToStringX | 68.10 ns | 0.3475 ns | 0.3251 ns | 67.59 ns | 68.69 ns |  1.15 |
| UuidToStringD | 24.95 ns | 0.0414 ns | 0.0346 ns | 24.91 ns | 25.01 ns |  0.42 |
| UuidToStringB | 28.32 ns | 0.0455 ns | 0.0404 ns | 28.26 ns | 28.40 ns |  0.48 |
| UuidToStringN | 24.01 ns | 0.0446 ns | 0.0396 ns | 23.96 ns | 24.11 ns |  0.41 |
| UuidToStringX | 33.82 ns | 0.1509 ns | 0.1411 ns | 33.56 ns | 34.04 ns |  0.57 |

## Roadmap

* Implement and test other methods.
* Add unit-tests.
* Discuss this with .NET Core team on GitHub.
* Issue a pull request if it makes sense or refine and release this project as third-party library.

## Requirements and dependencies

License: [MIT](http://opensource.org/licenses/MIT).

```FastGuid``` project does not depend on third-party libraries.

```Benchmarks``` project has external dependencies:

* BenchmarkDotNet
* CommandLineParser