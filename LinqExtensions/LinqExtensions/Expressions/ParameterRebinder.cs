/* For original source code see
 * https://blogs.msdn.microsoft.com/meek/2008/05/02/linq-to-entities-combining-predicates/
 */
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqExtensions.Expressions
{
    /// <summary>
    /// Provides the ability of rebinding the parameters of an <see cref="Expression"/>.
    /// </summary>
    internal class ParameterRebinder : ExpressionVisitor
    {
        #region Fields

        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
        /// </summary>
        /// <param name="map">
        /// The <see cref="ParameterExpression"/>s of the first <see cref="Expression"/> mapped to them of
        /// the second.
        /// </param>
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Replace the parameters of the following expression with them of another.
        /// </summary>
        /// <param name="map">
        /// The <see cref="ParameterExpression"/>s of the parameter <see cref="Expression"/> mapped to other paramters.
        /// </param>
        /// <param name="exp">The <see cref="Expression"/> whose parameters should be replaced.</param>
        /// <returns>the <see cref="Expression"/> with the replaced parameters.<see cref=""/></returns>
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        #endregion

        #region Overriden methods

        /// <summary>
        /// Visits the <see cref="ParameterExpression"/>.
        /// </summary>
        /// <param name="p">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified;
        /// otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }

        #endregion
    }
}
