namespace ToleSql.Builder
{
    public class ColumnExpression
    {
        public string Expression { get; private set; }
        public string Alias { get; private set; }

        public ColumnExpression(string expression, string alias)
        {
            Expression = expression;
            Alias = alias;
        }
    }
}
