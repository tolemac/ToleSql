using System;
using System.Linq.Expressions;
using System.Text;
using ToleSql.Dialect;
using ToleSql.Functions;

namespace ToleSql.Expressions.Visitors.Interceptors
{
    public class DbFunctionsSum : MethodCallInterceptorBase
    {
        public override bool Intercept(MethodCallExpression m, StringBuilder sql, Func<Expression, Expression> visit)
        {
            if (m.Method.DeclaringType == typeof(DbFunctions) && m.Method.Name == nameof(DbFunctions.Sum))
            {
                var identifier = m.Arguments[0];
                sql.Append($"{Dialect.Keyword(SqlKeyword.Sum)}");
                sql.Append($"{Dialect.Symbol(SqlSymbols.StartGroup)}");
                visit(identifier);
                sql.Append($"{Dialect.Symbol(SqlSymbols.EndGroup)}");
                return true;
            }
            return false;

        }
    }
}