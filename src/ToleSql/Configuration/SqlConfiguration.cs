using System;
using System.Collections.Concurrent;
using ToleSql.Expressions.Visitors.Interceptors;
using ToleSql.Generator.Dialect;
using ToleSql.Generator.SqlServer;

namespace ToleSql.Configuration
{
    public class SqlConfiguration
    {
        private static ConcurrentBag<IMethodCallInterceptor> _interceptors
            = new ConcurrentBag<IMethodCallInterceptor>();

        public static ConcurrentBag<IMethodCallInterceptor> Interceptors { get { return _interceptors; } }
        public static IDialect Dialect { get; set; } = new SqlServerDialect();
        public static void SetDialect(IDialect dialect)
        {
            Dialect = dialect;
        }

        public static void RegisterInterceptor(IMethodCallInterceptor interceptor)
        {
            _interceptors.Add(interceptor);
        }

        static SqlConfiguration()
        {
            SqlConfiguration.RegisterInterceptor(new SubStringInterceptor());
            SqlConfiguration.RegisterInterceptor(new StringContainsInterceptor());
            SqlConfiguration.RegisterInterceptor(new StringStartsWithInterceptor());
            SqlConfiguration.RegisterInterceptor(new StringEndsWithInterceptor());
        }
    }
}
