using ToleSql.Builder;
using ToleSql.Configuration;

namespace ToleSql.Generator
{
    public static class SqlGenerator
    {
        public static string GetSqlText(this SelectBuilder builder)
        {
            return SqlConfiguration.Dialect.GetSelect(builder);
        }
    }
}