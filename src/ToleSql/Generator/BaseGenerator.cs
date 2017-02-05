using System;
using System.Linq;
using System.Text;
using ToleSql.Builder;

namespace ToleSql.Generator
{
    public enum JoinStyle { Implicit, Explicit }
    public enum SqlKeyword { Select, As, AllColumns, From, InnerJoin, LeftJoin, On, Where, And, Or, Not, OrderBy, GroupBy, Having}
    public enum SqlSymbols { Comma, StartGroup, EndGroup }
    public abstract class BaseGenerator : IGenerator
    {
        protected readonly SelectBuilder Builder;
        protected JoinStyle JoinStyle;

        protected BaseGenerator(SelectBuilder builder)
        {
            Builder = builder;
            JoinStyle = JoinStyle.Explicit;
        }

        protected virtual string Symbol(SqlSymbols symbol)
        {
            switch (symbol)
            {
                case SqlSymbols.Comma:
                    return ",";
                case SqlSymbols.StartGroup:
                    return "(";
                case SqlSymbols.EndGroup:
                    return ")";
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol), symbol, null);
            }
        }

        protected virtual string Keyword(SqlKeyword keyword)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(keyword), keyword, null);
            }
        }

        protected virtual string GenerateSelect()
        {
            var columns = Builder.Columns.Any()
                ? string.Join($"{Symbol(SqlSymbols.Comma)} ",
                    Builder.Columns.Select(
                        c => c.ColumnName + (c.ColumnAs != null ? $" {Keyword(SqlKeyword.As)} {c.ColumnAs}" : ""))
                        .ToArray())
                : Keyword(SqlKeyword.AllColumns);
            return Keyword(SqlKeyword.Select) + " " + columns;
        }

        protected abstract string GenerateTableNameWithAlias(Table table);

        protected virtual string GetJoinTypeLiteral(JoinType joinType)
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

        protected virtual string GenerateSourceExplicitJoins()
        {
            var result = new StringBuilder(Keyword(SqlKeyword.From) + $" {GenerateTableNameWithAlias(Builder.MainTable)}");
            foreach (var j in Builder.Joins)
            {
                result.Append($" {GetJoinTypeLiteral(j.Type)} {GenerateTableNameWithAlias(j)} {Keyword(SqlKeyword.On)} {j.Condition}");
            }
            return result.ToString();
        }

        protected virtual string GenerateImplicitJoin()
        {
            var result = new StringBuilder(Keyword(SqlKeyword.From) + $" {GenerateTableNameWithAlias(Builder.MainTable)}");
            foreach (var j in Builder.Joins)
            {
                result.Append($"{Symbol(SqlSymbols.Comma)} {GenerateTableNameWithAlias(j)}");
            }
            return result.ToString().TrimEnd();
        }

        protected virtual string GenerateSource()
        {
            if (JoinStyle == JoinStyle.Explicit)
            {
                return GenerateSourceExplicitJoins();
            }
            return GenerateImplicitJoin();
        }

        protected virtual string GenerateImplicitJoinWhere()
        {
            if (!Builder.Joins.Any())
                return "";
            return Builder.Joins.Select(j => j.Condition).Aggregate((current, next) => current + $" {Keyword(SqlKeyword.And)} " + next);
        }
        protected virtual string GenerateWhere()
        {
            var result = new StringBuilder($"{Keyword(SqlKeyword.Where)} ");
            var implicitJoinWhere = "";
            if (JoinStyle == JoinStyle.Implicit)
            {
                implicitJoinWhere = GenerateImplicitJoinWhere();
                result.Append(
                    $"{Symbol(SqlSymbols.StartGroup)}{implicitJoinWhere}{Symbol(SqlSymbols.EndGroup)}");
                if (Builder.Wheres.Any())
                    result.Append($" {Keyword(SqlKeyword.And)} ");
            }
            if (implicitJoinWhere == "" && !Builder.Wheres.Any())
                return "";

            var whereCount = 0;
            foreach (var w in Builder.Wheres)
            {
                if (whereCount > 0)
                {
                    if (w.PreOperator == WhereOperator.And)
                        result.Append($" {Keyword(SqlKeyword.And)} ");
                    else if (w.PreOperator == WhereOperator.Or)
                        result.Append($" {Keyword(SqlKeyword.Or)} ");
                }
                result.Append($"{w.Condition}");
                whereCount++;
            }
            return result.ToString();
        }

        protected virtual string GenerateOrderBy()
        {
            if (!Builder.OrderBys.Any())
                return "";

            var orderBy =
                string.Join(", ", Builder.OrderBys.Select(
                    o => $"{o.CompleteColumnName}" + (o.Direction == OrderByDirection.Asc ? " ASC" : " DESC"))
                    .ToArray());

            return $"{Keyword(SqlKeyword.OrderBy)} " + orderBy;
        }
        private string GenerateGroupBy()
        {
            if (!Builder.GroupBys.Any())
                return "";
            return $"{Keyword(SqlKeyword.GroupBy)} " + string.Join(", ", Builder.GroupBys.ToArray());
        }
        private string GenerateHaving()
        {
            if (!Builder.Havings.Any())
                return "";

            var result = new StringBuilder($"{Keyword(SqlKeyword.Having)} ");
            var count = 0;
            foreach (var w in Builder.Havings)
            {
                if (count > 0)
                {
                    if (w.PreOperator == WhereOperator.And)
                        result.Append($" {Keyword(SqlKeyword.And)} ");
                    else if (w.PreOperator == WhereOperator.Or)
                        result.Append($" {Keyword(SqlKeyword.Or)} ");
                }
                result.Append($"{w.Condition}");
            }
            return result.ToString();
        }

        public virtual string Generate()
        {
            var select = GenerateSelect();
            var source = GenerateSource();
            var where = GenerateWhere();
            var orderBy = GenerateOrderBy();
            var groupBy = GenerateGroupBy();
            var having = GenerateHaving();

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
