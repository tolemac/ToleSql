using System;
using System.Linq.Expressions;
using ToleSql.SqlBuilder;

namespace ToleSql
{
    public class SelectFrom<TEntity1, TEntity2> : SelectFrom<TEntity1>
    {
        internal SelectFrom(SelectBuilder builder) : base(builder)
        {
            Builder = builder;
        }
        public SelectFrom<TEntity1, TEntity2> Where(Expression<Func<TEntity1, TEntity2, bool>> predicate)
        {
            Builder.Where<TEntity1, TEntity2>(predicate);
            return this;
        }
        public SelectFrom<TEntity1, TEntity2> Select(Expression<Func<TEntity1, TEntity2, object>> selector)
        {
            Builder.Select<TEntity1, TEntity2>(selector);
            return this;
        }

        public SelectFrom<TEntity2, TNewEntity> Join<TNewEntity>(Expression<Func<TEntity2, TNewEntity, bool>> condition)
        {
            Builder.AddJoin<TEntity2, TNewEntity>(condition);
            return new SelectFrom<TEntity2, TNewEntity>(Builder);
        }

        public SelectFrom<TEntity1, TEntity2> OrderBy(Expression<Func<TEntity1, TEntity2, Object>> keySelector)
        {
            Builder.OrderBy<TEntity1, TEntity2>(OrderByDirection.Asc, keySelector);
            return this;
        }
        public SelectFrom<TEntity1, TEntity2> OrderByDescending(Expression<Func<TEntity1, TEntity2, object>> keySelector)
        {
            Builder.OrderBy<TEntity1, TEntity2>(OrderByDirection.Desc, keySelector);
            return this;
        }
        public SelectFrom<TEntity1, TEntity2> GroupBy(Expression<Func<TEntity1, TEntity2, object>> keySelector)
        {
            Builder.GroupBy<TEntity1, TEntity2>(keySelector);
            return this;
        }
        public SelectFrom<TEntity1, TEntity2> Having(Expression<Func<TEntity1, TEntity2, bool>> condition)
        {
            Builder.Having<TEntity1, TEntity2>(condition);
            return this;
        }
    }
}