using System;
using System.Linq;
using System.Text;
using ToleSql.SqlBuilder;

namespace ToleSql.Dialect
{
    public abstract class DialectBase : IDialect
    {
        public abstract string TableToSql(string tableName, string schemaName);
        public abstract string ColumnToSql(string tableName, string schemaName, string columnName);
        public abstract string ColumnToSql(string alias, string columnName);
        public abstract string AllColumnsFrom(string alias);
        public abstract string AlaisToSql(string alias);
        public abstract string GetParameterPrefix();
        public abstract string Quoted(string inner);
        public virtual JoinStyle JoinStyle { get; set; } = JoinStyle.Explicit;
        public virtual string Symbol(SqlSymbols symbol)
        {
            switch (symbol)
            {
                case SqlSymbols.Comma:
                    return ",";
                case SqlSymbols.StartGroup:
                    return "(";
                case SqlSymbols.EndGroup:
                    return ")";
                case SqlSymbols.Wildchar:
                    return "%";
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        public virtual string ArithMeticOperand(SqlArithmeticOperand operand)
        {
            switch (operand)
            {
                case SqlArithmeticOperand.Add:
                    return "+";
                case SqlArithmeticOperand.Subtract:
                    return "-";
                case SqlArithmeticOperand.Multiply:
                    return "*";
                case SqlArithmeticOperand.Divide:
                    return "/";
                default:
                    throw new ArgumentOutOfRangeException(nameof(operand), operand, null);
            }
        }
        public virtual string ComparisonSymbol(SqlComparison symbol)
        {
            switch (symbol)
            {
                case SqlComparison.Equal:
                    return "=";
                case SqlComparison.LessThan:
                    return "<";
                case SqlComparison.LessThanOrEqual:
                    return "<=";
                case SqlComparison.GreaterThan:
                    return ">";
                case SqlComparison.GreaterThanOrEqual:
                    return ">=";
                case SqlComparison.NotEqual:
                    return "<>";
                case SqlComparison.Is:
                    return "IS";
                case SqlComparison.IsNot:
                    return "IS NOT";
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }

        }

        public virtual string Keyword(SqlKeyword keyword)
        {
            switch (keyword)
            {
                case SqlKeyword.Select:
                    return "SELECT";
                case SqlKeyword.As:
                    return "AS";
                case SqlKeyword.AllColumns:
                    return "*";
                case SqlKeyword.From:
                    return "FROM";
                case SqlKeyword.InnerJoin:
                    return "INNER JOIN";
                case SqlKeyword.LeftJoin:
                    return "LEFT JOIN";
                case SqlKeyword.On:
                    return "ON";
                case SqlKeyword.Where:
                    return "WHERE";
                case SqlKeyword.And:
                    return "AND";
                case SqlKeyword.Or:
                    return "OR";
                case SqlKeyword.Not:
                    return "NOT";
                case SqlKeyword.OrderBy:
                    return "ORDER BY";
                case SqlKeyword.GroupBy:
                    return "GROUP BY";
                case SqlKeyword.Having:
                    return "HAVING";
                case SqlKeyword.In:
                    return "IN";
                case SqlKeyword.Like:
                    return "LIKE";
                case SqlKeyword.SubString:
                    return "SUBSTRING";
                case SqlKeyword.Sum:
                    return "SUM";
                case SqlKeyword.Count:
                    return "COUNT";
                case SqlKeyword.Min:
                    return "MIN";
                case SqlKeyword.Max:
                    return "MAX";
                default:
                    throw new ArgumentOutOfRangeException(nameof(keyword), keyword, null);
            }
        }

        public virtual string GetSelectClausule(RawSelectBuilder builder)
        {
            var columns = builder.SelectSqls.Any()
                ? string.Join($"{Symbol(SqlSymbols.Comma)} ",
                    builder.SelectSqls.Select(
                        c => c.Expression + (c.Alias != null ? $" {Keyword(SqlKeyword.As)} {AlaisToSql(c.Alias)}" : ""))
                        .ToArray())
                : Keyword(SqlKeyword.AllColumns);
            return Keyword(SqlKeyword.Select) + " " + columns;
        }

        public virtual string GetSourceSqlWithAlias(string sourceExpression, string alias)
        {
            var result = sourceExpression;
            if (result.Contains(" "))
            {
                result = $"({result})";
            }
            if (!string.IsNullOrWhiteSpace(alias))
            {
                result += $" {Keyword(SqlKeyword.As)} {AlaisToSql(alias)}";
            }
            return result;
        }

        public virtual string GetJoinTypeLiteral(JoinType joinType)
        {
            switch (joinType)
            {
                case JoinType.Inner:
                    return Keyword(SqlKeyword.InnerJoin);
                case JoinType.Left:
                    return Keyword(SqlKeyword.LeftJoin);
                default:
                    throw new ArgumentOutOfRangeException(nameof(joinType), joinType, null);
            }
        }

        public virtual string GetSourceExplicitJoins(RawSelectBuilder builder)
        {
            if (builder.MainSourceSql == null)
                return "";
            var result = new StringBuilder(Keyword(SqlKeyword.From) + $" {GetSourceSqlWithAlias(builder.MainSourceSql.Expression, builder.MainSourceSql.Alias)}");
            foreach (var j in builder.JoinSqls)
            {
                result.Append($" {GetJoinTypeLiteral(j.Type)} {GetSourceSqlWithAlias(j.Expression, j.Alias)} {Keyword(SqlKeyword.On)} {j.Condition}");
            }
            return result.ToString();
        }

        public virtual string GetImplicitJoin(RawSelectBuilder builder)
        {
            var result = new StringBuilder(Keyword(SqlKeyword.From) + $" {GetSourceSqlWithAlias(builder.MainSourceSql.Expression, builder.MainSourceSql.Alias)}");
            foreach (var j in builder.JoinSqls)
            {
                result.Append($"{Symbol(SqlSymbols.Comma)} {GetSourceSqlWithAlias(j.Expression, j.Alias)}");
            }
            return result.ToString().TrimEnd();
        }

        public virtual string GetSource(RawSelectBuilder builder)
        {
            if (JoinStyle == JoinStyle.Explicit)
            {
                return GetSourceExplicitJoins(builder);
            }
            return GetImplicitJoin(builder);
        }

        public virtual string GetImplicitJoinWhere(RawSelectBuilder builder)
        {
            if (!builder.JoinSqls.Any())
                return "";
            return builder.JoinSqls.Select(j => j.Condition).Aggregate((current, next) => current + $" {Keyword(SqlKeyword.And)} " + next);
        }
        public virtual string GetWhereClausule(RawSelectBuilder builder)
        {
            var result = new StringBuilder($"{Keyword(SqlKeyword.Where)} ");
            var implicitJoinWhere = "";
            if (JoinStyle == JoinStyle.Implicit)
            {
                implicitJoinWhere = GetImplicitJoinWhere(builder);
                result.Append(
                    $"{Symbol(SqlSymbols.StartGroup)}{implicitJoinWhere}{Symbol(SqlSymbols.EndGroup)}");
                if (builder.WhereSqls.Any())
                    result.Append($" {Keyword(SqlKeyword.And)} ");
            }
            if (implicitJoinWhere == "" && !builder.WhereSqls.Any())
                return "";

            var whereCount = 0;
            foreach (var w in builder.WhereSqls)
            {
                if (whereCount > 0)
                {
                    if (w.PreOperator == WhereOperator.And)
                        result.Append($" {Keyword(SqlKeyword.And)} ");
                    else if (w.PreOperator == WhereOperator.Or)
                        result.Append($" {Keyword(SqlKeyword.Or)} ");
                }
                result.Append($"{w.Expression}");
                whereCount++;
            }
            return result.ToString();
        }

        public virtual string GetOrderByClausule(RawSelectBuilder builder)
        {
            if (!builder.OrderBySqls.Any())
                return "";

            var orderBy =
                string.Join(", ", builder.OrderBySqls.Select(
                    o => $"{o.Expression}" + (o.Direction == OrderByDirection.Asc ? " ASC" : " DESC"))
                    .ToArray());

            return $"{Keyword(SqlKeyword.OrderBy)} " + orderBy;
        }
        public virtual string GetGroupByClausule(RawSelectBuilder builder)
        {
            if (!builder.GroupBySqls.Any())
                return "";
            return $"{Keyword(SqlKeyword.GroupBy)} " + string.Join(", ", builder.GroupBySqls.ToArray());
        }
        public virtual string GetHavingClausule(RawSelectBuilder builder)
        {
            if (!builder.HavingSqls.Any())
                return "";

            var result = new StringBuilder($"{Keyword(SqlKeyword.Having)} ");
            var count = 0;
            foreach (var w in builder.HavingSqls)
            {
                if (count > 0)
                {
                    if (w.PreOperator == WhereOperator.And)
                        result.Append($" {Keyword(SqlKeyword.And)} ");
                    else if (w.PreOperator == WhereOperator.Or)
                        result.Append($" {Keyword(SqlKeyword.Or)} ");
                }
                result.Append($"{w.Expression}");
            }
            return result.ToString();
        }

        public virtual string GetSelect(RawSelectBuilder builder)
        {
            var select = GetSelectClausule(builder);
            var source = GetSource(builder);
            var where = GetWhereClausule(builder);
            var orderBy = GetOrderByClausule(builder);
            var groupBy = GetGroupByClausule(builder);
            var having = GetHavingClausule(builder);

            var result = $"{select} {source}";
            if (!string.IsNullOrWhiteSpace(where))
                result += $" {where}";
            if (!string.IsNullOrWhiteSpace(orderBy))
                result += $" {orderBy}";
            if (!string.IsNullOrWhiteSpace(groupBy))
                result += $" {groupBy}";
            if (!string.IsNullOrWhiteSpace(having))
                result += $" {having}";
            return result;
        }
    }
}
