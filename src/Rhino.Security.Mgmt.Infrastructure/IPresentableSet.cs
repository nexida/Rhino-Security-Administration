using System.Collections.Generic;

namespace Rhino.Security.Mgmt.Infrastructure
{
	/// <summary>
	/// Interface implemented to return collections of objects (collections, sets, lists, etc.) to the user interface tier.  
	/// On the user interface tier only counting, sorting and paging should be allowed.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IPresentableSet<T>
	{
		/// <summary>
		/// Returns an enumerable representation of the set.
		/// </summary>
		/// <returns></returns>
		IEnumerable<T> AsEnumerable();

		/// <summary>
		/// Sorts the set.
		/// </summary>
		/// <param name="sortBy">The field to sort by.</param>
		/// <param name="sortAscending">if set to <c>true</c> [sort ascending].</param>
		/// <returns></returns>
		IPresentableSet<T> Sort(string sortBy, bool sortAscending);

		/// <summary>
		/// Skips as many rows as indicated by <paramref name="startRowIndex"/>. It is used for paging.
		/// </summary>
		/// <param name="startRowIndex">Start index of the rows to be returned in the result set.</param>
		/// <returns></returns>
		IPresentableSet<T> Skip(int startRowIndex);

		/// <summary>
		/// Takes only as many rows as indicated by <paramref name="maxRows"/>. It is used for paging.
		/// </summary>
		/// <param name="maxRows">The max number of rows to be returned in the result set.</param>
		/// <returns></returns>
		IPresentableSet<T> Take(int maxRows);

		/// <summary>
		/// Return the total number of rows in the set.
		/// </summary>
		/// <returns></returns>
		int Count();
	}
}