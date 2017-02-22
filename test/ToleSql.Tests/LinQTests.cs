using Xunit;
using ToleSql;
using ToleSql.Functions;

namespace ToleSql.Tests
{
    public class LinQTests
    {
        [Fact]
        public void LinQSelect()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = from DeliveryNote d in new SelectFrom()
                    select d;

            var gen = b.GetSqlText();
            var spec = "SELECT [T0].* FROM [WH].[DeliveryNote] AS [T0]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void LinQWhere()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = from DeliveryNote d in new SelectFrom()
                    where d.TotalAmount > 100 && d.Year == "2017"
                    select d;

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] WHERE (([T0].[TotalAmount] > @SqlParam0) AND ([T0].[Year] = @SqlParam1))";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void LinQSelectProjection()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = from DeliveryNote d in new SelectFrom()
                    select new { TotalAmount = d.TotalAmount, SupplierName = d.Number };

            var gen = b.GetSqlText();
            var spec = "SELECT [T0].[TotalAmount] AS TotalAmount, [T0].[Number] AS SupplierName FROM [WH].[DeliveryNote] AS [T0]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void LinQSelectGroupBy()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = from DeliveryNote d in new SelectFrom()
                    group d by new { d.SupplierId, d.Year } into g
                    select new { Supplier = g.SupplierId, Year = g.Year, TotalAmount = DbFunctions.Sum(g.TotalAmount) };

            var gen = b.GetSqlText();
            var spec = "SELECT [T0].[SupplierId] AS Supplier, [T0].[Year] AS Year, SUM([T0].[TotalAmount]) AS TotalAmount FROM [WH].[DeliveryNote] AS [T0] GROUP BY [T0].[SupplierId], [T0].[Year]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void LinQSelectOrderBy()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = from DeliveryNote d in new SelectFrom()
                    orderby d.SupplierId, d.Date descending
                    select d;

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] ORDER BY [T0].[SupplierId] ASC, [T0].[Date] DESC";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void LinQSelectJoin()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = from d in new SelectFrom<DeliveryNote>()
                    join s in new SelectFrom<Supplier>() on d.SupplierId equals s.Id
                    select new { d, s };

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] INNER JOIN [WH].[Supplier] AS [T1] ON ([T0].[SupplierId] = [T1].[Id])";

            Assert.Equal(spec, gen);
        }
        // [Fact]
        // public void LinQSelectJoinMultipleKey()
        // {
        //     ExpressionsTests.SetModeling();
        //     Configuration.SetDialect(new TestDialect());
        //     var b = from DeliveryNote d in new SelectFrom()
        //             join Supplier s in new SelectFrom()
        //                 on (new { Id = d.SupplierId, Total = d.TotalAmount }) equals (new { Id = s.Id, Total = 0 })
        //             select new { d, s };

        //     var gen = b.GetSqlText();
        //     var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] INNER JOIN [WH].[Supplier] AS [T1] ON ([T0].[SupplierId] = [T1].[Id])";

        //     Assert.Equal(spec, gen);
        // }
    }
}