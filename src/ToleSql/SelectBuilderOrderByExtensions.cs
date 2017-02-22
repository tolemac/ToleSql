using System;
using System.Linq.Expressions;
using ToleSql.SqlBuilder;

namespace ToleSql
{
    public static class SelectBuilderOrderByExtensions
    {
        public static SelectBuilder OrderBy<TEntity>(this SelectBuilder builder,
            params Expression<Func<TEntity, object>>[] expressions)
        {
            return builder.OrderBy(OrderByDirection.Asc, expressions);
        }

        public static SelectBuilder OrderBy<TEntity>(this SelectBuilder builder,
            OrderByDirection direction, params Expression<Func<TEntity, object>>[] expressions)
        {
            return builder.OrderBy(direction, new[] { typeof(TEntity) }, expressions);
        }

        public static SelectBuilder OrderBy<TEntity1, TEntity2>(this SelectBuilder builder,
            params Expression<Func<TEntity1, TEntity2, object>>[] expressions)
        {
            return builder.OrderBy(OrderByDirection.Asc, expressions);
        }

        public static SelectBuilder OrderBy<TEntity1, TEntity2>(this SelectBuilder builder,
            OrderByDirection direction, params Expression<Func<TEntity1, TEntity2, object>>[] expressions)
        {
            return builder.OrderBy(direction, new[] { typeof(TEntity1), typeof(TEntity2) }, expressions);
        }
    }
}