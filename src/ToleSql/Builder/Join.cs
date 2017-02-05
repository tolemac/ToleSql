using System.Collections.Generic;

namespace ToleSql.Builder
{
    public enum JoinType
    {
        Inner,
        Left
    }

    public class Join : Table
    {
        public JoinType Type { get; set; }
        public string Condition { get; set; }

        public Join(JoinType type, string tableName, string schemaName, string alias, string condition) : base(tableName, schemaName, alias)
        {
            Type = type;
            Condition = condition;
        }
    }
}
