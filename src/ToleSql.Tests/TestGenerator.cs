using ToleSql.Builder;
using Xunit;

namespace ToleSql.Tests
{

    public class TestGenerator
    {        
        public TestGenerator()
        {
        }

        [Fact]
        public void SelectTable()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainSource("[CUSTOMERS]");

            var spec = "SELECT * FROM [CUSTOMERS] AS T0";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectTableWithSchemaAndAlias()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainSource("[WH].[CUSTOMERS]", "T1000");

            var spec = "SELECT * FROM [WH].[CUSTOMERS] AS T1000";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectColumnsFromTable()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainSource("[CUSTOMERS]");
            b.AddColumnExpression("_ID", "ID").AddColumnExpression("NAME");

            var spec = "SELECT _ID AS ID, NAME FROM [CUSTOMERS] AS T0";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectFromTableWithWhere()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainSource("[CUSTOMERS]");
            b.AddWhere("NAME LIKE @PARAM1").AddWhere("ID > 100").AddWhere(WhereOperator.Or, "ID > 10000");

            var spec = "SELECT * FROM [CUSTOMERS] AS T0 WHERE NAME LIKE @PARAM1 AND ID > 100 OR ID > 10000";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectFromTableWithOrderBy()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainSource("[CUSTOMERS]");
            b.OrderBy("ID").OrderBy(OrderByDirection.Desc, "NAME").OrderBy(OrderByDirection.Asc, "BALANCE");

            var spec = "SELECT * FROM [CUSTOMERS] AS T0 ORDER BY ID ASC, NAME DESC, BALANCE ASC";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }        
        [Fact]
        public void SelectFromTableWithGroupBy()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainSource("[CUSTOMERS]");
            b.GroupBy("COUNTRY").GroupBy("TYPE");

            var spec = "SELECT * FROM [CUSTOMERS] AS T0 GROUP BY COUNTRY, TYPE";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }        
        [Fact]
        public void SelectFromTableWithGroupByHaving()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainSource("[CUSTOMERS]");
            b.GroupBy("COUNTRY").GroupBy("TYPE").AddHaving("COUNT(TOTAL) > 10");

            var spec = "SELECT * FROM [CUSTOMERS] AS T0 GROUP BY COUNTRY, TYPE HAVING COUNT(TOTAL) > 10";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectTableWithExplicitJoin()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainSource("[CUSTOMERS]");
            b.AddJoin("[INVOICE]", "INVOICE.CUSTOMERID = CUSTOMER.ID");

            var spec = "SELECT * FROM [CUSTOMERS] AS T0 INNER JOIN [INVOICE] AS T1 ON INVOICE.CUSTOMERID = CUSTOMER.ID";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectTableWithImplicitJoin()
        {
            var b = new SelectBuilder();
            var g = new ImplicitJoinGenerator(b);

            b.SetMainSource("[CUSTOMERS]");
            b.AddJoin("[INVOICE]", "INVOICE.CUSTOMERID = CUSTOMER.ID");
            b.AddJoin("[INVOICEDETAIL]", "INVOICE.ID = INVOICEDETAIL.ID");

            var spec = "SELECT * FROM [CUSTOMERS] AS T0, [INVOICE] AS T1, [INVOICEDETAIL] AS T2 WHERE (INVOICE.CUSTOMERID = CUSTOMER.ID AND INVOICE.ID = INVOICEDETAIL.ID)";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }

        [Fact]
        public void SelectTableWithColumnsExplicitJoinWhereOrderBy()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainSource("[WH].[INVOICE]");
            b.AddColumnExpression("T0.DATE, T0.TOTAL");
            b.AddColumnExpression("T1.NAME, T1.BALANCE");
            b.AddJoin("[CUS].[CUSTOMER]", "T1.CUSTOMERID = T2.ID");

            var spec = "SELECT T0.DATE, T0.TOTAL, T1.NAME, T1.BALANCE FROM [WH].[INVOICE] AS T0 INNER JOIN [CUS].[CUSTOMER] AS T1 ON T1.CUSTOMERID = T2.ID";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }



    }
}
