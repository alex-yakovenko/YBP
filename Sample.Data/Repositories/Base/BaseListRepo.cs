using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Sample.Definitions.Common;

namespace Sample.Data.Repositories.Base
{
    public abstract class BaseListRepo<TDataObject, TFilter, TOrder, TKey> :
        BaseRepo<TDataObject, TKey>,
        IListSource<TFilter, TOrder, TKey> 
        where TDataObject : class, IEntity<TKey>, new()
        where TFilter : ListFilterBase<TKey>
        where TOrder : struct


    {
        protected BaseListRepo(SampleDbContext db) : base(db)
        {

        }

        protected virtual IQueryable<TDataObject> ApplyFilter(IQueryable<TDataObject> qry, TFilter filter)
        {
            if (filter == null)
                return qry;

            if (filter.ExceptIds != null && filter.ExceptIds.Any())
                qry = qry.Where(x => !filter.ExceptIds.Contains(x.Id));

            if (filter.Ids != null)
                qry = qry.Where(x => filter.Ids.Contains(x.Id));

            return qry;
        }

        public virtual int GetCount(TFilter filter = default(TFilter))
        {
            var qry = All();

            if (filter != default(TFilter))
                qry = ApplyFilter(qry, filter);

            return qry.Count();
        }

        public TListItem[] GetList<TListItem>(TFilter filter = default(TFilter), int skipCount = 0, int takeCount = Int32.MaxValue,
            TOrder? order = null, bool desc = false, Dictionary<TOrder, bool> orders = null)
        {
            var qry = All();

            if (filter != default(TFilter))
                qry = ApplyFilter(qry, filter);
            qry = ApplyOrder(qry, order, desc, orders);

            if (skipCount > 0)
                qry = qry.Skip(skipCount);

            if (takeCount != int.MaxValue)
                qry = qry.Take(takeCount);

            return qry
                .ProjectTo<TListItem>()
                .ToArray();
        }

        protected virtual IQueryable<TDataObject> ApplyExpression(IQueryable<TDataObject> qry, TOrder order, bool first, bool desc)
        {
            return qry.OrderFunction(x => x.Id, first, desc);
        }

        private IQueryable<TDataObject> ApplyOrder(IQueryable<TDataObject> qry, TOrder? order, bool desc, Dictionary<TOrder, bool> orders)
        {
            // Create new instance of dictionary to not to affect other code which depends on the dictionary in parameter
            orders = orders?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<TOrder, bool>();

            if (order != null)
                orders[order.Value] = desc;

            if (!orders.Any())
            {
                qry = qry.OrderBy(x => x.Id);
                return qry;
            }

            var first = true;

            foreach (var sort in orders)
            {
                qry = ApplyExpression(qry, sort.Key, first, sort.Value);
                first = false;
            }

            return qry;
        }


        public bool Any(TFilter filter = default(TFilter))
        {
            var qry = All();
            if (filter != default(TFilter))
                qry = ApplyFilter(qry, filter);
            return qry.Any();
        }

        public TListItem GetFirst<TListItem>(TFilter filter = default(TFilter))
        {
            var qry = All();
            if (filter != default(TFilter))
                qry = ApplyFilter(qry, filter);
            return qry
                .ProjectTo<TListItem>()
                .FirstOrDefault();
        }
    }

    public static class RepoHelper
    {
        public static IOrderedQueryable<TDataObject> OrderFunction<TDataObject, TKey>(this IQueryable<TDataObject> qry,
    Expression<Func<TDataObject, TKey>> keySelector, bool first, bool desc)
        {
            return first
                ? desc
                    ? qry.OrderByDescending(keySelector)
                    : qry.OrderBy(keySelector)
                : desc
                    ? ((IOrderedQueryable<TDataObject>)qry).ThenByDescending(keySelector)
                    : ((IOrderedQueryable<TDataObject>)qry).ThenBy(keySelector);
        }
    }

}