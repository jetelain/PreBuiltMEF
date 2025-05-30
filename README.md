# PreBuiltMEF

Experiment to try to improve Managed Extensibility Framework 1.0 performance: memory usage, and initialization time.

It's intended to allow large code bases based on MEF 1.0 to improve .NET 8 performance and allow NativeAOT usage.

## PreBuiltMEF for MEF

It relies on a "pre-built" catalog that is generated at build time using a source generator.

It uses MEF 1.0 packages to ensure compatibility with existing MEF 1.0 applications.

It allows NativeAOT compilations, as it discards the use of reflection. But NativeAOT does not support dynamic loading of assemblies. It's almost an oxymoron to use NativeAOT with MEF, but it works for static compositions (known at build time).

Current version supports only:
- Single imports
- Optional single imports (AllowDefault=true)
- Exports

Planned:
- Multiple imports (ImportMany)
- Deferred imports (Lazy)
- Imports with metadata

Most MEF features could be supported, as it really runs into MEF 1.0.

## PreBuiltMEF for Microsoft.Extensions.DependencyInjection 

It transforms MEF parts into Microsoft.Extensions.DependencyInjection primitives using a source generator.

Some MEF features might not be supported, as it runs on top of Microsoft.Extensions.DependencyInjection.

# Preliminary results

Based on a really basic test case with two parts and multiple exports/imports:

## Catalog loading time

`CatalogBenchmark`

| Method                        | Mean       | Error     | StdDev    | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------------------------ |-----------:|----------:|----------:|------:|-------:|-------:|----------:|------------:|
| MEF v1 Reflection             | 9,399.6 ns | 212.94 ns | 312.13 ns |  1.00 | 0.4578 |      - |   5.69 KB |        1.00 |
| MEF v1 Prebuilt (JIT)         |   376.1 ns |   6.39 ns |   9.37 ns |  0.04 | 0.1874 | 0.0010 |    2.3 KB |        0.40 |
| MEF v1 Prebuilt (NativeAOT)   |   422.8 ns |   3.09 ns |   4.43 ns |  0.04 | 0.1721 | 0.0010 |   2.11 KB |        0.37 |
| ServiceCollection (JIT)       |   150.0 ns |   1.81 ns |   2.71 ns |  0.02 | 0.1140 | 0.0005 |    1.4 KB |        0.25 |
| ServiceCollection (NativeAOT) |   174.0 ns |   1.00 ns |   1.41 ns |  0.02 | 0.1051 | 0.0002 |   1.29 KB |        0.22 |


## Composition time

`ComposeBenchmark`

.NET 8.0.16 (8.0.1625.21506), X64 RyuJIT AVX2

| Method                        | Mean     | Error     | StdDev    | Ratio | Gen0   | Gen1   | Gen2   | Allocated | Alloc Ratio |
|------------------------------ |---------:|----------:|----------:|------:|-------:|-------:|-------:|----------:|------------:|
| MEF v1 Reflection             | 5.868 us | 0.0671 us | 0.1004 us |  1.00 | 0.8850 | 0.0076 |      - |  10.92 KB |        1.00 |
| MEF v1 Prebuilt (JIT)         | 4.412 us | 0.0504 us | 0.0754 us |  0.75 | 0.7172 | 0.0763 | 0.0305 |   8.59 KB |        0.79 |
| MEF v1 Prebuilt (NativeAOT)   | 4.764 us | 0.0409 us | 0.0600 us |  0.81 | 0.6790 | 0.0763 | 0.0305 |   8.05 KB |        0.73 |
| ServiceCollection (JIT)       | 3.375 us | 0.1308 us | 0.1957 us |  0.58 | 0.8812 | 0.2174 |      - |   10.8 KB |        0.99 |
| ServiceCollection (NativeAOT) | 3.299 us | 0.1503 us | 0.2249 us |  0.56 | 0.7210 | 0.1793 |      - |   8.84 KB |        0.80 |