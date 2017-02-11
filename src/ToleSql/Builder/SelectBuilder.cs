using System.Collections.Generic;

namespace ToleSql.Builder
{
    public class SelectBuilder
    {
        internal SourceExpression MainSource;
        internal List<ColumnExpression> Selects = new List<ColumnExpression>();
        internal List<JoinExpression> Joins = new List<JoinExpression>();
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

        public SelectBuilder SetMainSource(string expression)
        {
            return SetMainSource(expression, null);
        }
        public SelectBuilder SetMainSource(string expression, string alias)
        {
            MainSource = new SourceExpression(expression, alias ?? GetNextAlias());
            return this;
        }

        public SelectBuilder AddColumnExpression(string expression)
        {
            return AddColumnExpression(expression, null);
        }
        public SelectBuilder AddColumnExpression(string expression, string alias)
        {
            Selects.Add(new ColumnExpression(expression, alias));
            return this;
        }

        public SelectBuilder AddJoin(string sourceExpression, string conditionExpression)
        {
            return AddJoin(JoinType.Inner, sourceExpression, null, conditionExpression);
        }
        public SelectBuilder AddJoin(JoinType type, string sourceExpression, string conditionExpression)
        {
            return AddJoin(type, sourceExpression, null, conditionExpression);
        }

        public SelectBuilder AddJoin(JoinType type, string sourceExpression, string alias, string conditionExpression)
        {
            Joins.Add(new JoinExpression(type, sourceExpression, alias ?? GetNextAlias(), conditionExpression));
            return this;
        }

        public SelectBuilder AddWhere(string expression)
        {
            return AddWhere(WhereOperator.And, expression);
        }
        public SelectBuilder AddWhere(WhereOperator preOperator, string expression)
        {
            Wheres.Add(new Where(preOperator, expression));
            return this;
        }

        public SelectBuilder OrderBy(string columnNameOrAlias)
        {
            return OrderBy(OrderByDirection.Asc, columnNameOrAlias);
        }
        public SelectBuilder OrderBy(OrderByDirection direction, string columnNameOrAlias)
        {
            OrderBys.Add(new OrderBy(direction, columnNameOrAlias));
            return this;
        }

        public SelectBuilder GroupBy(string columnNameOrAlias)
        {
            GroupBys.Add(columnNameOrAlias);
            return this;
        }

        public SelectBuilder AddHaving(string expression)
        {
            return AddHaving(WhereOperator.And, expression);
        }
        public SelectBuilder AddHaving(WhereOperator preOperator, string expression)
        {
            Havings.Add(new Where(preOperator, expression));
            return this;
        }
    }
}
