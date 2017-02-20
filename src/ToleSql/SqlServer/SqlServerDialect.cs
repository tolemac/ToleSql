using ToleSql.Dialect;

namespace ToleSql.SqlServer
{
    public class SqlServerDialect : DialectBase
    {
        public SqlServerDialect()
        {
            JoinStyle = JoinStyle.Explicit;
        }

        public override string TableToSql(string tableName, string schemaName)
        {
            var result = $"[{tableName}]";
            if (!string.IsNullOrWhiteSpace(schemaName))
            {
                result = $"[{schemaName}]." + result;
            }
            return result;
        }

        public override string ColumnToSql(string tableName, string schemaName, string columnName)
        {
            var result = $"[{tableName}].[{columnName}]";
            if (!string.IsNullOrWhiteSpace(schemaName))
            {
                result = $"[{schemaName}]." + result;
            }
            return result;
        }
        public override string ColumnToSql(string alias, string columnName)
        {
            var result = $"[{alias}].[{columnName}]";
            return result;
        }

        public override string AlaisToSql(string alias)
        {
            return $"[{alias}]";
        }

        public override string GetParameterPrefix()
        {
            return "@";
        }

        public override string Quoted(string inner)
        {
            return $"'{inner}'";
        }
    }
}
