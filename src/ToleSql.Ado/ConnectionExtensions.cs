using System.Data;

namespace ToleSql.Ado
{
    public static class DbConnectionExtensions
    {
        public static IDbCommand CreateCommand(this IDbConnection cnn, IBuilder builder)
        {
            var cmd = cnn.CreateCommand();
            cmd.CommandText = builder.GetSqlText();
            foreach (var param in builder.Parameters)
            {
                var dbParam = cmd.CreateParameter();
                dbParam.ParameterName = param.Key;
                dbParam.Value = param.Value;
                cmd.Parameters.Add(dbParam);
            }
            return cmd;
        }
        public static IDataReader ExecuteReader(this IDbConnection cnn, IBuilder builder)
        {
            var cmd = cnn.CreateCommand();
            return cmd.ExecuteReader();
        }
    }
}
