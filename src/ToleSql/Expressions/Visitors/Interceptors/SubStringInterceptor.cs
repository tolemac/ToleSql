using System;
using System.Linq.Expressions;
using System.Text;
using ToleSql.Dialect;

namespace ToleSql.Expressions.Visitors.Interceptors
{
    internal class SubStringInterceptor : MethodCallInterceptorBase
    {
        public override Expression Intercept(MethodCallExpression m, StringBuilder sql,
            Func<Expression, Expression> visit)
        {
            if (m.Method.DeclaringType == typeof(string) && m.Method.Name == "Substring")
            {
                var start = m.Arguments[0];
                var end = m.Arguments.Count > 1 ? m.Arguments[1] : Expression.Constant(int.MaxValue);
                sql.Append(Dialect.Keyword(SqlKeyword.SubString));
                sql.Append(Dialect.Symbol(SqlSymbols.StartGroup));
                visit(m.Object);
                sql.Append(Dialect.Symbol(SqlSymbols.Comma));
                visit(start);
                sql.Append(Dialect.Symbol(SqlSymbols.Comma));
                visit(end);
                sql.Append(Dialect.Symbol(SqlSymbols.EndGroup));
                return m;
            }
            return null;
        }
    }
}