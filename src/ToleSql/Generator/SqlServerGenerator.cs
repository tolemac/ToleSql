using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToleSql.Builder;

namespace ToleSql.Generator
{
    public class SqlServerGenerator : BaseGenerator
    {
        public SqlServerGenerator(SelectBuilder builder) : base(builder)
        {
        }

        protected override string GenerateTableNameWithAlias(Table table)
        {
            var result = $"[{table.TableName}]";
            if (!string.IsNullOrWhiteSpace(table.SchemaName))
            {
                result = $"[{table.SchemaName}]" + result;
            }
            if (!string.IsNullOrWhiteSpace(table.Alias))
            {
                result += $" {Keyword(SqlKeyword.As)} {table.Alias}";
            }
            return result;
        }
    }
}
