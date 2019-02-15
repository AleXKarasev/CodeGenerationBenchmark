using BenchmarkDotNet.Running;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<Runner>();
            // var runner = new Runner();
            // runner.Generic();
            // runner.Native();
        }
    }
}