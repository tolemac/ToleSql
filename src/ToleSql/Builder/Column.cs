namespace ToleSql.Builder
{
    public class Column
    {
        public string ColumnName { get; private set; }
        public string ColumnAs { get; private set; }

        public Column(string columnName, string columnAs)
        {
            ColumnName = columnName;
            ColumnAs = columnAs;
        }
    }
}
