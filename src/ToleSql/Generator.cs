namespace ToleSql
{
    public static class Generator
    {
        public static string GetSqlText(this RawSelectBuilder builder)
        {
            return Configuration.Dialect.GetSelect(builder);
        }
    }
}