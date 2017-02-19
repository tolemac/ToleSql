namespace ToleSql.Builder.Definitions
{
    public enum OrderByDirection { Asc, Desc }
    internal class OrderBy
    {
        public OrderByDirection Direction { get; set; }
        public string Expression { get; set; }

        public OrderBy(OrderByDirection direction, string expression)
        {
            Direction = direction;
            Expression = expression;
        }
    }
}
