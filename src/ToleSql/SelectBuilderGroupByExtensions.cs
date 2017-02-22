using System;
using System.Linq.Expressions;

namespace ToleSql
{
    public static class SelectBuilderGroupByExtensions
    {
        public static SelectBuilder GroupBy<TEntity>(this SelectBuilder builder,
            params Expression<Func<TEntity, object>>[] expressions)
        {
            return builder.GroupBy(new[] { typeof(TEntity) }, expressions);
        }

        public static SelectBuilder GroupBy<TEntity1, TEntity2>(this SelectBuilder builder,
            params Expression<Func<TEntity1, TEntity2, object>>[] expressions)
        {
            return builder.GroupBy(new[] { typeof(TEntity1), typeof(TEntity2) }, expressions);
        }

    }
}