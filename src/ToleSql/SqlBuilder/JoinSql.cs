namespace ToleSql.SqlBuilder
{
    public enum JoinType
    {
        Inner,
        Left
    }

    internal class JoinSql : SourceSql
    {
        public JoinType Type { get; set; }
        public string Condition { get; set; }

        public JoinSql(JoinType type, string expression, string alias, string condition) : base(expression, alias)
        {
            Type = type;
            Condition = condition;
        }
    }
}
