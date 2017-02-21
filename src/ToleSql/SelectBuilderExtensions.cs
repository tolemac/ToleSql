using System;
using System.Linq.Expressions;
using ToleSql.SqlBuilder;

namespace ToleSql
{
    public static class SelectBuilderExtensions
    {
        public static SelectBuilder Join<TEntity1, TEntity2, TJoin>(this SelectBuilder builder,
            string alias, Expression<Func<TEntity1, TEntity2, TJoin, bool>> condition)
        {
            return builder.Join(JoinType.Inner, alias, condition);
        }
        public static SelectBuilder Join<TEntity1, TEntity2, TJoin>(this SelectBuilder builder,
            Expression<Func<TEntity1, TEntity2, TJoin, bool>> condition)
        {
            return builder.Join(JoinType.Inner, builder.GetNextTableAlias(), condition);
        }
        public static SelectBuilder Join<TEntity1, TEntity2, TJoin>(this SelectBuilder builder,
            JoinType joinType, string alias, Expression<Func<TEntity1, TEntity2, TJoin, bool>> condition)
        {
            return builder.Join(joinType, alias, typeof(TJoin),
                new[] { typeof(TEntity1), typeof(TEntity2) }, condition);
        }
        public static SelectBuilder Select<TEntity1, TEntity2>(this SelectBuilder builder,
            params Expression<Func<TEntity1, TEntity2, object>>[] expressions)
        {
            return builder.Select(new[] { typeof(TEntity1), typeof(TEntity2) }, expressions);
        }
        public static SelectBuilder Where<TEntity1, TEntity2>(this SelectBuilder builder,
            Expression<Func<TEntity1, TEntity2, bool>> expression)
        {
            return builder.Where(WhereOperator.And, expression);
        }

        public static SelectBuilder Where<TEntity1, TEntity2>(this SelectBuilder builder,
            WhereOperator preOperator, Expression<Func<TEntity1, TEntity2, bool>> expression)
        {
            return builder.Where(preOperator, new[] { typeof(TEntity1), typeof(TEntity2) }, expression);
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
        public static SelectBuilder GroupBy<TEntity1, TEntity2>(this SelectBuilder builder,
            params Expression<Func<TEntity1, TEntity2, object>>[] expressions)
        {
            return builder.GroupBy(new[] { typeof(TEntity1), typeof(TEntity2) }, expressions);
        }
        public static SelectBuilder Having<TEntity1, TEntity2>(this SelectBuilder builder,
            Expression<Func<TEntity1, TEntity2, bool>> expression)
        {
            return builder.Having(WhereOperator.And, expression);
        }
        public static SelectBuilder Having<TEntity1, TEntity2>(this SelectBuilder builder,
            WhereOperator preOperator, Expression<Func<TEntity1, TEntity2, bool>> expression)
        {
            return builder.Having(preOperator, new[] { typeof(TEntity1), typeof(TEntity2) }, expression);
        }
    }
}