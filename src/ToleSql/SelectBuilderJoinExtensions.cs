using System;
using System.Linq.Expressions;
using ToleSql.SqlBuilder;

namespace ToleSql
{
    public static class SelectBuilderJoinExtensions
    {
        public static SelectBuilder Join<TEntity1, TEntity2>(this SelectBuilder builder,
            Expression<Func<TEntity1, TEntity2, bool>> condition)
        {
            return builder.Join(JoinType.Inner, builder.GetNextTableAlias(), condition);
        }
        public static SelectBuilder Join<TEntity1, TEntity2>(this SelectBuilder builder,
            string alias, Expression<Func<TEntity1, TEntity2, bool>> condition)
        {
            return builder.Join(JoinType.Inner, alias, condition);
        }
        public static SelectBuilder Join<TEntity1, TEntity2>(this SelectBuilder builder,
            JoinType joinType, Expression<Func<TEntity1, TEntity2, bool>> condition)
        {
            return builder.Join(joinType, builder.GetNextTableAlias(), condition);
        }

        public static SelectBuilder Join<TEntity, TJoin>(this SelectBuilder builder,
            JoinType joinType, string alias, Expression<Func<TEntity, TJoin, bool>> condition)
        {
            return builder.Join(joinType, alias, typeof(TJoin), new[] { typeof(TEntity) }, condition);
        }

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

    }
}