/* For original source code see
 * https://blogs.msdn.microsoft.com/meek/2008/05/02/linq-to-entities-combining-predicates/
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqExtensions.Expressions
{
    /// <summary>
    /// Contains extension methods for LINQ expressions.
    /// </summary>
    public static class ExpressionExtensions
    {
        #region Extension methods

        /// <summary>
        /// Composes two <see cref="Expression{T}"/> using another <see cref="Expression"/>.
        /// </summary>
        /// <typeparam name="TDelegate">the delegate type.</typeparam>
        /// <param name="first">The first <see cref="Expression{TDelegate}"/>.</param>
        /// <param name="second">The second <see cref="Expression{TDelegate}"/>.</param>
        /// <param name="merge">
        /// The <see cref="Expression"/> that is used to combine the first and the second <see cref="Expression{TDelegate}"/>.
        /// </param>
        /// <returns>the composed <see cref="Expression{TDelegate}"/>.</returns>
        public static Expression<TDelegate> Compose<TDelegate>(
            this Expression<TDelegate> first,
            Expression<TDelegate> second,
            Func<Expression, Expression, Expression> merge)
        {
            // Build parameter map (from parameters of second to parameters of first)
            Dictionary<ParameterExpression, ParameterExpression> map = first.Parameters
                .Select((f, i) => new
                {
                    f, s = second.Parameters[i]
                })
                .ToDictionary(
                    p => p.s,
                    p => p.f);

            // Replace parameters in the second lambda expression with parameters from the first
            Expression secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // Apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<TDelegate>(merge(first.Body, secondBody), first.Parameters);
        }


        /// <summary>
        /// Combines the following <see cref="Expression{TDelegate}"/>'s with an 'and' operator.
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="first">The first <see cref="Expression{TDelegate}"/>.</param>
        /// <param name="second">The second <see cref="Expression{TDelegate}"/>.</param>
        /// <returns>the composed <see cref="Expression{TDelegate}"/>.</returns>
        public static Expression<Func<TDelegate, bool>> And<TDelegate>(
            this Expression<Func<TDelegate, bool>> first,
            Expression<Func<TDelegate, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }


        /// <summary>
        /// Combines the following <see cref="Expression{TDelegate}"/>'s with an 'and' operator.
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="first">The first <see cref="Expression{TDelegate}"/>.</param>
        /// <param name="second">The second <see cref="Expression{TDelegate}"/>.</param>
        /// <returns>the composed <see cref="Expression{TDelegate}"/>.</returns>
        public static Expression<Func<TDelegate, bool>> Or<TDelegate>(
            this Expression<Func<TDelegate, bool>> first,
            Expression<Func<TDelegate, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        #endregion
    }
}
