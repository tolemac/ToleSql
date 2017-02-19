using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using ToleSql.Expressions;

namespace ToleSql.Configuration.Definitions
{
    public class TableModel
    {
        public Type ModelType { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        internal ConcurrentDictionary<string, ColumnModel> _properties = new ConcurrentDictionary<string, ColumnModel>();
        public TableModel(Type modelType)
        {
            ModelType = modelType;
        }
        public ColumnModel Column(string propertyName)
        {
            return _properties.GetOrAdd(propertyName, (proName) => new ColumnModel());
        }
        public TableModel SetColumnName(string propertyName, string columnName)
        {
            Column(propertyName).ColumnName = columnName;
            return this;
        }
    }
    public class TableModel<TEntity> : TableModel
    {
        public TableModel() : base(typeof(TEntity))
        {

        }
        public TableModel<TEntity> SetSchema(string schemaName)
        {
            SchemaName = schemaName;
            return this;
        }
        public TableModel<TEntity> SetTable(string tableName)
        {
            TableName = tableName;
            return this;
        }

        protected string GetPropertyName(Expression<Func<TEntity, object>> expr)
        {
            var memberExpression = ExpressionsHelper.GetMemberExpression(expr.Body);
            return memberExpression.Member.Name;
        }

        public ColumnModel Column(Expression<Func<TEntity, object>> expression)
        {
            return Column(GetPropertyName(expression));
        }
        public TableModel<TEntity> SetColumnName(
            Expression<Func<TEntity, object>> expression, string columnName)
        {
            Column(expression).ColumnName = columnName;
            return this;
        }
        public TableModel<TEntity> Ignore(Expression<Func<TEntity, object>> expression, bool ignore = true)
        {
            Column(expression).Ignored = ignore;
            return this;
        }
    }
}