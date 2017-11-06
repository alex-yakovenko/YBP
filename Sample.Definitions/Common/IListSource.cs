
using System.Collections.Generic;

namespace Sample.Definitions.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TFilter">Type which describes possible filtration criterias</typeparam>
    /// <typeparam name="TOrder">Enum which contains possible sort types</typeparam>
    /// <typeparam name="TKey">Type of key value of the entity</typeparam>
    public interface IListSource<TFilter, TOrder, TKey>
        where TFilter: ListFilterBase<TKey>
        where TOrder: struct
    {
        int GetCount(TFilter filter = null);

        /// <summary>
        /// Returns list of entities
        /// </summary>
        /// <typeparam name="TListItem"></typeparam>
        /// <param name="filter">Filter criterias</param>
        /// <param name="skipCount">Number of entities to skip</param>
        /// <param name="takeCount">Number of entities to return</param>
        /// <param name="order">Sort order if it is single. This parameter is ignored if <b>orders</b> parameter is not null.</param>
        /// <param name="desc">true order by descending. The value is used only if single sort order is specified. If <b>orders</b> parameter is not null, <b>desc</b> parameter is ignored.</param>
        /// <param name="orders">Allows to specify several sort orders. keys of the dictionary are sort types, values are directions (<b>true</b> for descending). If this parameter is specified, <b>order</b> and <b>desc</b> parameters are ignored</param>
        /// <returns></returns>
        TListItem[] GetList<TListItem>(
            TFilter filter = null, 
            int skipCount = 0, 
            int takeCount = int.MaxValue,
            TOrder? order = null, 
            bool desc = false,
            Dictionary<TOrder, bool> orders = null);

        bool Any(TFilter filter = default(TFilter));

        TListItem GetFirst<TListItem>(TFilter filter);
    }
}