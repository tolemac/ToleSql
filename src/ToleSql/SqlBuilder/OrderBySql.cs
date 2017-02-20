namespace ToleSql.SqlBuilder
{
    public enum OrderByDirection { Asc, Desc }
    internal class OrderBySql
    {
        public OrderByDirection Direction { get; set; }
        public string Expression { get; set; }

        public OrderBySql(OrderByDirection direction, string expression)
        {
            Direction = direction;
            Expression = expression;
        }
    }
}
