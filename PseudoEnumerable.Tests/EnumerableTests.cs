using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace PseudoEnumerable.Tests
{
    [TestFixture]
    public class EnumerableTests
    {
        [TestCase(new int[] { 1, 2, 3, 4, -56, -85, -9 }, ExpectedResult = new int[] { 1, 2, 3, 4 })]
        [TestCase(new int[] { 1, 1, 1, 1, -2, 1, 1 }, ExpectedResult = new int[] { 1, 1, 1, 1, 1, 1 })]
        [TestCase(new int[] { 1, 22, 33, 44, 5, 66 }, ExpectedResult = new int[] { 1, 22, 33, 44, 5, 66 })]
        [TestCase(new int[] { int.MaxValue, 0, int.MaxValue }, ExpectedResult = new int[] { int.MaxValue, int.MaxValue })]
        public IEnumerable<int> FilterIsPositiveTests(IEnumerable<int> array)
            => array.Filter(new Func<int, bool>(i => i > 0));


        [TestCase(new int[] { 1, 2, 3, 4, -56, -85, -9 }, ExpectedResult = new int[] { })]
        [TestCase(new int[] { 1, 1, 0, 1, -2, 0, 1 }, ExpectedResult = new int[] { 0,0})]
        [TestCase(new int[] { 1, 22, 33, 44, 5, 0}, ExpectedResult = new int[] { 0 })]
        [TestCase(new int[] { int.MaxValue, 0, int.MaxValue }, ExpectedResult = new int[] {0})]
        public IEnumerable<int> FilterIsZeroTests(IEnumerable<int> array)
            => array.Filter(new Func<int, bool>(i => i == 0));


        [TestCase(arg:new string[] { "day", "morning", "dog", "aaaa"}, ExpectedResult = new string[] {"morning" , "dog"})]
        public IEnumerable<string> FilterIsContainsTests(IEnumerable<string> array)
            => array.Filter(new Func<string, bool>(i => i.Contains('o')));

        [TestCase(new int[] { 1, 2, 3, 4, -56, -85, -9 }, ExpectedResult = false)]
        [TestCase(new int[] { 0, 0 }, ExpectedResult = true)]
        [TestCase(new int[] { 1, 22, 33, 44, 5, 0 }, ExpectedResult = false)]
        [TestCase(new int[] { int.MaxValue, 0, int.MaxValue }, ExpectedResult =false)]
        public bool FilterIsZeroForAllTests(IEnumerable<int> array)
            => array.ForAll(new Func<int, bool>(i => i == 0));
    }
}