using System;
using System.Linq.Expressions;
using ToleSql.SqlBuilder;

namespace ToleSql
{
    public static class SelectBuilderHavingExtensions
    {
        public static SelectBuilder Having<TEntity>(this SelectBuilder builder,
            Expression<Func<TEntity, bool>> expression)
        {
            return builder.Having(WhereOperator.And, expression);
        }
        public static SelectBuilder Having<TEntity>(this SelectBuilder builder,
            WhereOperator preOperator, Expression<Func<TEntity, bool>> expression)
        {
            return builder.Having(preOperator, new[] { typeof(TEntity) }, expression);
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