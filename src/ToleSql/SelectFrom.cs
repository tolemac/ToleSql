using System;
using System.Linq.Expressions;
using ToleSql.SqlBuilder;

namespace ToleSql
{
    //delegate R Expression<Func<T1, R>(T1 arg1);
    //delegate R Expression<Func<T1, T2, R>(T1 arg1, T2 arg2);
    public class SelectFrom
    {
        internal SelectBuilder Builder;
        public SelectFrom()
        {
            Builder = new SelectBuilder();
        }
        public SelectFrom<T> Cast<T>()
        {
            return new SelectFrom<T>(Builder);
        }

        public string GetSqlText()
        {
            return Builder.GetSqlText();
        }
    }
    public class SelectFrom<T> : SelectFrom
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
                Builder.SetMainTable<T>();
            }
        }

        public SelectFrom<T> Where(Expression<Func<T, bool>> predicate)
        {
            Builder.Where(predicate);
            return this;
        }
        public SelectFrom Select(Expression<Func<T, object>> selector)
        {
            Builder.Select<T>(selector);
            return this;
        }
        // public SelectFrom<V> SelectMany<U, V>(Expression<Func<T, SelectFrom<U>>> selector,
        //     Expression<Func<T, U, V>> resultSelector)
        // {
        //     return new SelectFrom<V>(Builder);
        // }
        public SelectFrom<V> Join<U, K, V>(SelectFrom<U> inner, Expression<Func<T, K>> outerKeySelector,
            Expression<Func<U, K>> innerKeySelector, Expression<Func<T, U, V>> resultSelector)
        {
            inner.Builder = this.Builder;
            var binaryExpression = Expression.MakeBinary(ExpressionType.Equal, outerKeySelector.Body, innerKeySelector.Body);
            var lambda = Expression.Lambda<Func<T, U, bool>>(binaryExpression, outerKeySelector.Parameters[0], innerKeySelector.Parameters[0]);
            Builder.AddJoin<T, U>(lambda);
            return new SelectFrom<V>(Builder);
        }
        public SelectFrom<U> Join<U>(Expression<Func<T, U, bool>> condition)
        {
            Builder.AddJoin<T, U>(condition);
            return new SelectFrom<U>(Builder);
        }
        // public SelectFrom<V> GroupJoin<U, K, V>(SelectFrom<U> inner, Expression<Func<T, K>> outerKeySelector,
        //     Expression<Func<U, K>> innerKeySelector, Expression<Func<T, SelectFrom<U>, V>> resultSelector)
        // {
        //     return new SelectFrom<V>(Builder);
        // }
        public Order<T> OrderBy(Expression<Func<T, Object>> keySelector)
        {
            Builder.OrderBy<T>(OrderByDirection.Asc, keySelector);
            return new Order<T>(Builder);
        }
        public Order<T> OrderByDescending(Expression<Func<T, object>> keySelector)
        {
            Builder.OrderBy<T>(OrderByDirection.Desc, keySelector);
            return new Order<T>(Builder);
        }
        public SelectFrom<T> GroupBy(Expression<Func<T, object>> keySelector)
        {
            Builder.GroupBy<T>(keySelector);
            return this;
        }
        // public SelectFrom<Group<K, E>> GroupBy<K, E>(Expression<Func<T, K>> keySelector,
        //     Expression<Func<T, E>> elementSelector)
        // {
        //     return new SelectFrom<Group<K, E>>(Builder);
        // }

        public SelectFrom<T> Having(Expression<Func<T, bool>> condition)
        {
            Builder.Having<T>(condition);
            return this;
        }
    }
    public class Order<T> : SelectFrom<T>
    {
        public Order(SelectBuilder builder) : base(builder)
        {
        }

        public Order<T> ThenBy(Expression<Func<T, object>> keySelector)
        {
            Builder.OrderBy<T>(OrderByDirection.Asc, keySelector);
            return this;
        }
        public Order<T> ThenByDescending(Expression<Func<T, object>> keySelector)
        {
            Builder.OrderBy<T>(OrderByDirection.Desc, keySelector);
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