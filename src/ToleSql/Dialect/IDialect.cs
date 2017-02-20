using ToleSql.SqlBuilder;

namespace ToleSql.Dialect
{
    public enum JoinStyle { Implicit, Explicit }
    public enum SqlKeyword { Select, As, AllColumns, From, InnerJoin, LeftJoin, On, Where, And, Or, Not, OrderBy, GroupBy, Having, In, Like, SubString, Sum, Count, Min, Max }
    public enum SqlComparison { Equal, NotEqual, LessThan, LessThanOrEqual, GreaterThan, GreaterThanOrEqual, Is, IsNot };
    public enum SqlArithmeticOperand { Add, Subtract, Multiply, Divide };
    public enum SqlSymbols { Comma, StartGroup, EndGroup, Wildchar }
    public interface IDialect
    {
        JoinStyle JoinStyle { get; }
        string Symbol(SqlSymbols symbol);
        string ComparisonSymbol(SqlComparison symbol);
        string ArithMeticOperand(SqlArithmeticOperand operand);
        string Keyword(SqlKeyword keyword);
        string GetSelectClausule(RawSelectBuilder builder);
        string GetSourceSqlWithAlias(string sourceExpression, string alias);
        string TableToSql(string tableName, string schemaName);
        string ColumnToSql(string tableName, string schemaName, string columnName);
        string ColumnToSql(string alias, string columnName);
        string AllColumnsFrom(string alias);
        string AlaisToSql(string alias);
        string GetJoinTypeLiteral(JoinType joinType);
        string GetSourceExplicitJoins(RawSelectBuilder builder);
        string GetImplicitJoin(RawSelectBuilder builder);
        string GetSource(RawSelectBuilder builder);
        string GetImplicitJoinWhere(RawSelectBuilder builder);
        string GetWhereClausule(RawSelectBuilder builder);
        string GetOrderByClausule(RawSelectBuilder builder);
        string GetGroupByClausule(RawSelectBuilder builder);
        string GetHavingClausule(RawSelectBuilder builder);
        string GetSelect(RawSelectBuilder builder);
        string GetParameterPrefix();
        string Quoted(string inner);
    }
}