using System;
using System.Linq.Expressions;
using System.Text;
using ToleSql.Generator.Dialect;

namespace ToleSql.Expressions.Visitors.Interceptors
{
    internal class StringStartsWithInterceptor : MethodCallInterceptorBase
    {
        public override Expression Intercept(MethodCallExpression m, StringBuilder sql, Func<Expression, Expression> visit)
        {
            if (m.Method.DeclaringType == typeof(string) && m.Method.Name == "StartsWith")
            {
                var left = m.Object;
                var right = m.Arguments[0];
                visit(left);
                sql.Append($" {Dialect.Keyword(SqlKeyword.Like)} ");
                visit(right);
                sql.Append(" + " + Dialect.Quoted(Dialect.Symbol(SqlSymbols.Wildchar)));
                return m;
            }
            return null;
        }
    }
}