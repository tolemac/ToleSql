using System;
using System.Collections;
using System.Collections.Generic;
using ToleSql.Builder.Definitions;
using ToleSql.Configuration;
using ToleSql.Generator.Dialect;

namespace ToleSql.Builder
{
    public class SelectBuilder
    {
        internal SourceSql MainSourceSql;
        internal List<ColumnSql> SelectSqls = new List<ColumnSql>();
        internal List<JoinSql> JoinSqls = new List<JoinSql>();
        internal List<WhereSql> WhereSqls = new List<WhereSql>();
        internal List<OrderBy> OrderBySqls = new List<OrderBy>();
        internal List<string> GroupBySqls = new List<string>();
        internal List<WhereSql> HavingSqls = new List<WhereSql>();
        internal IDialect Dialect { get { return SqlConfiguration.Dialect; } }
        public IDictionary<string, object> Parameters { get { return _parameters; } }

        private int _aliasCount = 0;
        private int _paramCount = 0;
        private int _subQueryCount = 0;
        private IDictionary<string, object> _parameters = new Dictionary<string, object>();
        public SelectBuilder()
        {
        }

        internal string ParameterNamePrefix { get; } = "SqlParam";

        protected string GetNextTableAlias()
        {
            return "T" + _aliasCount++;
        }

        public SelectBuilder SetMainSourceSql(string expression)
        {
            return SetMainSourceSql(expression, null);
        }
        public SelectBuilder SetMainSourceSql(string expression, string alias)
        {
            if (MainSourceSql != null)
            {
                throw new NotSupportedException("Main source already defined.");
            }
            MainSourceSql = new SourceSql(expression, alias ?? GetNextTableAlias());
            return this;
        }

        public SelectBuilder AddColumnSql(string expression)
        {
            return AddColumnSql(expression, null);
        }
        public SelectBuilder AddColumnSql(string expression, string alias)
        {
            SelectSqls.Add(new ColumnSql(expression, alias));
            return this;
        }

        public SelectBuilder AddJoinSql(string sourceExpression, string conditionExpression)
        {
            return AddJoinSql(JoinType.Inner, sourceExpression, null, conditionExpression);
        }
        public SelectBuilder AddJoinSql(JoinType type, string sourceExpression, string conditionExpression)
        {
            return AddJoinSql(type, sourceExpression, null, conditionExpression);
        }

        public SelectBuilder AddJoinSql(JoinType type, string sourceExpression, string alias, string conditionExpression)
        {
            JoinSqls.Add(new JoinSql(type, sourceExpression, alias ?? GetNextTableAlias(), conditionExpression));
            return this;
        }

        public SelectBuilder AddWhereSql(string expression)
        {
            return AddWhereSql(WhereOperator.And, expression);
        }
        public SelectBuilder AddWhereSql(WhereOperator preOperator, string expression)
        {
            WhereSqls.Add(new WhereSql(preOperator, expression));
            return this;
        }

        public SelectBuilder AddOrderBySql(string expression)
        {
            return AddOrderBySql(OrderByDirection.Asc, expression);
        }
        public SelectBuilder AddOrderBySql(OrderByDirection direction, string expression)
        {
            OrderBySqls.Add(new OrderBy(direction, expression));
            return this;
        }

        public SelectBuilder AddGroupBySql(string expression)
        {
            GroupBySqls.Add(expression);
            return this;
        }

        public SelectBuilder AddHavingSql(string expression)
        {
            return AddHavingSql(WhereOperator.And, expression);
        }
        public SelectBuilder AddHavingSql(WhereOperator preOperator, string expression)
        {
            HavingSqls.Add(new WhereSql(preOperator, expression));
            return this;
        }

        internal string GetSubQueryParamPrefix()
        {
            return "SubQ" + _subQueryCount++;
        }

        public string NextParamId()
        {
            return ParameterNamePrefix + (_paramCount++);
        }
        public string AddParameter(object value)
        {
            var key = NextParamId();
            _parameters.Add(key, value);
            return key;
        }
    }
}
