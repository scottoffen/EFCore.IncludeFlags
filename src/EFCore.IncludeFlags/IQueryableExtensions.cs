namespace EFCore.IncludeFlags
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> IncludeFlags<TEntity, TEnum>(this IQueryable<TEntity> queryable, TEnum? flags) where TEntity : class where TEnum : Enum
        {
            if (flags ==  null) return queryable;

            try
            {
                return FlagIncluder<TEntity, TEnum>.AddIncludes(queryable, flags);
            }
            catch (TypeInitializationException e)
            {
                throw (e.InnerException != null)
                    ? e.InnerException
                    : e;
            }
        }
    }
}
