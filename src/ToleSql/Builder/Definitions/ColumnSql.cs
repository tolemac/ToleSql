namespace ToleSql.Builder.Definitions
{
    internal class ColumnSql
    {
        public string Expression { get; private set; }
        public string Alias { get; private set; }

        public ColumnSql(string expression, string alias)
        {
            Expression = expression;
            Alias = alias;
        }
    }
}
