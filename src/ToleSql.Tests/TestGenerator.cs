using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators;
using ToleSql.Generator;
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

            b.SetMainTable("CUSTOMERS");

            var spec = "SELECT * FROM [CUSTOMERS] AS T0";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectTableWithSchemaAndAlias()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainTable("CUSTOMERS", "WH", "T1000");

            var spec = "SELECT * FROM [WH].[CUSTOMERS] AS T1000";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectColumnsFromTable()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainTable("CUSTOMERS");
            b.AddColumn("_ID", "ID").AddColumn("NAME");

            var spec = "SELECT _ID AS ID, NAME FROM [CUSTOMERS] AS T0";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectFromTableWithWhere()
        {
            var b = new SelectBuilder();
            var g = new SimpleGenerator(b);

            b.SetMainTable("CUSTOMERS");
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

            b.SetMainTable("CUSTOMERS");
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

            b.SetMainTable("CUSTOMERS");
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

            b.SetMainTable("CUSTOMERS");
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

            b.SetMainTable("CUSTOMERS");
            b.AddJoin("INVOICE", "INVOICE.CUSTOMERID = CUSTOMER.ID");

            var spec = "SELECT * FROM [CUSTOMERS] AS T0 INNER JOIN [INVOICE] AS T1 ON INVOICE.CUSTOMERID = CUSTOMER.ID";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectTableWithImplicitJoin()
        {
            var b = new SelectBuilder();
            var g = new ImplicitJoinGenerator(b);

            b.SetMainTable("CUSTOMERS");
            b.AddJoin("INVOICE", "INVOICE.CUSTOMERID = CUSTOMER.ID");
            b.AddJoin("INVOICEDETAIL", "INVOICE.ID = INVOICEDETAIL.ID");

            var spec = "SELECT * FROM [CUSTOMERS] AS T0, [INVOICE] AS T1, [INVOICEDETAIL] AS T2 WHERE (INVOICE.CUSTOMERID = CUSTOMER.ID AND INVOICE.ID = INVOICEDETAIL.ID)";
            var gen = g.Generate();
            Assert.Equal(spec, gen);
        }


    }
}
