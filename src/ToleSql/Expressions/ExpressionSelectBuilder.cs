using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using ToleSql.Builder;
using ToleSql.Expressions.Visitors;
using ToleSql.Builder.Definitions;

namespace ToleSql.Expressions
{
    public class ExpressionSelectBuilder : SelectBuilder
    {
        private List<TableDefinition> tableDefinitions = new List<TableDefinition>();

        internal TableDefinition GetOrAddTableDefinition(Type modelType, string alias)
        {
            var result = tableDefinitions.LastOrDefault(d => d.ModelType == modelType && d.Alias == alias);
            if (result == null)
            {
                result = new TableDefinition(modelType, alias);
                tableDefinitions.Add(result);
            }
            return result;
        }

        //internal string SubQueryParametersPrefix { get; set; } = "Sub0";
        public ExpressionSelectBuilder SetMainTable<TEntity>(string alias)
        {
            var td = GetOrAddTableDefinition(typeof(TEntity), alias);
            SetMainSourceSql(Dialect.TableToSql(td.TableName, td.SchemaName), alias);
            return this;
        }
        public ExpressionSelectBuilder SetMainTable<TEntity>()
        {
            return SetMainTable<TEntity>(GetNextTableAlias());
        }
        public ExpressionSelectBuilder AddJoin<TEntity1, TEntity2>(Expression<Func<TEntity1, TEntity2, bool>> condition)
        {
            return AddJoin(JoinType.Inner, GetNextTableAlias(), condition);
        }
        public ExpressionSelectBuilder AddJoin<TEntity1, TEntity2>(string alias,
            Expression<Func<TEntity1, TEntity2, bool>> condition)
        {
            return AddJoin(JoinType.Inner, alias, condition);
        }
        public ExpressionSelectBuilder AddJoin<TEntity1, TEntity2>(JoinType joinType,
            Expression<Func<TEntity1, TEntity2, bool>> condition)
        {
            return AddJoin(joinType, GetNextTableAlias(), condition);
        }

        public ExpressionSelectBuilder AddJoin<TEntity, TJoin>(JoinType joinType, string alias,
            Expression<Func<TEntity, TJoin, bool>> condition)
        {
            var tableSourceDefinition = tableDefinitions.Last(td => td.ModelType == typeof(TEntity));
            var tableJoinDefinition = GetOrAddTableDefinition(typeof(TJoin), alias);
            var aliases = new[] { tableSourceDefinition.Alias, tableJoinDefinition.Alias };

            var definitionsByParameterName = GetTableDefinitionByParameterName(condition, aliases);
            var visitor = new Visitor(definitionsByParameterName, this);

            var conditionSql = visitor.GetSql(condition.Body);

            AddJoinSql(joinType, Dialect.TableToSql(tableJoinDefinition.TableName, tableJoinDefinition.SchemaName), alias, conditionSql);
            return this;
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
        public ExpressionSelectBuilder Select<TEntity>(params Expression<Func<TEntity, object>>[] expressions)
        {
            var aliases = tableDefinitions.Where(td => td.ModelType == typeof(TEntity)).Select(td => td.Alias).ToArray();
            foreach (var expr in expressions)
            {
                var definitionsByParameterName = GetTableDefinitionByParameterName(expr, aliases);
                var visitor = new Visitor(definitionsByParameterName, this);

                AddColumnSql(visitor.GetSql(expr.Body));
            }
            return this;
        }
        public ExpressionSelectBuilder Select<TEntity1, TEntity2>(params Expression<Func<TEntity1, TEntity2, object>>[] expressions)
        {
            var alias1 = tableDefinitions.Single(td => td.ModelType == typeof(TEntity1)).Alias;
            var alias2 = tableDefinitions.Single(td => td.ModelType == typeof(TEntity2)).Alias;
            var aliases = new[] { alias1, alias2 };
            foreach (var expr in expressions)
            {
                var definitionsByParameterName = GetTableDefinitionByParameterName(expr, aliases);
                var visitor = new Visitor(definitionsByParameterName, this);

                AddColumnSql(visitor.GetSql(expr.Body));
            }
            return this;
        }

        public ExpressionSelectBuilder Where<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            return Where(WhereOperator.And, expression);
        }
        public ExpressionSelectBuilder Where<TEntity>(WhereOperator preOperator,
                    Expression<Func<TEntity, bool>> expression)
        {
            var tableDefinition = tableDefinitions.Last(td => td.ModelType == typeof(TEntity));
            var aliases = new[] { tableDefinition.Alias };

            var definitionsByParameterName = GetTableDefinitionByParameterName(expression, aliases);
            var visitor = new Visitor(definitionsByParameterName, this);

            var conditionSql = visitor.GetSql(expression.Body);

            AddWhereSql(preOperator, conditionSql);
            return this;
        }

        public ExpressionSelectBuilder Where<TEntity1, TEntity2>(Expression<Func<TEntity1, TEntity2, bool>> expression)
        {
            return Where(WhereOperator.And, expression);
        }

        public ExpressionSelectBuilder Where<TEntity1, TEntity2>(WhereOperator preOperator,
                    Expression<Func<TEntity1, TEntity2, bool>> expression)
        {
            var tableDefinition1 = tableDefinitions.Last(td => td.ModelType == typeof(TEntity1));
            var tableDefinition2 = tableDefinitions.Last(td => td.ModelType == typeof(TEntity2));
            var aliases = new[] { tableDefinition1.Alias, tableDefinition2.Alias };

            var definitionsByParameterName = GetTableDefinitionByParameterName(expression, aliases);
            var visitor = new Visitor(definitionsByParameterName, this);

            var conditionSql = visitor.GetSql(expression.Body);

            AddWhereSql(preOperator, conditionSql);
            return this;
        }

        public ExpressionSelectBuilder OrderBy<TEntity>(params Expression<Func<TEntity, object>>[] expressions)
        {
            return OrderBy(OrderByDirection.Asc, expressions);
        }

        public ExpressionSelectBuilder OrderBy<TEntity>(OrderByDirection direction, params Expression<Func<TEntity, object>>[] expressions)
        {
            var tableDefinition = tableDefinitions.Last(td => td.ModelType == typeof(TEntity));
            var aliases = new[] { tableDefinition.Alias };

            foreach (var expr in expressions)
            {
                var definitionsByParameterName = GetTableDefinitionByParameterName(expr, aliases);
                var visitor = new Visitor(definitionsByParameterName, this);

                var orderbySql = visitor.GetSql(expr.Body);
                AddOrderBySql(direction, orderbySql);
            }
            return this;
        }
    }
}
