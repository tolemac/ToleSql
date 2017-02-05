using System.Collections.Generic;

namespace ToleSql.Builder
{
    public class Join : Table
    {
        public List<string> Conditions { get; set; } = new List<string>();
        public Join(string tableName, string schemaName, string alias, string condition) : base(tableName, schemaName, alias)
        {
            Conditions.Add(condition);
        }
    }
}
