using System;
using System.Diagnostics;

namespace StopwatchExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            for (int i = 0; i <= 100; i++)
            {
                sw.Restart();
                int f = Fib(i);
                sw.Stop();
                Console.WriteLine($"{i} - {f} ({sw.ElapsedMilliseconds} ms)");
            }
        }

        static int Fib(int n)
        {
            // 0 1 1 2 3 5 8 ...
            if (n < 0) throw new ArgumentException("No válido para negativos");
            if (n <= 1) return n;
            return Fib(n - 1) + Fib(n - 2);
        }
    }
}
