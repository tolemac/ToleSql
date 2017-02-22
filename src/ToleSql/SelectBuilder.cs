using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using ToleSql.SqlBuilder;
using ToleSql.Expressions;
using ToleSql.Expressions.Visitors;
using ToleSql.Model;

namespace ToleSql
{
    public class SelectBuilder : RawSelectBuilder
    {
        private List<TableDefinition> tableDefinitions = new List<TableDefinition>();
        public TypeModeling Modeling { get; set; } = new TypeModeling();

        private TableDefinition GetOrAddTableDefinition(Type modelType, string alias)
        {
            var result = tableDefinitions.LastOrDefault(d => d.ModelType == modelType && d.Alias == alias);
            if (result == null)
            {
                result = new TableDefinition(modelType, alias, Modeling);
                tableDefinitions.Add(result);
            }
            return result;
        }
        private Dictionary<string, TableDefinition> GetTableDefinitionByParameterName(
            LambdaExpression expr, string[] aliases)
        {
            var result = new Dictionary<string, TableDefinition>();
            for (int i = 0; i < expr.Parameters.Count; i++)
            {
                var par = expr.Parameters[i];
                var alias = aliases[i];
                var definition = tableDefinitions.Single(td => td.ModelType == par.Type && td.Alias == alias);
                result.Add(par.Name, definition);
            }
            return result;
        }
        private string[] GetAliasesForTypes(Type[] types)
        {
            var aliases = new List<string>();
            foreach (var type in types)
                aliases.Add(tableDefinitions.Last(td => td.ModelType == type)?.Alias);
            return aliases.ToArray();
        }
        public SelectBuilder From<TEntity>()
        {
            return From<TEntity>(GetNextTableAlias());
        }
        public SelectBuilder From<TEntity>(string alias)
        {
            var td = GetOrAddTableDefinition(typeof(TEntity), alias);
            FromSql(Dialect.TableToSql(td.TableName, td.SchemaName), alias);
            return this;
        }
        internal SelectBuilder Join(JoinType joinType, string newAlias,
            Type newType, Type[] existingsTypes, LambdaExpression condition)
        {
            var aliases = new List<string>(GetAliasesForTypes(existingsTypes));
            var newTableDefinition = GetOrAddTableDefinition(newType, newAlias);
            aliases.Add(newAlias);

            var definitionsByParameterName = GetTableDefinitionByParameterName(condition, aliases.ToArray());
            var visitor = new Visitor(definitionsByParameterName, this);
            var conditionSql = visitor.GetSql(condition.Body);

            JoinSql(joinType,
                Dialect.TableToSql(newTableDefinition.TableName, newTableDefinition.SchemaName),
                newAlias, conditionSql);

            return this;
        }
        internal SelectBuilder Select(Type[] types, LambdaExpression[] expressions)
        {
            var aliases = GetAliasesForTypes(types);

            foreach (var expr in expressions)
            {
                var definitionsByParameterName = GetTableDefinitionByParameterName(expr, aliases.ToArray());
                var visitor = new Visitor(definitionsByParameterName, this);
                visitor.UseColumnAliases = true;
                SelectSql(visitor.GetSql(expr.Body));
            }
            return this;
        }
        internal SelectBuilder Where(WhereOperator preOperator, Type[] types, LambdaExpression expression)
        {
            var aliases = GetAliasesForTypes(types);
            var definitionsByParameterName = GetTableDefinitionByParameterName(expression, aliases);

            var visitor = new Visitor(definitionsByParameterName, this);
            var conditionSql = visitor.GetSql(expression.Body);

            WhereSql(preOperator, conditionSql);
            return this;
        }
        internal SelectBuilder OrderBy(OrderByDirection direction, Type[] types, LambdaExpression[] expressions)
        {
            var aliases = GetAliasesForTypes(types);

            foreach (var expr in expressions)
            {
                var definitionsByParameterName = GetTableDefinitionByParameterName(expr, aliases);
                var visitor = new Visitor(definitionsByParameterName, this);

                var orderbySql = visitor.GetSql(expr.Body);
                OrderBySql(direction, orderbySql);
            }
            return this;
        }
        internal SelectBuilder GroupBy(Type[] types, LambdaExpression[] expressions)
        {
            var aliases = GetAliasesForTypes(types);

            foreach (var expr in expressions)
            {
                var definitionsByParameterName = GetTableDefinitionByParameterName(expr, aliases);
                var visitor = new Visitor(definitionsByParameterName, this);

                var groupBySql = visitor.GetSql(expr.Body);
                GroupBySql(groupBySql);
            }
            return this;
        }
        internal SelectBuilder Having(WhereOperator preOperator, Type[] types, LambdaExpression expression)
        {
            var aliases = GetAliasesForTypes(types);
            var definitionsByParameterName = GetTableDefinitionByParameterName(expression, aliases);

            var visitor = new Visitor(definitionsByParameterName, this);
            var conditionSql = visitor.GetSql(expression.Body);

            HavingSql(preOperator, conditionSql);
            return this;
        }
    }
}
