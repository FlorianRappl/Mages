# MAGES Performance

Performance was one of the most important features for designing and implementing MAGES. All performance tests have been run against YAMP, as YAMP has a decent and well-understood performance behavior.

The tests are performed on a standard development machine using [BenchmarkDotNet v0.9.4.0](https://github.com/PerfDotNet/BenchmarkDotNet) on Microsoft Windows NT 6.2.9200.0. The system featured the following specs:

```ini
Processor=Intel(R) Core(TM) i5-4570 CPU @ 3.20GHz, ProcessorCount=4
Frequency=3124097 ticks, Resolution=320.0925 ns, Timer=TSC
HostCLR=MS.NET 4.0.30319.42000, Arch=64-bit RELEASE [RyuJIT]
JitModules=clrjit-v4.6.1080.0
```

## Standard Benchmarks

The barrier for the performance of MAGES is YAMP. Essentially, every operation of MAGES needs to be faster. From validation up to computing the product of matrices MAGES is though as a more-direct, easier to tame, yet more powerful successor. This results in the following benchmark of trivial operations.

The smallest relative difference is in a test that includes multiple statements with variable access. The main time is spent on variable resolution, i.e., there is not much to distinguish between the two. Variable access works similar in MAGES. There is no upfront resolution, even though this would increase the performance. The reasoning behind this design choice is in the dynamic freedom. This way one can use caching without fixing the global scope.

                                 Method |     Median |    StdDev |
--------------------------------------- |----------- |---------- |
 Mages_AddMultiplyDivideAndPowerNumbers |  5.2891 us | 0.0836 us |
  Yamp_AddMultiplyDivideAndPowerNumbers |  8.8750 us | 0.2023 us |
                    Mages_AddTwoNumbers |  1.3968 us | 0.0101 us |
                     Yamp_AddTwoNumbers |  1.9657 us | 0.0691 us |
            Mages_CallStandardFunctions |  8.5316 us | 0.1030 us |
             Yamp_CallStandardFunctions | 16.5168 us | 0.2329 us |
             Mages_MultiplyTwoVariables |  4.2642 us | 0.0393 us |
              Yamp_MultiplyTwoVariables |  5.5230 us | 0.1062 us |
               Mages_Transpose4x5Matrix | 17.7762 us | 0.2434 us |
                Yamp_Transpose4x5Matrix | 41.7720 us | 0.9471 us |

The largest difference can be seen in the transpose operation. Even though filling the matrix requires a lot of operations in MAGES, the way that matrices are handled is much faster than it was formerly in YAMP. This direct way is also reflected in method handling. This explains why MAGES outperforms YAMP easily when calling a lot of standard functions.

## Effects of Caching

A novel feature of MAGES is the capability to "compile" interpretation-ready code. This way, the whole process of validating and transforming source code to operations is omitted. The speedup is immense.

The following benchmark considers a simple query of the form 

```c
sin(pi / 4) * cos(pi * 0.25) + exp(2) * log(3)
```

evaluated with a trivial caching mechanism in form of a `Dictionary<String, Func<Object>>`. We compare MAGES against itself without the caching, as we know that MAGES itself already outperforms YAMP by a factor of two.

                               Method |    Median |    StdDev |
------------------------------------- |---------- |---------- |
   Mages_Cached_CallStandardFunctions | 1.0348 us | 0.0058 us |
 Mages_Uncached_CallStandardFunctions | 8.6564 us | 0.2163 us |

Caching may (dependent on the expression) be several binary orders faster than directly evaluating queries. This is crucial for repeating user input that originated from script files. It is possible to evaluate the user input in the beginning (or just after it changes) and use the cached result for faster, more energy-conserving results.