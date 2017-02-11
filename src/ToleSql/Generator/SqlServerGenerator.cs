using ToleSql.Builder;

namespace ToleSql.Generator
{
    public class SqlServerGenerator : BaseGenerator
    {
        public SqlServerGenerator(SelectBuilder builder) : base(builder)
        {
        }

        protected override string GenerateSourceExpressionWithAlias(SourceExpression sourceExpression)
        {
            var result = sourceExpression.Expression;
            if (result.Contains(" ")) 
            {
                result = $"({result})";
            }
            if (!string.IsNullOrWhiteSpace(sourceExpression.Alias))
            {
                result += $" {Keyword(SqlKeyword.As)} {sourceExpression.Alias}";
            }
            return result;
        }
    }
}
