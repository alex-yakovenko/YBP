using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sample.Definitions.Common;

namespace Sample.Data.Repositories.Base
{
    public abstract class BaseRepo<TDataObject, TKey> : 
        IRepo<TKey>
        where TDataObject : class, IEntity<TKey>, new()
    {
        protected readonly SampleDbContext Db;

        protected BaseRepo(SampleDbContext db)
        {
            Db = db;
        }

        protected virtual IQueryable<TDataObject> All()
        {
            IQueryable<TDataObject> qry = Db.Set<TDataObject>();
            return qry;
        }

        public TResultEntity Get<TResultEntity>(TKey id) 
        {
            var ids = new[] {id};

            return All()
                .Where(x => ids.Contains(x.Id))
                .ProjectTo<TResultEntity>()
                .FirstOrDefault();
        }

        public virtual BoundItem[] GetBoundItems(TKey id)
        {
            return new BoundItem[] {};
        }

        public void Delete(TKey id)
        {
            var entity = Db
                .Set<TDataObject>()
                .Find(id);

            if (entity == null)
                return;

            Db.Set<TDataObject>().Remove(entity);
            Db.SaveChanges();
        }

        public TKey Save<TUpdateInfo>(TUpdateInfo item) 
            where TUpdateInfo : class, IEntity<TKey>, new()

        {
            var data = !Equals(item.Id, default(TKey))
                ? Db.Set<TDataObject>().Find(item.Id)
                : new TDataObject();

            if (data == null)
                return default(TKey);

            data = Mapper.Map<TUpdateInfo, TDataObject>(item, data);

            if (Equals(item.Id, default(TKey)))
            {
                Db.Set<TDataObject>().Add(data);
            }

            Db.SaveChanges();

            return data.Id;
        }

        public virtual CanDeleteCheckResult CheckCanDelete(int id)
        {
            return new CanDeleteCheckResult
            {
                CanDelete = true
            };
        }

    }
}
