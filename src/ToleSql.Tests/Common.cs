using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToleSql.Generator;
using ToleSql.Builder;

namespace ToleSql.Tests
{
    public class SimpleGenerator : BaseGenerator
    {
        public SimpleGenerator(SelectBuilder builder) : base(builder)
        {
        }

        protected override string GenerateTableNameWithAlias(Table table)
        {
            var result = $"[{table.TableName}]";
            if (!string.IsNullOrWhiteSpace(table.SchemaName))
            {
                result = $"[{table.SchemaName}]." + result;
            }
            if (!string.IsNullOrWhiteSpace(table.Alias))
            {
                result += $" {Keyword(SqlKeyword.As)} {table.Alias}";
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
