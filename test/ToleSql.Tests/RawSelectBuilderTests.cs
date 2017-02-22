using ToleSql.SqlBuilder;
using Xunit;

namespace ToleSql.Tests
{

    public class BuilderGeneratorTests
    {
        public BuilderGeneratorTests()
        {
        }

        [Fact]
        public void SelectTable()
        {
            Configuration.SetDialect(new TestDialect());
            var b = new RawSelectBuilder();

            b.FromSql("[CUSTOMERS]");

            var spec = "SELECT * FROM [CUSTOMERS] AS [T0]";
            var gen = b.GetSqlText();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectTableWithSchemaAndAlias()
        {
            Configuration.SetDialect(new TestDialect());
            var b = new RawSelectBuilder();

            b.FromSql("[WH].[CUSTOMERS]", "T1000");

            var spec = "SELECT * FROM [WH].[CUSTOMERS] AS [T1000]";
            var gen = b.GetSqlText();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectColumnsFromTable()
        {
            Configuration.SetDialect(new TestDialect());
            var b = new RawSelectBuilder();

            b.FromSql("[CUSTOMERS]");
            b.SelectSql("_ID", "ID").SelectSql("NAME");

            var spec = "SELECT _ID AS [ID], NAME FROM [CUSTOMERS] AS [T0]";
            var gen = b.GetSqlText();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectFromTableWithWhere()
        {
            Configuration.SetDialect(new TestDialect());
            var b = new RawSelectBuilder();

            b.FromSql("[CUSTOMERS]");
            b.WhereSql("NAME LIKE @PARAM1").WhereSql("ID > 100").WhereSql(WhereOperator.Or, "ID > 10000");

            var spec = "SELECT * FROM [CUSTOMERS] AS [T0] WHERE NAME LIKE @PARAM1 AND ID > 100 OR ID > 10000";
            var gen = b.GetSqlText();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectFromTableWithOrderBy()
        {
            Configuration.SetDialect(new TestDialect());
            var b = new RawSelectBuilder();

            b.FromSql("[CUSTOMERS]");
            b.OrderBySql("ID").OrderBySql(OrderByDirection.Desc, "NAME").OrderBySql(OrderByDirection.Asc, "BALANCE");

            var spec = "SELECT * FROM [CUSTOMERS] AS [T0] ORDER BY ID ASC, NAME DESC, BALANCE ASC";
            var gen = b.GetSqlText();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectFromTableWithGroupBy()
        {
            Configuration.SetDialect(new TestDialect());
            var b = new RawSelectBuilder();

            b.FromSql("[CUSTOMERS]");
            b.GroupBySql("COUNTRY").GroupBySql("TYPE");

            var spec = "SELECT * FROM [CUSTOMERS] AS [T0] GROUP BY COUNTRY, TYPE";
            var gen = b.GetSqlText();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectFromTableWithGroupByHaving()
        {
            Configuration.SetDialect(new TestDialect());
            var b = new RawSelectBuilder();

            b.FromSql("[CUSTOMERS]");
            b.GroupBySql("COUNTRY").GroupBySql("TYPE").HavingSql("COUNT(TOTAL) > 10");

            var spec = "SELECT * FROM [CUSTOMERS] AS [T0] GROUP BY COUNTRY, TYPE HAVING COUNT(TOTAL) > 10";
            var gen = b.GetSqlText();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectTableWithExplicitJoin()
        {
            Configuration.SetDialect(new TestDialect());
            var b = new RawSelectBuilder();

            b.FromSql("[CUSTOMERS]");
            b.JoinSql("[INVOICE]", "INVOICE.CUSTOMERID = CUSTOMER.ID");

            var spec = "SELECT * FROM [CUSTOMERS] AS [T0] INNER JOIN [INVOICE] AS [T1] ON INVOICE.CUSTOMERID = CUSTOMER.ID";
            var gen = b.GetSqlText();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectTableWithImplicitJoin()
        {
            Configuration.SetDialect(new TestImplicitJoinDialect());
            var b = new RawSelectBuilder();

            b.FromSql("[CUSTOMERS]");
            b.JoinSql("[INVOICE]", "INVOICE.CUSTOMERID = CUSTOMER.ID");
            b.JoinSql("[INVOICEDETAIL]", "INVOICE.ID = INVOICEDETAIL.ID");

            var spec = "SELECT * FROM [CUSTOMERS] AS [T0], [INVOICE] AS [T1], [INVOICEDETAIL] AS [T2] WHERE (INVOICE.CUSTOMERID = CUSTOMER.ID AND INVOICE.ID = INVOICEDETAIL.ID)";
            var gen = b.GetSqlText();
            Assert.Equal(spec, gen);
        }

        [Fact]
        public void SelectTableWithColumnsExplicitJoinWhereOrderBy()
        {
            Configuration.SetDialect(new TestDialect());
            var b = new RawSelectBuilder();

            b.FromSql("[WH].[INVOICE]");
            b.SelectSql("T0.DATE, T0.TOTAL");
            b.SelectSql("T1.NAME, T1.BALANCE");
            b.JoinSql("[CUS].[CUSTOMER]", "T1.CUSTOMERID = T2.ID");

            var spec = "SELECT T0.DATE, T0.TOTAL, T1.NAME, T1.BALANCE FROM [WH].[INVOICE] AS [T0] INNER JOIN [CUS].[CUSTOMER] AS [T1] ON T1.CUSTOMERID = T2.ID";
            var gen = b.GetSqlText();
            Assert.Equal(spec, gen);
        }

    }
}
