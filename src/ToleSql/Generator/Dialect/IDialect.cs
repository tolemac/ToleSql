using ToleSql.Builder;
using ToleSql.Builder.Definitions;

namespace ToleSql.Generator.Dialect
{
    public enum JoinStyle { Implicit, Explicit }
    public enum SqlKeyword { Select, As, AllColumns, From, InnerJoin, LeftJoin, On, Where, And, Or, Not, OrderBy, GroupBy, Having, In, Like, SubString }
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
        string GetSelectClausule(SelectBuilder builder);
        string GetSourceSqlWithAlias(string sourceExpression, string alias);
        string TableToSql(string tableName, string schemaName);
        string ColumnToSql(string tableName, string schemaName, string columnName);
        string ColumnToSql(string alias, string columnName);
        string AlaisToSql(string alias);
        string GetJoinTypeLiteral(JoinType joinType);
        string GetSourceExplicitJoins(SelectBuilder builder);
        string GetImplicitJoin(SelectBuilder builder);
        string GetSource(SelectBuilder builder);
        string GetImplicitJoinWhere(SelectBuilder builder);
        string GetWhereClausule(SelectBuilder builder);
        string GetOrderByClausule(SelectBuilder builder);
        string GetGroupByClausule(SelectBuilder builder);
        string GetHavingClausule(SelectBuilder builder);
        string GetSelect(SelectBuilder builder);
        string GetParameterPrefix();
        string Quoted(string inner);
    }
}