using System;
using ToleSql.Model;

namespace ToleSql.Expressions
{
    internal class TableDefinition
    {
        public TableDefinition(Type modelType, string alias, TypeModeling builderModeling)
        {
            ModelType = modelType;
            Alias = alias;

            var tableName = modelType.Name;
            var schemaName = builderModeling.DefaultSchema ?? Modeling.DefaultSchema ?? null;
            // First global modeling
            if (Modeling.HasModel(modelType))
            {
                _tableModel = Modeling.GetModel(modelType);
                tableName = _tableModel.TableName ?? tableName;
                schemaName = _tableModel.SchemaName ?? schemaName;
            }
            // By the end check builder modeling
            if (builderModeling.HasModel(modelType))
            {
                _builderTableModel = builderModeling.GetModel(modelType);
                tableName = _builderTableModel.TableName ?? tableName;
                schemaName = _builderTableModel.SchemaName ?? schemaName;
            }
            TableName = tableName;
            SchemaName = schemaName;
        }
        public Type ModelType { get; set; }
        public string TableName { get; set; }
        public string SchemaName { get; set; }
        public string Alias { get; set; }

        internal string GetColumnName(string name)
        {
            return _builderTableModel?.Column(name)?.ColumnName ?? _tableModel?.Column(name)?.ColumnName;
        }
        private TableModel _tableModel;
        private TableModel _builderTableModel;
    }
}