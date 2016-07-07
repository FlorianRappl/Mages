# MAGES Performance

Performance was one of the most important features for designing and implementing MAGES. All performance tests have been run against YAMP, as YAMP has a decent and well-understood performance behavior.

The tests are performed on a standard development machine using [BenchmarkDotNet v0.9.6.0](https://github.com/PerfDotNet/BenchmarkDotNet) on Microsoft Windows NT 6.2.9200.0. The system featured the following specs:

```ini
Processor=Intel(R) Core(TM) i5-4570 CPU @ 3.20GHz, ProcessorCount=4
Frequency=3124097 ticks, Resolution=320.0925 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1080.0
```

## Standard Benchmarks

The barrier for the performance of MAGES is YAMP. Essentially, every operation of MAGES needs to be faster. From validation up to computing the product of matrices MAGES is though as a more-direct, easier to tame, yet more powerful successor. This results in the following benchmark of trivial operations.

The smallest relative difference is in a test that includes multiple statements with variable access. The main time is spent on variable resolution, i.e., there is not much to distinguish between the two. Variable access works similar in MAGES. There is no upfront resolution, even though this would increase the performance. The reasoning behind this design choice is in the dynamic freedom. This way one can use caching without fixing the global scope.

| Parser |                           Method |     Median |    StdDev |
| ------ | -------------------------------- |----------- |---------- |
| Mages  | AddMultiplyDivideAndPowerNumbers |  5.1666 us | 0.0879 us |
| Yamp   | AddMultiplyDivideAndPowerNumbers |  8.4325 us | 0.0220 us |
| Mages  |                    AddTwoNumbers |  1.3742 us | 0.0136 us |
| Yamp   |                    AddTwoNumbers |  1.8482 us | 0.0122 us |
| Mages  |            CallStandardFunctions |  8.3647 us | 0.0685 us |
| Yamp   |            CallStandardFunctions | 16.3588 us | 0.1172 us |
| Mages  |             MultiplyTwoVariables |  4.0711 us | 0.0325 us |
| Yamp   |             MultiplyTwoVariables |  5.3462 us | 0.0322 us |
| Mages  |               Transpose4x5Matrix | 15.7278 us | 0.2363 us |
| Yamp   |               Transpose4x5Matrix | 41.2478 us | 0.2637 us |

The largest difference can be seen in the transpose operation. Even though filling the matrix requires a lot of operations in MAGES, the way that matrices are handled is much faster than it was formerly in YAMP. This direct way is also reflected in method handling. This explains why MAGES outperforms YAMP easily when calling a lot of standard functions.

## Effects of Caching

A novel feature of MAGES is the capability to "compile" interpretation-ready code. This way, the whole process of validating and transforming source code to operations is omitted. The speedup is immense.

The following benchmark considers a simple query of the form 

```c
sin(pi / 4) * cos(pi * 0.25) + exp(2) * log(3)
```

evaluated with a trivial caching mechanism in form of a `Dictionary<String, Func<Object>>`. We compare MAGES against itself without the caching, as we know that MAGES itself already outperforms YAMP by a factor of two.

| Cached? |                Method |    Median |    StdDev |
| ------- | --------------------- |---------- |---------- |
| Yes     | CallStandardFunctions | 1.1564 us | 0.0123 us |
| No      | CallStandardFunctions | 8.3767 us | 0.0543 us |

Caching may (dependent on the expression) be several binary orders faster than directly evaluating queries. This is crucial for repeating user input that originated from script files. It is possible to evaluate the user input in the beginning (or just after it changes) and use the cached result for faster, more energy-conserving results.

## Extended Benchmarks

Until now we've seen that MAGES is decently faster than YAMP. But what about the performance for extended capabilities, e.g., custom-defined functions or matrix operations. The following table compares some of these operations.

| Parser |                 Method |          Median |        StdDev |
| ------ | ---------------------- |---------------- |-------------- |
| Mages  |   CreateAndUseFunction |      17.5819 us |     0.0444 us |
| Yamp   |   CreateAndUseFunction |      24.5831 us |     0.2574 us |
| Mages  |  MultiplySmallMatrices |       5.1958 us |     0.0929 us |
| Yamp   |  MultiplySmallMatrices |      13.4905 us |     0.1122 us |
| Mages  | MultiplyMediumMatrices |     265.5289 us |     0.7518 us |
| Yamp   | MultiplyMediumMatrices |  17,874.4550 us |   130.2489 us |
| Mages  |  MultiplyLargeMatrices |   4,753.0534 us |    25.9203 us |
| Yamp   |  MultiplyLargeMatrices | 298,255.1038 us | 1,298.1635 us |
| Mages  |           ObjectAccess |       8.8594 us |     0.0871 us |
| Yamp   |           ObjectAccess |      18.7118 us |     0.0976 us |

As we can see MAGES does a great job beating YAMP in every scenario. Especially for matrices MAGES brings a huge performance gain to the table. In the benchmark above even with multiplying two "large" matrices MAGES outperforms the performance of YAMP when multiplying medium matrices. Already for small matrices we see a nice gain by using MAGES.

Another area of improvement is objects. Initially, YAMP did not know about objects, however, with one of the more recent versions support for arbitrary objects has been added. Now we see that MAGES, which comes with object literals and extended support from the beginning, is able to outperform MAGES already in simple scenarios easily.

## Comparison Benchmark

Let's compare MAGES against other math expression parsers. Needless to say that MAGES offers much more than most of these libraries, however, having a quick comparison in this area is still interesting.

For this comparison benchmark we use a (potentially modified version of the) query:

```C
2^3 * (2 + 3 * sin(1) / 0.3 - sqrt(5))
```

The following table shows how MAGES compares against its competition in the area of math expression parsers.

|                 Parser |         Median |      StdDev |
| ---------------------- |--------------- |------------ |
|        MAGES (Default) |     11.1582 us |   0.6506 us |
|         MAGES (Cached) |      1.3827 us |   0.0499 us |
|                   YAMP |     30.9068 us |   0.5521 us |
|               mxParser | 11,543.4065 us | 216.0238 us |
|                dotMath |      8.9858 us |   0.1715 us |
|               MuParser |     68.0219 us |  15.4130 us |
|     GuiLabs.MathParser |     16.4246 us |   3.0767 us |
|                 Arpain |     15.1224 us |   1.8141 us |
|                     S# |    246.5942 us |  32.9897 us |
|         Jace (Default) |      1.1588 us |   0.2792 us |
|        Jace (Uncached) |     12.7334 us |   1.7223 us |
|     Jace (Interpreted) |     13.1057 us |   2.0007 us |
|            Jace (Bare) |     12.8641 us |   2.8407 us |

We see that Jace is potentially the closest one to MAGES. They are both very similar, with Jace defaulting to cached evaluation. Without caching dotMath is doing a great job.

Please note, however, that the original query throws an exception in dotMath as there seems to be a bug in handling the round brackets as a single expression on the right hand side. Therefore, the value has to be taken with a small question mark.

Finally, S# - which is quite similar to MAGES from some of its goals - is much more heavyweight.
