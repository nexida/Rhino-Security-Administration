using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace Rhino.Security.Mgmt.Infrastructure
{
    /// <summary>
    /// Wraps an IQueryable object, to expose only the functionalities provided by <see cref="IPresentableSet{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryablePresentableSet<T> : IPresentableSet<T>
    {
        private readonly IQueryable<T> _queryable;

        public QueryablePresentableSet(IQueryable<T> queryable)
        {
            if(queryable == null)
            {
                throw new ArgumentNullException("queryable");
            }
            _queryable = queryable;
        }

        /// <summary>
        /// Returns an enumerable representation of the set.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> AsEnumerable()
        {
            return _queryable.AsEnumerable();
        }

        /// <summary>
        /// Sorts the set.
        /// </summary>
        /// <param name="sortBy">The field to sort by.</param>
        /// <param name="sortAscending">if set to <c>true</c> [sort ascending].</param>
        /// <returns></returns>
        public IPresentableSet<T> Sort(string sortBy, bool sortAscending)
        {
            if(string.IsNullOrEmpty(sortBy))
            {
                return new QueryablePresentableSet<T>(_queryable);
            }
            string ordering = sortAscending ? " asc" : " desc";
            return new QueryablePresentableSet<T>(_queryable.OrderBy(sortBy + ordering));
        }

        /// <summary>
        /// Skips as many rows as indicated by <paramref name="startRowIndex"/>. It is used for paging.
        /// </summary>
        /// <param name="startRowIndex">Start index of the rows to be returned in the result set.</param>
        /// <returns></returns>
        public IPresentableSet<T> Skip(int startRowIndex)
        {
            return new QueryablePresentableSet<T>(_queryable.Skip(startRowIndex));
        }

        /// <summary>
        /// Takes only as many rows as indicated by <paramref name="maxRows"/>. It is used for paging.
        /// </summary>
        /// <param name="maxRows">The max number of rows to be returned in the result set.</param>
        /// <returns></returns>
        public IPresentableSet<T> Take(int maxRows)
        {
            return new QueryablePresentableSet<T>(_queryable.Take(maxRows));
        }

        /// <summary>
        /// Return the total number of rows in the set.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _queryable.Count();
        }
    }
}