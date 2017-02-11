namespace ToleSql.Builder
{
    public enum WhereOperator
    {
        And,
        Or
    }

    public class Where
    {
        public WhereOperator PreOperator { get; set; }
        public string Expression { get; set; }

        public Where(WhereOperator preOperator, string expression)
        {
            PreOperator = preOperator;
            Expression = expression;
        }
    }
}
