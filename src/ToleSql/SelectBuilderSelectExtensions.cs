using System;
using System.Linq.Expressions;

namespace ToleSql
{
    public static class SelectBuilderSelectExtensions
    {
        public static SelectBuilder Select<TEntity>(this SelectBuilder builder,
            params Expression<Func<TEntity, object>>[] expressions)
        {
            return builder.Select(new[] { typeof(TEntity) }, expressions);
        }

        public static SelectBuilder Select<TEntity1, TEntity2>(this SelectBuilder builder,
            params Expression<Func<TEntity1, TEntity2, object>>[] expressions)
        {
            return builder.Select(new[] { typeof(TEntity1), typeof(TEntity2) }, expressions);
        }

    }
}