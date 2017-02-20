using System;
using ToleSql.Model;

namespace ToleSql.Expressions
{
    internal class TableDefinition
    {
        public TableDefinition(Type modelType, string alias)
        {
            ModelType = modelType;
            Alias = alias;

            var tableName = modelType.Name;
            var schemaName = (string)null;
            if (Modeling.HasModel(modelType))
            {
                TableModel = Modeling.GetModel(modelType);
                tableName = TableModel.TableName ?? tableName;
                schemaName = TableModel.SchemaName ?? schemaName;
            }
            TableName = tableName;
            SchemaName = schemaName;
        }
        public Type ModelType { get; set; }
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public string Alias { get; set; }
        public TableModel TableModel { get; set; }
    }
}