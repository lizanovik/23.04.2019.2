using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PseudoEnumerable;

namespace PseudoEnumerableConsole
{
    class Program
    {
        static void Main(string[] args)
        {
           string[] source = new[] {"good", "morning", "my", "dear"};
           var result = source.Transform(s => s.Length);
           var resultLinq = source.Select(s => s.Length);
           foreach (var item in result)
           {
               Console.Write(item + " ");
           }

           Console.WriteLine();
           foreach (var item in resultLinq)
           {
               Console.Write(item + " ");
           }

           Console.WriteLine();
           int[] source1 = new int[] {1, 2, -3, 4, -56, -85, -9};
           int[] output = new int[] {1, 2, 3, 4};
           var result1 = source1.Filter(i => i > 0);
           var result1Linq = source1.Where(i => i > 0);
           foreach (var item in result1)
           {
               Console.Write(item + " ");
           }

           Console.WriteLine();
           foreach (var item in result1Linq)
           {
               Console.Write(item + " ");
           }

           var source2 = new object[] { "good", "morning", "my", "dear"};
           var result2 = source2.CastTo<string>();
           foreach (var item in result2)
           {
               Console.Write(item + " ");
           }

           var result22Linq = source2.OfType<string>();
           foreach (var item in result22Linq)
           {
               Console.Write(item + " ");
           }
            var result2Linq = source2.Cast<string>();
           foreach (var item in result2Linq)
           {
               Console.Write(item + " ");
           }
        }
    }
}
