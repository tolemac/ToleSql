using System.Collections.Generic;

namespace ToleSql.Builder
{
    public class SelectBuilder
    {
        internal Table MainTable;
        internal List<Column> Columns = new List<Column>();
        internal List<Join> Joins = new List<Join>();
        internal List<Where> Wheres = new List<Where>();
        internal List<OrderBy> OrderBys = new List<OrderBy>();
        internal List<string> GroupBys = new List<string>();
        internal List<Where> Havings = new List<Where>();

        private int _aliasCount;
        public SelectBuilder()
        {
            _aliasCount = 0;
        }

        private string GetNextAlias()
        {
            return "T" + _aliasCount++;
        }

        public SelectBuilder SetMainTable(string tableName)
        {
            return SetMainTable(tableName, null, null);
        }
        public SelectBuilder SetMainTable(string tableName, string schemaName)
        {
            return SetMainTable(tableName, schemaName, null);
        }
        public SelectBuilder SetMainTable(string tableName, string schemaName, string alias)
        {
            MainTable = new Table(tableName, schemaName, alias ?? GetNextAlias());
            return this;
        }

        public SelectBuilder AddColumn(string columnName)
        {
            return AddColumn(columnName, null);
        }
        public SelectBuilder AddColumn(string column, string columnAs)
        {
            Columns.Add(new Column(column, columnAs));
            return this;
        }

        public SelectBuilder AddJoin(string tableName, string condition)
        {
            return AddJoin(tableName, MainTable?.SchemaName, null, condition);
        }

        public SelectBuilder AddJoin(string tableName, string schemaName, string condition)
        {
            return AddJoin(tableName, schemaName, null, condition);
        }

        public SelectBuilder AddJoin(string tableName, string schemaName, string alias, string condition)
        {
            Joins.Add(new Join(tableName, schemaName, alias ?? GetNextAlias(), condition));
            return this;
        }

        public SelectBuilder AddWhere(string condition)
        {
            return AddWhere(WhereOperator.And, condition);
        }
        public SelectBuilder AddWhere(WhereOperator preOperator, string condition)
        {
            Wheres.Add(new Where(preOperator, condition));
            return this;
        }

        public SelectBuilder OrderBy(string completeColumnName)
        {
            return OrderBy(OrderByDirection.Asc, completeColumnName);
        }
        public SelectBuilder OrderBy(OrderByDirection direction, string completeColumnName)
        {
            OrderBys.Add(new OrderBy(direction, completeColumnName));
            return this;
        }

        public SelectBuilder GroupBy(string completeColumnName)
        {
            GroupBys.Add(completeColumnName);
            return this;
        }

        public SelectBuilder AddHaving(string condition)
        {
            return AddHaving(WhereOperator.And, condition);
        }
        public SelectBuilder AddHaving(WhereOperator preOperator, string condition)
        {
            Havings.Add(new Where(preOperator, condition));
            return this;
        }
    }
}
