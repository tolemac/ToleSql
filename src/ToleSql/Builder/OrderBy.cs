namespace ToleSql.Builder
{
    public enum OrderByDirection { Asc, Desc}
    public class OrderBy
    {
        public OrderByDirection Direction { get; set; }
        public string ColumnNameOrAlias { get; set; }

        public OrderBy(OrderByDirection direction, string columnNameOrAlias)
        {
            Direction = direction;
            ColumnNameOrAlias = columnNameOrAlias;
        }
    }
}
