using System;
using System.Collections.Generic;
using ToleSql.SqlBuilder;
using ToleSql.Dialect;

namespace ToleSql
{
    public class RawSelectBuilder
    {
        internal SourceSql MainSourceSql;
        internal List<ColumnSql> SelectSqls = new List<ColumnSql>();
        internal List<JoinSql> JoinSqls = new List<JoinSql>();
        internal List<WhereSql> WhereSqls = new List<WhereSql>();
        internal List<OrderBySql> OrderBySqls = new List<OrderBySql>();
        internal List<string> GroupBySqls = new List<string>();
        internal List<WhereSql> HavingSqls = new List<WhereSql>();
        internal IDialect Dialect { get { return Configuration.Dialect; } }
        public IDictionary<string, object> Parameters { get { return _parameters; } }

        private int _aliasCount = 0;
        private int _paramCount = 0;
        private int _subQueryCount = 0;
        private IDictionary<string, object> _parameters = new Dictionary<string, object>();
        public RawSelectBuilder()
        {
        }

        internal string ParameterNamePrefix { get; } = "SqlParam";

        protected string GetNextTableAlias()
        {
            return "T" + _aliasCount++;
        }

        public RawSelectBuilder SetMainSourceSql(string expression)
        {
            return SetMainSourceSql(expression, null);
        }
        public RawSelectBuilder SetMainSourceSql(string expression, string alias)
        {
            if (MainSourceSql != null)
            {
                throw new NotSupportedException("Main source already defined.");
            }
            MainSourceSql = new SourceSql(expression, alias ?? GetNextTableAlias());
            return this;
        }

        public RawSelectBuilder AddColumnSql(string expression)
        {
            return AddColumnSql(expression, null);
        }
        public RawSelectBuilder AddColumnSql(string expression, string alias)
        {
            SelectSqls.Add(new ColumnSql(expression, alias));
            return this;
        }

        public RawSelectBuilder AddJoinSql(string sourceExpression, string conditionExpression)
        {
            return AddJoinSql(JoinType.Inner, sourceExpression, null, conditionExpression);
        }
        public RawSelectBuilder AddJoinSql(JoinType type, string sourceExpression, string conditionExpression)
        {
            return AddJoinSql(type, sourceExpression, null, conditionExpression);
        }

        public RawSelectBuilder AddJoinSql(JoinType type, string sourceExpression, string alias, string conditionExpression)
        {
            JoinSqls.Add(new JoinSql(type, sourceExpression, alias ?? GetNextTableAlias(), conditionExpression));
            return this;
        }

        public RawSelectBuilder AddWhereSql(string expression)
        {
            return AddWhereSql(WhereOperator.And, expression);
        }
        public RawSelectBuilder AddWhereSql(WhereOperator preOperator, string expression)
        {
            WhereSqls.Add(new WhereSql(preOperator, expression));
            return this;
        }

        public RawSelectBuilder AddOrderBySql(string expression)
        {
            return AddOrderBySql(OrderByDirection.Asc, expression);
        }
        public RawSelectBuilder AddOrderBySql(OrderByDirection direction, string expression)
        {
            OrderBySqls.Add(new OrderBySql(direction, expression));
            return this;
        }

        public RawSelectBuilder AddGroupBySql(string expression)
        {
            GroupBySqls.Add(expression);
            return this;
        }

        public RawSelectBuilder AddHavingSql(string expression)
        {
            return AddHavingSql(WhereOperator.And, expression);
        }
        public RawSelectBuilder AddHavingSql(WhereOperator preOperator, string expression)
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
