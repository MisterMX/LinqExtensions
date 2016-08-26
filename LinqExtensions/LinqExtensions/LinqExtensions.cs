using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqExtensions
{
    /// <summary>
    /// Contains extension methods for LINQ to entities.
    /// </summary>
    public static class LinqExtensions
    {
        #region Extension methods

        /// <summary>
        /// Performs a left outer join between an <see cref="IQueryable{TLeft}"/> and an
        /// <see cref="IEnumerable{TRight}"/>.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left collection.</typeparam>
        /// <typeparam name="TRight">The type of the right collection.</typeparam>
        /// <typeparam name="TKey">The type of the key that is used for the join.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="outer">The left collection.</param>
        /// <param name="inner">The right collection.</param>
        /// <param name="outerKeySelector">The key selector expression for the left collection.</param>
        /// <param name="innerKeySelector">The key selector expression for the right collection.</param>
        /// <param name="resultSelector">The result selector expression.</param>
        /// <returns>the resulting <see cref="IQueryable{TResult}"/>.</returns>
        public static IQueryable<TResult> LeftOuterJoin<TLeft, TRight, TKey, TResult>(
            this IQueryable<TLeft> outer,
            IEnumerable<TRight> inner,
            Expression<Func<TLeft, TKey>> outerKeySelector,
            Expression<Func<TRight, TKey>> innerKeySelector,
            Expression<Func<JoinResult<TLeft, TRight>, TResult>> resultSelector)
        {
            IQueryable<GroupJoinResult<TLeft, TRight>> groupJoinResult = outer
                .GroupJoin(
                    inner,
                    outerKeySelector,
                    innerKeySelector,
                    (o, i) => new GroupJoinResult<TLeft, TRight>
                    {
                        Key = o,
                        Values = i
                    });

            IQueryable<TResult> leftOuterJoinResults = groupJoinResult
                .SelectMany(
                    x => x.Values.Select(c => new JoinResult<TLeft, TRight>
                    {
                        Left = x.Key,
                        Right = c
                    }))
                .Select(resultSelector);

            return leftOuterJoinResults;
        }


        /// <summary>
        /// Performs a left outer join between an <see cref="IEnumerable{TLeft}"/> and an
        /// <see cref="IEnumerable{TRight}"/>.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left collection.</typeparam>
        /// <typeparam name="TRight">The type of the right collection.</typeparam>
        /// <typeparam name="TKey">The type of the key that is used for the join.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="outer">The left collection.</param>
        /// <param name="inner">The right collection.</param>
        /// <param name="outerKeySelector">The key selector expression for the left collection.</param>
        /// <param name="innerKeySelector">The key selector expression for the right collection.</param>
        /// <param name="resultSelector">The result selector expression.</param>
        /// <returns>the resulting <see cref="IQueryable{TResult}"/>.</returns>
        public static IEnumerable<TResult> LeftOuterJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> outer,
            IEnumerable<TRight> inner,
            Func<TLeft, TKey> outerKeySelector,
            Func<TRight, TKey> innerKeySelector,
            Func<JoinResult<TLeft, TRight>, TResult> resultSelector)
        {
            IEnumerable<GroupJoinResult<TLeft, TRight>> groupJoinResult = outer
                .GroupJoin(
                    inner,
                    outerKeySelector,
                    innerKeySelector,
                    (o, i) => new GroupJoinResult<TLeft, TRight>
                    {
                        Key = o,
                        Values = i
                    });

            IEnumerable<TResult> leftOuterJoinResults = groupJoinResult
                .SelectMany(
                    x => x.Values.Select(c => new JoinResult<TLeft, TRight>
                    {
                        Left = x.Key,
                        Right = c
                    }))
                .Select(resultSelector);

            return leftOuterJoinResults;
        }

        #endregion
    }
}
