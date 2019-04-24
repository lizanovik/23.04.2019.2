using System;
using System.Collections;
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

        [TestCase(arg: new object [] {1, 2, 3, 4, -56, -85, -9}, ExpectedResult = new int[] { 1, 2, 3, 4, -56, -85, -9 })]
        public IEnumerable<int> CastToTests(IEnumerable array)
            => Enumerable.CastTo<int>(array);

        [Test]
        public void CastTo_InvalidItem_ThrowsInvalidCastException()
            => Assert.Throws<InvalidCastException>(() => Enumerable.CastTo<int>(new object[] {5,"5" }));


        [TestCase(arg: new int[] {1, 2, 3, 4, -56, -85, -9}, ExpectedResult = new int[] {1, 2, 3, 4,-9, -56, -85})]
        [TestCase(arg: new int[] {-5, 5, 2,4,1}, ExpectedResult = new int[] { 1,2,4,-5,5 })]
        public IEnumerable<int> SortByAbsTests(IEnumerable<int> array)
            => Enumerable.SortBy(array, new Func<int, int>(x => Math.Abs(x)));

        [TestCase(arg: new string[] { "good", "morning", "my", "dear"}, ExpectedResult = new string[] { "my", "good", "dear","morning" })]
        public IEnumerable<string> SortByLengthTests(IEnumerable<string> array)
            => Enumerable.SortBy(array, new Func<string, int>(x => x.Length));


        [TestCase(arg: new int[] { 1, 2, 3, 4 }, ExpectedResult = new string[] { "1", "2", "3", "4" })]
        public IEnumerable<string> TransformTests(IEnumerable<int> array)
            => Enumerable.Transform(array, new Func<int, string>(x => x.ToString()));

        [TestCase(2, 5, ExpectedResult = new int[] { 2, 3, 4, 5, 6})]
        public IEnumerable<int> GenerateTests(int start, int count)
            => Enumerable.Generate(start, count, new Func<int, int>(x => x++));

        [TestCase(9, 4, ExpectedResult = new int[] {81, 100, 121, 144})]
        public IEnumerable<int> GenerateMoreTests(int start, int count)
            => Enumerable.Generate(start, count, new Func<int, int>(x => x*x));
    }
}