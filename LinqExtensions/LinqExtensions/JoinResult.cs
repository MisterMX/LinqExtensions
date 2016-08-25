namespace LinqExtensions
{
    /// <summary>
    /// Represents the result of a join.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    public class JoinResult<TLeft, TRight>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the left value of the current <see cref="JoinResult{TLeft, TRight}"/>.
        /// </summary>
        public TLeft Left { get; set; }

        /// <summary>
        /// Gets or sets the right value of the current <see cref="JoinResult{TLeft, TRight}"/>.
        /// </summary>
        public TRight Right { get; set; }

        #endregion
    }
}
