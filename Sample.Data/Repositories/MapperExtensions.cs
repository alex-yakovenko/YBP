using AutoMapper;

namespace Sample.Data
{
    public static class MapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> MapOnlyIfDirty<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map)
        {
            map.ForAllMembers(source =>
            {
                source.Condition((s, d, sv, dv, rc) =>
                {
                    if (sv == null)
                        return dv != null;
                    return !sv.Equals(dv);
                });
            });
            return map;
        }
    }
}
