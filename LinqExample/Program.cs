using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var numeros = GetIntegers();

            var pares = numeros.Where(n =>
            {
                return n % 2 == 0;
            });

            var cuadradosPares = pares.Select(n => n * n);
            var primeros100 = cuadradosPares.Take(100);

            foreach (var n in primeros100)
            {
                Console.WriteLine(n);
            }

        }

        static IEnumerable<int> GetIntegers()
        {
            int n = 0;
            while(true)
            {
                yield return n++;
            }
        }
    }
}
