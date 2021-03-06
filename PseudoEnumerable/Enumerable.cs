﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

namespace PseudoEnumerable
{
    public static class Enumerable
    {
        #region API
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">An <see cref="IEnumerable{TSource}"/> to filter.</param>
        /// <param name="predicate">A function to test each source element for a condition</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> that contains elements from the input
        ///     sequence that satisfy the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="predicate"/> is null.</exception>
        public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            Validate(source, predicate);
            return DoFilter(source, predicate);
        }

        /// <summary>
        /// Transforms each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by transformer.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="transformer">A transform function to apply to each source element.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TResult}"/> whose elements are the result of
        ///     invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="transformer"/> is null.</exception>
        public static IEnumerable<TResult> Transform<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TResult> transformer)
        {
            Validate(source, transformer);
            return DoTransform(source, transformer);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by key.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="key">A function to extract a key from an element.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="key"/> is null.</exception>
        public static IEnumerable<TSource> SortBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> key)
        {
            Validate(source, key);
            return DoSortBy(new SortedDictionary<TKey, List<TSource>>(), source, key);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according by using a specified comparer for a key .
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by key.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="key">A function to extract a key from an element.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="comparer"/> is null.</exception>
        public static IEnumerable<TSource> SortBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> key, IComparer<TKey> comparer)
        {
            Validate(source, key);
            if (comparer is null)
            {
                throw new ArgumentNullException($"{nameof(comparer)} cannot be null!");
            }

            return DoSortBy(new SortedDictionary<TKey, List<TSource>>(comparer), source, key);
        }

        /// <summary>
        /// Casts the elements of an IEnumerable to the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to cast the elements of source to.</typeparam>
        /// <param name="source">The <see cref="IEnumerable"/> that contains the elements to be cast to type TResult.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{T}"/> that contains each element of the source sequence cast to the specified type.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type TResult.</exception>
        public static IEnumerable<TResult> CastTo<TResult>(this IEnumerable source)
        {
            if (source is null)
            {
                throw new ArgumentNullException($"{nameof(source)} cannot be null!");
            }

            foreach (var item in source)
            {
                if (!(item is TResult))
                {
                    throw new InvalidCastException($"Impossible to convert {item.GetType()} to {typeof(TResult)}");
                }
            }

            return DoCastTo<TResult>(source);
        }

        /// <summary>
        /// The method-generator of sequence of integers according some predicate.
        /// </summary>
        /// <param name="start">Some integer initial value.</param>
        /// <param name="predicate">Predicate for sequence.</param>
        /// <returns>The sequence of integer numbers.</returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="predicate"/> is null.</exception>
        public static IEnumerable<int> Generate(int start, int count, Func<int, int> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException($"{nameof(predicate)} cannot be null!");
            }

            return DoGenerate(start, count, predicate);
        }

        /// <summary>
        /// Determines whether all elements of a sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///     true if every element of the source sequence passes the test in the specified predicate,
        ///     or if the sequence is empty; otherwise, false
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="predicate"/> is null.</exception>
        public static bool ForAll<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Validate(source, predicate);
            return DoForAll(source, predicate);
        } 
        #endregion

        #region Private Methods
        private static bool DoForAll<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                if (!predicate(item))
                {
                    return false;
                }
            }

            return true;
        }

        private static IEnumerable<TResult> DoCastTo<TResult>(IEnumerable source)
        {
            foreach (var item in source)
            {
                yield return (TResult)item;
            }
        }

        private static void Validate<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> predicate)
        {
            if (source is null)
            {
                throw new ArgumentNullException($"{nameof(source)} cannot be null!");
            }

            if (predicate is null)
            {
                throw new ArgumentNullException($"{nameof(predicate)} cannot be null!");
            }
        }

        private static IEnumerable<TSource> DoFilter<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<TResult> DoTransform<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> transformer)
        {
            foreach (var item in source)
            {
                yield return transformer(item);
            }
        }

        private static IEnumerable<TSource> DoSortBy<TSource, TKey>(SortedDictionary<TKey, List<TSource>> sDictionary, IEnumerable<TSource> source, Func<TSource, TKey> key)
        {
            foreach (var item in source)
            {
                if (!sDictionary.ContainsKey(key(item)))
                {
                    sDictionary.Add(key(item), new List<TSource>() { item });
                }
                else
                {
                    sDictionary[key(item)].Add(item);
                }
            }

            foreach (var itemList in sDictionary.Values)
            {
                foreach (var item in itemList)
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<int> DoGenerate(int start, int count, Func<int, int> predicate)
        {
            for (int i = start; i < start + count; i++)
            {
                yield return predicate(i);
            }
        } 
        #endregion
    }
}