namespace ToleSql.SqlBuilder
{
    public enum WhereOperator
    {
        And,
        Or
    }

    internal class WhereSql
    {
        public WhereOperator PreOperator { get; set; }
        public string Expression { get; set; }

        public WhereSql(WhereOperator preOperator, string expression)
        {
            PreOperator = preOperator;
            Expression = expression;
        }
    }
}
