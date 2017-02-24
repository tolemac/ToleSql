using System.Collections.Generic;

namespace ToleSql
{
    public interface IBuilder
    {
        IDictionary<string, object> Parameters { get; }
        string GetSqlText();
    }
}