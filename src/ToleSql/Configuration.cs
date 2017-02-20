using System.Collections.Concurrent;
using ToleSql.Expressions.Visitors.Interceptors;
using ToleSql.Dialect;
using ToleSql.SqlServer;

namespace ToleSql
{
    public class Configuration
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

        static Configuration()
        {
            Configuration.RegisterInterceptor(new SubStringInterceptor());
            Configuration.RegisterInterceptor(new StringContainsInterceptor());
            Configuration.RegisterInterceptor(new StringStartsWithInterceptor());
            Configuration.RegisterInterceptor(new StringEndsWithInterceptor());
        }
    }
}
