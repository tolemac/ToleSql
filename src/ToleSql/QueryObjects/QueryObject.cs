using System.Collections.Generic;

namespace ToleSql.QueryObjects
{
    public abstract class QueryObject : QueryObjectBase<SelectBuilder> { }
    public abstract class QueryObject<T> : QueryObjectBase<SelectFrom<T>> { }
    public abstract class QueryObjectBase<TBuilder> : IBuilder where TBuilder : IBuilder
    {
        protected virtual TBuilder Builder { get; set; }
        public virtual IDictionary<string, object> Parameters { get { return Builder.Parameters; } }

        public virtual string GetSqlText()
        {
            return Builder.GetSqlText();
        }

        protected abstract void Prepare();
    }
}