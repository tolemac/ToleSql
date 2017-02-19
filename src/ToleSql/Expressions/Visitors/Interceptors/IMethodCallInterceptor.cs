using System;
using System.Linq.Expressions;
using System.Text;

namespace ToleSql.Expressions.Visitors.Interceptors
{
    public interface IMethodCallInterceptor
    {
        Expression Intercept(MethodCallExpression m, StringBuilder sql,
           Func<Expression, Expression> visit);
    }
}