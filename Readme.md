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
* ```Equals``` benchmark results too good because of aggressive inlining (can be slower when inlining not available).
* For ```CompareTo``` and ```Equals``` two benchmarks used for each struct to see the difference between comparing the same and different identifiers.
* For ```CompareTo``` and ```Equals``` appropriate methods called thrice like ```_guid1.CompareTo(_guid2) ^ _guid2.CompareTo(_guid3) ^ _guid3.CompareTo(_guid1)```.
* For ```ToString``` last letter means one of the [standard guid formats](https://docs.microsoft.com/en-us/dotnet/api/system.guid.tostring?view=netcore-2.1) — D, B, N or X.

### CompareTo

|               Method |      Mean |     Error |    StdDev |       Min |       Max | Ratio |
|--------------------- |----------:|----------:|----------:|----------:|----------:|------:|
| GuidDifferentCompare |  8.019 ns | 0.0194 ns | 0.0162 ns |  8.001 ns |  8.063 ns |  1.00 |
|      GuidSameCompare | 24.242 ns | 0.0817 ns | 0.0764 ns | 24.102 ns | 24.335 ns |  3.02 |
| UuidDifferentCompare |  3.862 ns | 0.0056 ns | 0.0053 ns |  3.855 ns |  3.870 ns |  0.48 |
|      UuidSameCompare | 13.991 ns | 0.0711 ns | 0.0665 ns | 13.903 ns | 14.110 ns |  1.75 |

### Equals

|              Method |     Mean |     Error |    StdDev |      Min |      Max | Ratio |
|-------------------- |---------:|----------:|----------:|---------:|---------:|------:|
| GuidDifferentEquals | 5.866 ns | 0.0185 ns | 0.0173 ns | 5.833 ns | 5.891 ns |  1.00 |
|      GuidSameEquals | 7.537 ns | 0.0067 ns | 0.0059 ns | 7.525 ns | 7.549 ns |  1.29 |
| UuidDifferentEquals | 2.387 ns | 0.0080 ns | 0.0067 ns | 2.378 ns | 2.401 ns |  0.41 |
|      UuidSameEquals | 4.034 ns | 0.0138 ns | 0.0129 ns | 4.000 ns | 4.051 ns |  0.69 |

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

### TryParseExact

|             Method |      Mean |     Error |    StdDev |    Median |       Min |       Max | Ratio |
|------------------- |----------:|----------:|----------:|----------:|----------:|----------:|------:|
| GuidTryParseExactD | 295.73 ns | 0.5214 ns | 0.6207 ns | 295.67 ns | 294.35 ns | 297.03 ns |  1.00 |
| GuidTryParseExactB | 293.97 ns | 1.0966 ns | 1.3468 ns | 294.20 ns | 288.35 ns | 295.10 ns |  0.99 |
| GuidTryParseExactN | 417.32 ns | 0.4367 ns | 0.5522 ns | 417.35 ns | 416.38 ns | 418.63 ns |  1.41 |
| GuidTryParseExactX | 697.23 ns | 1.3468 ns | 1.6540 ns | 697.81 ns | 693.32 ns | 699.37 ns |  2.36 |
| UuidTryParseExactD |  39.67 ns | 0.3329 ns | 0.4444 ns |  39.42 ns |  39.30 ns |  40.74 ns |  0.13 |
| UuidTryParseExactB |  40.94 ns | 0.2564 ns | 0.3423 ns |  41.07 ns |  40.10 ns |  41.15 ns |  0.14 |
| UuidTryParseExactN |  39.86 ns | 0.1022 ns | 0.1217 ns |  39.81 ns |  39.73 ns |  40.19 ns |  0.13 |
| UuidTryParseExactX |  44.96 ns | 0.1092 ns | 0.1458 ns |  45.01 ns |  44.69 ns |  45.13 ns |  0.15 |

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