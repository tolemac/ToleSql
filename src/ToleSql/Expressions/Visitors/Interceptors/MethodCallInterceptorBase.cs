using System;
using System.Linq.Expressions;
using System.Text;
using ToleSql.Dialect;

namespace ToleSql.Expressions.Visitors.Interceptors
{
    public abstract class MethodCallInterceptorBase : IMethodCallInterceptor
    {
        protected IDialect Dialect { get { return Configuration.Dialect; } }
        public abstract Expression Intercept(MethodCallExpression m, StringBuilder sql,
                    Func<Expression, Expression> visit);
    }
}