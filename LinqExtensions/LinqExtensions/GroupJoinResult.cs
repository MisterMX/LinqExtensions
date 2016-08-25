using System.Collections.Generic;

namespace LinqExtensions
{
    /// <summary>
    /// Represents the result of an group join.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the values.</typeparam>
    public class GroupJoinResult<TKey, TValue>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the key of the current <see cref="GroupJoinResult{TKey, TValue}"/>.
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// Gets or sets the values of the current <see cref="GroupJoinResult{TKey, TValue}"/>.
        /// </summary>
        public IEnumerable<TValue> Values { get; set; }

        #endregion
    }
}
