using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Pirina.Kernel.Data;

namespace Pirina.Providers.Databases.AzureCosmosDatabase
{
    public interface ITransitDbContext
    {
        /// <summary>
        /// Sets this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> Set<T>() where T : BaseTransactionModel;

        /// <summary>
        /// Set an item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> Set<T>(Expression<Func<T, bool>> predicate) where T : BaseTransactionModel;

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task<T> Add<T>(T item) where T : BaseTransactionModel;

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool Remove<T>(T item) where T : BaseTransactionModel;
    }
}