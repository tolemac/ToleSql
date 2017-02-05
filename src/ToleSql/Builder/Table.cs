namespace ToleSql.Builder
{
    public class Table
    {
        public string TableName { get; private set; }
        public string SchemaName { get; private set; }
        public string Alias { get; private set; }

        public Table(string tableName, string schemaName, string alias)
        {
            TableName = tableName;
            SchemaName = schemaName;
            Alias = alias;
        }
    }
}
