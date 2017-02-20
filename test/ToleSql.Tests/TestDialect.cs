using ToleSql.Dialect;
using ToleSql.SqlServer;

namespace ToleSql.Tests
{
    public class TestDialect : SqlServerDialect
    {
    }

    public class TestImplicitJoinDialect : SqlServerDialect
    {
        public TestImplicitJoinDialect()
        {
            JoinStyle = JoinStyle.Implicit;
        }
    }
}
