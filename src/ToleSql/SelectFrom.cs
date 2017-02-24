using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ToleSql.SqlBuilder;

namespace ToleSql
{
    public class SelectFrom : IBuilder
    {
        public SelectBuilder Builder { get; protected set; }
        public IDictionary<string, object> Parameters { get { return Builder.Parameters; } }
        public SelectFrom()
        {
            Builder = new SelectBuilder();
        }
        public SelectFrom<TEntity> Cast<TEntity>()
        {
            return new SelectFrom<TEntity>(Builder);
        }

        public string GetSqlText()
        {
            return Builder.GetSqlText();
        }
    }
    public class SelectFrom<TEntity> : SelectFrom
    {
        public SelectFrom() : base()
        {
            SetMain();
        }

        internal SelectFrom(SelectBuilder builder)
        {
            Builder = builder;
            SetMain();
        }

        private void SetMain()
        {
            if (Builder.MainSourceSql == null)
            {
                Builder.From<TEntity>();
            }
        }

        public SelectFrom<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            Builder.Where<TEntity>(predicate);
            return this;
        }
        public SelectFrom<TEntity> Select(Expression<Func<TEntity, object>> selector)
        {
            Builder.Select<TEntity>(selector);
            return this;
        }
        // public SelectFrom<V> SelectMany<U, V>(Expression<Func<TEntity, SelectFrom<U>>> selector,
        //     Expression<Func<Tentity, U, V>> resultSelector)
        // {
        //     return new SelectFrom<V>(Builder);
        // }
        public SelectFrom<TResult> Join<TNewEntity, TKey, TResult>(SelectFrom<TNewEntity> inner, Expression<Func<TEntity, TKey>> outerKeySelector,
            Expression<Func<TNewEntity, TKey>> innerKeySelector, Expression<Func<TEntity, TNewEntity, TResult>> resultSelector)
        {
            inner.Builder = this.Builder;
            var binaryExpression = Expression.MakeBinary(ExpressionType.Equal, outerKeySelector.Body, innerKeySelector.Body);
            var lambda = Expression.Lambda<Func<TEntity, TNewEntity, bool>>(binaryExpression, outerKeySelector.Parameters[0], innerKeySelector.Parameters[0]);
            Builder.Join<TEntity, TNewEntity>(lambda);
            return new SelectFrom<TResult>(Builder);
        }
        public SelectFrom<TEntity, TNewEntity> Join<TNewEntity>(Expression<Func<TEntity, TNewEntity, bool>> condition)
        {
            Builder.Join<TEntity, TNewEntity>(condition);
            return new SelectFrom<TEntity, TNewEntity>(Builder);
        }
        // public SelectFrom<V> GroupJoin<U, K, V>(SelectFrom<U> inner, Expression<Func<TEntity, K>> outerKeySelector,
        //     Expression<Func<U, K>> innerKeySelector, Expression<Func<TEntity, SelectFrom<U>, V>> resultSelector)
        // {
        //     return new SelectFrom<V>(Builder);
        // }
        public Order<TEntity> OrderBy(Expression<Func<TEntity, object>> keySelector)
        {
            Builder.OrderBy<TEntity>(OrderByDirection.Asc, keySelector);
            return new Order<TEntity>(Builder);
        }
        public Order<TEntity> OrderByDescending(Expression<Func<TEntity, object>> keySelector)
        {
            Builder.OrderBy<TEntity>(OrderByDirection.Desc, keySelector);
            return new Order<TEntity>(Builder);
        }
        public SelectFrom<TEntity> GroupBy(Expression<Func<TEntity, object>> keySelector)
        {
            Builder.GroupBy<TEntity>(keySelector);
            return this;
        }
        // public SelectFrom<Group<K, E>> GroupBy<K, E>(Expression<Func<TEntity, K>> keySelector,
        //     Expression<Func<TEntity, E>> elementSelector)
        // {
        //     return new SelectFrom<Group<K, E>>(Builder);
        // }

        public SelectFrom<TEntity> Having(Expression<Func<TEntity, bool>> condition)
        {
            Builder.Having<TEntity>(condition);
            return this;
        }
    }
    public class Order<TEntity> : SelectFrom<TEntity>
    {
        public Order(SelectBuilder builder) : base(builder)
        {
        }

        public Order<TEntity> ThenBy(Expression<Func<TEntity, object>> keySelector)
        {
            Builder.OrderBy<TEntity>(OrderByDirection.Asc, keySelector);
            return this;
        }
        public Order<TEntity> ThenByDescending(Expression<Func<TEntity, object>> keySelector)
        {
            Builder.OrderBy<TEntity>(OrderByDirection.Desc, keySelector);
            return this;
        }
    }
    // public class Group<K, T> : SelectFrom<T>
    // {
    //     public Group(SelectBuilder builder) : base(builder)
    //     {
    //     }

    //     public K Key { get; }
    // }
}