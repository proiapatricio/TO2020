using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace RaceCondition
{
    class Program
    {
        static int counter;
        const int nthreads = 30;
        const int niter = 5000000;

        static void Main(string[] args)
        {
            Test(nameof(Increment), Increment);
            Test(nameof(IncrementWithLock), IncrementWithLock);
            Test(nameof(IncrementWithInterlocked), IncrementWithInterlocked);
        }

        static void Test(string testName, ThreadStart threadStart)
        {
            counter = 0;
            Console.WriteLine(testName);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            {
                // Creamos unos cuantos threads
                var threads = new List<Thread>();
                for (int i = 0; i < nthreads; i++)
                {
                    threads.Add(new Thread(threadStart));
                }

                // Iniciamos todos los threads
                for (int i = 0; i < nthreads; i++)
                {
                    threads[i].Start();
                }

                // Esperamos que todos los threads terminen la ejecución
                for (int i = 0; i < nthreads; i++)
                {
                    threads[i].Join();
                }
            }
            sw.Stop();

            int expected = niter * nthreads;
            Console.WriteLine($"ACTUAL: {counter}, EXPECTED: {expected}");
            Console.WriteLine($"RESULT: {(counter == expected ? "SUCCESS" : "ERROR")}");
            Console.WriteLine($"TIME (ms): {sw.ElapsedMilliseconds}");
            Console.WriteLine();
        }

        static void Increment()
        {
            for (int i = 0; i < niter; i++)
            {
                // Sin ningún mecanismo de sincronización múltiples threads acceden a 
                // esta variable al mismo tiempo. Race condition!
                counter++;
            }
        }

        static object counter_lock = new object();
        static void IncrementWithLock()
        {
            for (int i = 0; i < niter; i++)
            {
                // Usando un lock sólo un thread a la vez puede acceder a la variable. 
                // Ahora el resultado es el correcto pero el proceso tarda mucho más.
                lock (counter_lock)
                {
                    counter++;
                }
            }
        }

        static void IncrementWithInterlocked()
        {
            for (int i = 0; i < niter; i++)
            {
                // Usando Interlocked podemos incrementar la variable de forma atómica.
                Interlocked.Increment(ref counter);
            }
        }
    }
}
