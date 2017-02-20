namespace ToleSql.SqlBuilder
{
    internal class SourceSql
    {
        public string Expression { get; private set; }
        public string Alias { get; private set; }

        public SourceSql(string expression, string alias)
        {
            Expression = expression;
            Alias = alias;
        }
    }
}
