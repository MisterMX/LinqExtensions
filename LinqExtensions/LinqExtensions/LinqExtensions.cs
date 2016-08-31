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
        #region Left outer join

        /// <summary>
        /// Performs a left outer join between an <see cref="IQueryable{TLeft}"/> and an
        /// <see cref="IEnumerable{TRight}"/>.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left collection.</typeparam>
        /// <typeparam name="TRight">The type of the right collection.</typeparam>
        /// <typeparam name="TKey">The type of the key that is used for the join.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="left">The left collection.</param>
        /// <param name="right">The right collection.</param>
        /// <param name="leftKeySelector">The key selector expression for the left collection.</param>
        /// <param name="rightKeySelector">The key selector expression for the right collection.</param>
        /// <param name="resultSelector">The result selector expression.</param>
        /// <returns>the resulting <see cref="IQueryable{TResult}"/>.</returns>
        public static IQueryable<TResult> LeftOuterJoin<TLeft, TRight, TKey, TResult>(
            this IQueryable<TLeft> left,
            IEnumerable<TRight> right,
            Expression<Func<TLeft, TKey>> leftKeySelector,
            Expression<Func<TRight, TKey>> rightKeySelector,
            Expression<Func<JoinResult<TLeft, TRight>, TResult>> resultSelector)
        {
            IQueryable<GroupJoinResult<TLeft, TRight>> groupJoinResult = left
                .GroupJoin(
                    right,
                    leftKeySelector,
                    rightKeySelector,
                    (l, r) => new GroupJoinResult<TLeft, TRight>
                    {
                        Key = l,
                        Values = r
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
        /// <param name="left">The left collection.</param>
        /// <param name="right">The right collection.</param>
        /// <param name="leftKeySelector">The key selector expression for the left collection.</param>
        /// <param name="rightKeySelector">The key selector expression for the right collection.</param>
        /// <param name="resultSelector">The result selector expression.</param>
        /// <returns>the resulting <see cref="IQueryable{TResult}"/>.</returns>
        public static IEnumerable<TResult> LeftOuterJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<JoinResult<TLeft, TRight>, TResult> resultSelector)
        {
            IEnumerable<GroupJoinResult<TLeft, TRight>> groupJoinResult = left
                .GroupJoin(
                    right,
                    leftKeySelector,
                    rightKeySelector,
                    (l, r) => new GroupJoinResult<TLeft, TRight>
                    {
                        Key = l,
                        Values = r
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

        #region Full outer join

        /// <summary>
        /// Performs a full outer join between an <see cref="IQueryable{TLeft}"/> and an
        /// <see cref="IQueryable{TRight}"/>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TLeft">The type of the left collection.</typeparam>
        /// <typeparam name="TRight">The type of the right collection.</typeparam>
        /// <typeparam name="TKey">The type of the key that is used for the join.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="left">The left collection.</param>
        /// <param name="right">The right collection.</param>
        /// <param name="leftKeySelector">The key selector expression for the left collection.</param>
        /// <param name="rightKeySelector">The key selector expression for the right collection.</param>
        /// <param name="resultSelector">The result selector expression.</param>
        /// <returns>the resulting <see cref="IQueryable{TResult}"/>.</returns>
        public static IQueryable<TResult> FullOuterJoin<TLeft, TRight, TKey, TResult>(
            this IQueryable<TLeft> left,
            IQueryable<TRight> right,
            Expression<Func<TLeft, TKey>> leftKeySelector,
            Expression<Func<TRight, TKey>> rightKeySelector,
            Expression<Func<JoinResult<TLeft, TRight>, TResult>> resultSelector)
        {
            IQueryable<TResult> leftOuterJoinResult = left.LeftOuterJoin(right, leftKeySelector, rightKeySelector, resultSelector);

            // The left and right values are swapped - revert it!
            IQueryable<TResult> rightOuterJoinResult = right
                .LeftOuterJoin(
                    left,
                    rightKeySelector,
                    leftKeySelector,
                    jr => new JoinResult<TLeft, TRight>
                    {
                        Left = jr.Right,
                        Right = jr.Left
                    })
                .Select(resultSelector);

            return leftOuterJoinResult.Union(rightOuterJoinResult);
        }

        /// <summary>
        /// Performs a full outer join between an <see cref="IEnumerable{TLeft}"/> and an
        /// <see cref="IEnumerable{TRight}"/>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TLeft">The type of the left collection.</typeparam>
        /// <typeparam name="TRight">The type of the right collection.</typeparam>
        /// <typeparam name="TKey">The type of the key that is used for the join.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="left">The left collection.</param>
        /// <param name="right">The right collection.</param>
        /// <param name="leftKeySelector">The key selector expression for the left collection.</param>
        /// <param name="rightKeySelector">The key selector expression for the right collection.</param>
        /// <param name="resultSelector">The result selector expression.</param>
        /// <returns>the resulting <see cref="IQueryable{TResult}"/>.</returns>
        public static IEnumerable<TResult> FullOuterJoin<TLeft, TRight, TKey, TResult>(
            this IEnumerable<TLeft> left,
            IEnumerable<TRight> right,
            Func<TLeft, TKey> leftKeySelector,
            Func<TRight, TKey> rightKeySelector,
            Func<JoinResult<TLeft, TRight>, TResult> resultSelector)
        {
            IEnumerable<TResult> leftOuterJoinResult = left.LeftOuterJoin(right, leftKeySelector, rightKeySelector, resultSelector);

            // The left and right values are swapped - revert it!
            IEnumerable<TResult> rightOuterJoinResult = right
                .LeftOuterJoin(
                    left,
                    rightKeySelector,
                    leftKeySelector,
                    jr => new JoinResult<TLeft, TRight>
                    {
                        Left = jr.Right,
                        Right = jr.Left
                    })
                .Select(resultSelector);

            return leftOuterJoinResult.Union(rightOuterJoinResult);
        }

        #endregion
    }
}
