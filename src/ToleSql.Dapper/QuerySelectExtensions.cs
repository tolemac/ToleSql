using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ToleSql.Dapper
{
    public static class QuerySelectExtensions
    {

        public static IEnumerable<dynamic> Query(this IDbConnection cnn, RawSelectBuilder builder,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.Query(builder.GetSqlText(), builder.Parameters, transaction, buffered, commandTimeout, commandType);
        }
        public static IEnumerable<object> Query(this IDbConnection cnn, Type type, RawSelectBuilder builder,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.Query(type, builder.GetSqlText(), builder.Parameters, transaction, buffered, commandTimeout, commandType);
        }
        public static IEnumerable<T> Query<T>(this IDbConnection cnn, RawSelectBuilder builder, SqlTransaction transaction = null,
            bool buffered = true, int? commandTimeout = 0, CommandType? commandType = null)
        {
            return cnn.Query<T>(builder.GetSqlText(), builder.Parameters, transaction, buffered, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TReturn>(this IDbConnection cnn, RawSelectBuilder builder,
            Type[] types, Func<object[], TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.Query<TReturn>(builder.GetSqlText(), types, map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(this IDbConnection cnn, RawSelectBuilder builder,
            Func<TFirst, TSecond, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.Query(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn, RawSelectBuilder builder,
            Func<TFirst, TSecond, TThird, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.Query(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection cnn, RawSelectBuilder builder,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.Query(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbConnection cnn, RawSelectBuilder builder,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.Query(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(this IDbConnection cnn, RawSelectBuilder builder,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.Query(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
        public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this IDbConnection cnn, RawSelectBuilder builder,
            Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map, IDbTransaction transaction = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?),
            CommandType? commandType = default(CommandType?))
        {
            return cnn.Query(builder.GetSqlText(), map, builder.Parameters, transaction, buffered, splitOn, commandTimeout, commandType);
        }
    }
}