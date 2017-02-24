using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace ToleSql.Dapper
{
    public static class DapperAsyncSelectBuilderExtensions
    {
        public static Task<IDataReader> ExecuteReader(this IDbConnection cnn, IBuilder builder,
            IDbTransaction transaction = null, int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.ExecuteReaderAsync(builder.GetSqlText(), builder.Parameters, transaction, commandTimeout, commandType);
        }

        public static Task<IEnumerable<dynamic>> QueryAsync(this IDbConnection cnn, IBuilder builder,
            IDbTransaction transaction = null, int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.QueryAsync(builder.GetSqlText(), builder.Parameters, transaction, commandTimeout, commandType);
        }
        public static Task<IEnumerable<object>> Query(this IDbConnection cnn, Type type, IBuilder builder,
            IDbTransaction transaction = null, int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.QueryAsync(type, builder.GetSqlText(), builder.Parameters, transaction, commandTimeout, commandType);
        }
        public static Task<IEnumerable<T>> Query<T>(this IDbConnection cnn, IBuilder builder, SqlTransaction transaction = null,
            int? commandTimeout = 0, CommandType? commandType = null)
        {
            return cnn.QueryAsync<T>(builder.GetSqlText(), builder.Parameters, transaction, commandTimeout, commandType);
        }
        public static Task<IEnumerable<TReturn>> Query<TReturn>(this IDbConnection cnn, IBuilder builder,
            Type[] types, Func<object[], TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.QueryAsync<TReturn>(builder.GetSqlText(), types, map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static Task<IEnumerable<TReturn>> Query<TFirst, TSecond, TReturn>(this IDbConnection cnn, IBuilder builder,
            Func<TFirst, TSecond, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.QueryAsync(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static Task<IEnumerable<TReturn>> Query<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn, IBuilder builder,
            Func<TFirst, TSecond, TThird, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.QueryAsync(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static Task<IEnumerable<TReturn>> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection cnn, IBuilder builder,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.QueryAsync(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static Task<IEnumerable<TReturn>> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbConnection cnn, IBuilder builder,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.QueryAsync(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static Task<IEnumerable<TReturn>> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbConnection cnn, IBuilder builder,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.QueryAsync(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static Task<IEnumerable<TReturn>> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbConnection cnn, IBuilder builder,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.QueryAsync(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
    }
}