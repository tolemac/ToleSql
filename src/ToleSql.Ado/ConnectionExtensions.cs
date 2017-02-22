using System.Data;

namespace ToleSql.Ado
{
    public static class DbConnectionExtensions
    {
        public static T Apply<T>(this T cmd, SelectBuilder builder) where T : IDbCommand
        {
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
        public static T Apply<T>(this T cmd, SelectFrom builder) where T : IDbCommand
        {
            return Apply(cmd, builder.Builder);
        }
    }
}