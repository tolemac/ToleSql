namespace ToleSql
{
    /// <summary>
    /// Methods to be used only on expressions. They do nothing, only exists to be used for expression visitor.
    /// </summary>
    public static class SelectBuilderQueryExpressionMethods
    {
        /// <summary>
        /// Add Where In (...) clausule
        /// </summary>
        /// <param name="builder">Builder to be include as select</param>
        /// <param name="obj">Identifier</param>
        /// <returns></returns>
        public static bool Contains(this SelectBuilder builder, object obj)
        {
            return true;
        }
    }
}