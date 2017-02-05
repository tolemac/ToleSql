namespace ToleSql.Builder
{
    public enum OrderByDirection { Asc, Desc}
    public class OrderBy
    {
        public OrderByDirection Direction { get; set; }
        public string CompleteColumnName { get; set; }

        public OrderBy(OrderByDirection direction, string completeColumnName)
        {
            Direction = direction;
            CompleteColumnName = completeColumnName;
        }
    }
}
