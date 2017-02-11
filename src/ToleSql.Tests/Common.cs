using ToleSql.Generator;
using ToleSql.Builder;

namespace ToleSql.Tests
{
    public class SimpleGenerator : BaseGenerator
    {
        public SimpleGenerator(SelectBuilder builder) : base(builder)
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

    public class ImplicitJoinGenerator : SimpleGenerator {
        public ImplicitJoinGenerator(SelectBuilder builder): base(builder) {
            JoinStyle = JoinStyle.Implicit;
        }
    }
}
