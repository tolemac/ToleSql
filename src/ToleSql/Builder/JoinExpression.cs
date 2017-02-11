namespace ToleSql.Builder
{
    public enum JoinType
    {
        Inner,
        Left
    }

    public class JoinExpression : SourceExpression
    {
        public JoinType Type { get; set; }
        public string Condition { get; set; }

        public JoinExpression(JoinType type, string expression, string alias, string condition) : base(expression, alias)
        {
            Type = type;
            Condition = condition;
        }
    }
}
