using Xunit;
using ToleSql.Functions;

namespace ToleSql.Tests
{
    public class SelectFromTests
    {
        [Fact]
        public void Select()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = new SelectFrom<DeliveryNote>();

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void Where()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = new SelectFrom<DeliveryNote>().Where(d => d.TotalAmount > 100 && d.Year == "2017");

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] WHERE (([T0].[TotalAmount] > @SqlParam0) AND ([T0].[Year] = @SqlParam1))";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectProjection()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = new SelectFrom<DeliveryNote>().Select(d => new { TotalAmount = d.TotalAmount, SupplierName = d.Number });

            var gen = b.GetSqlText();
            var spec = "SELECT [T0].[TotalAmount] AS TotalAmount, [T0].[Number] AS SupplierName FROM [WH].[DeliveryNote] AS [T0]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void GroupBy()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = new SelectFrom<DeliveryNote>()
                .GroupBy(d => new { d.SupplierId, d.Year })
                .Select(g => new { Supplier = g.SupplierId, Year = g.Year, TotalAmount = DbFunctions.Sum(g.TotalAmount) });

            var gen = b.GetSqlText();
            var spec = "SELECT [T0].[SupplierId] AS Supplier, [T0].[Year] AS Year, SUM([T0].[TotalAmount]) AS TotalAmount FROM [WH].[DeliveryNote] AS [T0] GROUP BY [T0].[SupplierId], [T0].[Year]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void OrderBy()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = new SelectFrom<DeliveryNote>()
                .OrderBy(d => d.SupplierId).ThenByDescending(d => d.Date);

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] ORDER BY [T0].[SupplierId] ASC, [T0].[Date] DESC";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void Join()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = new SelectFrom<DeliveryNote>().Join<Supplier>((d, s) => d.SupplierId == s.Id);

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] INNER JOIN [WH].[Supplier] AS [T1] ON ([T0].[SupplierId] = [T1].[Id])";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void JoinWithMultipleSelect()
        {
            ExpressionsTests.SetModeling();
            Configuration.SetDialect(new TestDialect());
            var b = new SelectFrom<DeliveryNote>()
                .Join<Supplier>((d, s) => d.SupplierId == s.Id)
                .Select((d, s) => d.Number + s.Name)
                .GroupBy((d, s) => d.Number + s.Name)
                .Join<User>((s, u) => s.CreatedBy == u.Id)
                .Select((s, u) => new { UserName = u.Name })
                .Select((s, u) => DbFunctions.Count(u.Name))
                .GroupBy((s, u) => u.Name)
                .OrderBy((s, u) => u.Name)
                .Having((s, u) => DbFunctions.Count(u.Name) > 1);

            var gen = b.GetSqlText();
            var spec = "SELECT ([T0].[Number] + [T1].[Name]), [T2].[Name] AS UserName, COUNT([T2].[Name]) FROM [WH].[DeliveryNote] AS [T0] INNER JOIN [WH].[Supplier] AS [T1] ON ([T0].[SupplierId] = [T1].[Id]) INNER JOIN [LoB].[SecurityProfile] AS [T2] ON ([T1].[CreatedBy_Id] = [T2].[Id]) GROUP BY ([T0].[Number] + [T1].[Name]), [T2].[Name] HAVING (COUNT([T2].[Name]) > @SqlParam0) ORDER BY [T2].[Name] ASC";

            Assert.Equal(spec, gen);
        }
    }
}
