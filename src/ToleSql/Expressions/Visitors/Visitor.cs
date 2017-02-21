using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using ToleSql.Dialect;

namespace ToleSql.Expressions.Visitors
{
    internal class Visitor : ExpressionVisitor
    {
        protected StringBuilder Result = new StringBuilder();
        Dictionary<string, TableDefinition> _definitionsByParameterName;
        RawSelectBuilder _builder;
        internal bool UseColumnAliases { get; set; } = false;
        public Visitor(Dictionary<string, TableDefinition> definitionsByParameterName, RawSelectBuilder builder)
        {
            _definitionsByParameterName = definitionsByParameterName;
            _builder = builder;
        }

        public string GetSql(Expression expr)
        {
            Result.Clear();
            if (expr.Type == typeof(bool) && expr.NodeType == ExpressionType.MemberAccess)
            {
                expr = ExpressionsHelper.GetBinaryExpression(expr);
            }
            Visit(expr);
            return Result.ToString();
        }
        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                var parameterExpression = (ParameterExpression)m.Expression;
                var tableDefinition = _definitionsByParameterName[parameterExpression.Name];
                var alias = tableDefinition.Alias;
                var columnName = tableDefinition.GetColumnName(m.Member.Name);
                if (columnName == null)
                {
                    columnName = m.Member.Name;
                }
                if (alias == null)
                {
                    Result.Append(Configuration.Dialect
                        .ColumnToSql(tableDefinition.TableName, tableDefinition.SchemaName, columnName));
                }
                else
                {
                    Result.Append(Configuration.Dialect.ColumnToSql(alias, columnName));
                }
                //_builder.GetOrAddTableDefinition
                //Result.Append(SqlConfiguration.Dialect.ColumnToSql() m.Member.DeclaringType.Name + ".");
                //Result.Append(m.Member.Name);
                return m;
            }
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Constant)
            {
                //throw new NotSupportedException("Not supported constans");
                var eval = EvaluateMember(m);
                var constantExpression = Expression.Constant(eval, m.Type);
                Visit(constantExpression);
                return constantExpression;
            }
            return m;
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            var tableDefinition = _definitionsByParameterName[p.Name];
            var alias = tableDefinition.Alias;
            Result.Append(Configuration.Dialect.AllColumnsFrom(alias));
            return p;
        }


        protected object EvaluateMember(MemberExpression m)
        {
            var compiled = Expression.Lambda(m).Compile();
            return compiled.DynamicInvoke();
        }

        protected void AddSubQuery(RawSelectBuilder subQueryBuilder)
        {
            var sql = subQueryBuilder.GetSqlText();
            foreach (var p in subQueryBuilder.Parameters)
            {
                _builder.AddParameter(p.Value);
                var paramName = _builder.Parameters.Last().Key;
                sql = sql.Replace(p.Key, paramName);
            }
            Result.Append(sql);
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;

            if (q == null && c.Value == null)
            {
                Result.Append("NULL");
            }
            else if (q == null)
            {
                if (c.Type == typeof(SelectBuilder))
                {
                    var builder = (SelectBuilder)c.Value;
                    AddSubQuery(builder);
                }
                else
                {
                    var paramKey = _builder.AddParameter(c.Value);
                    Result.Append(Configuration.Dialect.GetParameterPrefix() + paramKey);
                }

                // switch (Type.GetTypeCode(c.Value.GetType()))
                // {
                //     case TypeCode.Boolean:
                //         sb.Append(((bool)c.Value) ? 1 : 0);
                //         break;

                //     case TypeCode.String:
                //         sb.Append("'");
                //         sb.Append(c.Value);
                //         sb.Append("'");
                //         break;

                //     case TypeCode.DateTime:
                //         sb.Append("'");
                //         sb.Append(c.Value);
                //         sb.Append("'");
                //         break;

                //     case TypeCode.Object:
                //         throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));

                //     default:
                //         sb.Append(c.Value);
                //         break;
                // }
            }

            return c;
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    Result.Append(" NOT (");
                    this.Visit(u.Operand);
                    Result.Append(")");
                    break;
                case ExpressionType.Convert:
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            var left = b.Left;
            var right = b.Right;
            var boolOperators = new[] { ExpressionType.AndAlso, ExpressionType.OrElse };
            if (boolOperators.Contains(b.NodeType))
            {
                left = left.NodeType == ExpressionType.MemberAccess ? ExpressionsHelper.GetBinaryExpression(left) : left;
                right = right.NodeType == ExpressionType.MemberAccess ? ExpressionsHelper.GetBinaryExpression(right) : right;
            }

            var dialect = Configuration.Dialect;
            Result.Append(dialect.Symbol(SqlSymbols.StartGroup));
            this.Visit(left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    Result.Append($" {dialect.Keyword(SqlKeyword.And)} ");
                    break;

                case ExpressionType.AndAlso:
                    Result.Append($" {dialect.Keyword(SqlKeyword.And)} ");
                    break;

                case ExpressionType.Or:
                    Result.Append($" {dialect.Keyword(SqlKeyword.Or)} ");
                    break;

                case ExpressionType.OrElse:
                    Result.Append($" {dialect.Keyword(SqlKeyword.Or)} ");
                    break;

                case ExpressionType.Equal:
                    if (IsNullConstant(b.Right))
                    {
                        Result.Append($" {dialect.ComparisonSymbol(SqlComparison.Is)} ");
                    }
                    else
                    {
                        Result.Append($" {dialect.ComparisonSymbol(SqlComparison.Equal)} ");
                    }
                    break;

                case ExpressionType.NotEqual:
                    if (IsNullConstant(b.Right))
                    {
                        Result.Append($" {dialect.ComparisonSymbol(SqlComparison.IsNot)} ");
                    }
                    else
                    {
                        Result.Append($" {dialect.ComparisonSymbol(SqlComparison.NotEqual)} ");
                    }
                    break;

                case ExpressionType.LessThan:
                    Result.Append($" {dialect.ComparisonSymbol(SqlComparison.LessThan)} ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    Result.Append($" {dialect.ComparisonSymbol(SqlComparison.LessThanOrEqual)} ");
                    break;

                case ExpressionType.GreaterThan:
                    Result.Append($" {dialect.ComparisonSymbol(SqlComparison.GreaterThan)} ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    Result.Append($" {dialect.ComparisonSymbol(SqlComparison.GreaterThanOrEqual)} ");
                    break;

                case ExpressionType.Add:
                    Result.Append($" {dialect.ArithMeticOperand(SqlArithmeticOperand.Add)} ");
                    break;

                case ExpressionType.Subtract:
                    Result.Append($" {dialect.ArithMeticOperand(SqlArithmeticOperand.Subtract)} ");
                    break;

                case ExpressionType.Multiply:
                    Result.Append($" {dialect.ArithMeticOperand(SqlArithmeticOperand.Multiply)} ");
                    break;

                case ExpressionType.Divide:
                    Result.Append($" {dialect.ArithMeticOperand(SqlArithmeticOperand.Divide)} ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));

            }

            this.Visit(right);
            Result.Append(dialect.Symbol(SqlSymbols.EndGroup));
            return b;
        }

        protected bool IsNullConstant(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null)
            || (exp.NodeType == ExpressionType.MemberAccess && ((MemberExpression)exp).Expression.NodeType == ExpressionType.Constant
            && EvaluateMember((MemberExpression)exp) == null);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(SelectBuilderQueryExpressionMethods) && m.Method.Name == nameof(SelectBuilderQueryExpressionMethods.Contains))
            {
                var a = m.Arguments[1];
                Visit(a);
                Result.Append(" " + Configuration.Dialect.Keyword(SqlKeyword.In) + " ");
                Result.Append(Configuration.Dialect.Symbol(SqlSymbols.StartGroup));
                Visit(m.Arguments[0]);
                Result.Append(Configuration.Dialect.Symbol(SqlSymbols.EndGroup));
            }
            else
            {
                foreach (var interceptor in Configuration.Interceptors)
                {
                    var result = interceptor.Intercept(m, Result, (exp) => Visit(exp));
                    if (result)
                        return m;
                }

                var compiled = Expression.Lambda(m).Compile();
                var constantExpression = Expression.Constant(compiled.DynamicInvoke(), m.Type);
                Visit(constantExpression);
            }

            // if (m.Method.DeclaringType == typeof(string) && m.Method.Name == "Substring")
            // {
            //     var start = m.Arguments[0];
            //     var end = m.Arguments.Count > 1? m.Arguments[1] : null;
            //     Result.Append("SUBSTRING")
            //     Visit(left);
            //     Result.Append(" LIKE " + SqlConfiguration.Dialect.Quoted(SqlConfiguration.Dialect.Symbol(SqlSymbols.Wildchar)) + " + ");
            //     Visit(right);
            // }
            return m;
        }

        private void AddColumnAs(Expression identifier, string columnAlias)
        {
            Visit(identifier);
            if (UseColumnAliases)
            {
                Result.Append(" " + Configuration.Dialect.Keyword(SqlKeyword.As) + " ");
                Result.Append(columnAlias);
            }
        }
        protected override Expression VisitNew(NewExpression nex)
        {
            // IEnumerable<Expression> args = this.VisitExpressionList(nex.Arguments);
            // if (args != nex.Arguments)
            // {
            //     if (nex.Members != null)
            //         return Expression.New(nex.Constructor, args, nex.Members);
            //     else 
            //         return Expression.New(nex.Constructor, args);
            // }

            // Only if type is anonymous.
            // Anonymous type initialization is translate to new expression.
            // View section 7.6.10.6 of C# specification. 
            if (nex.Type.Name.Contains("AnonymousType") && nex.Type.Namespace == null)
            {
                for (int i = 0; i < nex.Arguments.Count; i++)
                {
                    AddColumnAs(nex.Arguments[i], nex.Members[i].Name);
                    if (nex.Arguments.Count > i + 1)
                    {
                        Result.Append(Configuration.Dialect.Symbol(SqlSymbols.Comma) + " ");
                    }
                }
            }

            return nex;
        }
        protected override Expression VisitMemberInit(MemberInitExpression init)
        {
            NewExpression n = this.VisitNew(init.NewExpression) as NewExpression;
            var i = 0;
            foreach (var binding in init.Bindings)
            {
                var ma = binding as MemberAssignment;
                if (ma == null)
                    continue;
                AddColumnAs(ma.Expression, ma.Member.Name);
                i++;
                if (init.Bindings.Count > i)
                {
                    Result.Append(Configuration.Dialect.Symbol(SqlSymbols.Comma) + " ");
                }
            }


            return init;
        }

    }
}