namespace ToleSql.Builder
{
    public class SourceExpression
    {
        public string Expression { get; private set; }
        public string Alias { get; private set; }

        public SourceExpression(string expression, string alias)
        {
            Expression = expression;            
            Alias = alias;
        }
    }
}
