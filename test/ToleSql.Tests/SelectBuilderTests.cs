using Xunit;
using ToleSql.SqlBuilder;
using System;
using ToleSql.Functions;

namespace ToleSql.Tests
{
    public class ExpressionsTests
    {
        public ExpressionsTests()
        {
        }

        private void SetModeling()
        {
            Modeling.ResetModeling();
            Modeling.Model<Supplier>().SetSchema("WH");
            Modeling.Model<DeliveryNote>().SetSchema("WH");
            Modeling.Model<DeliveryNoteDetail>().SetSchema("WH");
            Modeling.Model<Product>().SetSchema("WH");
            Modeling.Model<EquivalentProduct>().SetSchema("WH");
            Modeling.Model<DeliveryNoteDetail>().SetColumnName(dnd => dnd.DeliveryNoteId, "DeliveryNote_Id");
            Modeling.Model<User>()
                .SetSchema("LoB")
                .SetTable("SecurityProfile")
                .SetColumnName(u => u.CreatedBy, "CreatedBy_Id");
        }
        [Fact]
        public void SelectFromTable()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectFromTableWithAlias()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>("T100");

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T100]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectColumnsFromTable()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();
            b.Select<DeliveryNote>(i => i.Number, i => i.Date);

            var gen = b.GetSqlText();
            var spec = "SELECT [T0].[Number], [T0].[Date] FROM [WH].[DeliveryNote] AS [T0]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectColumnsWithSubString()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();
            b.Select<DeliveryNote>(i => i.Number.Substring(0, 3));

            var gen = b.GetSqlText();
            var spec = "SELECT SUBSTRING([T0].[Number],@SqlParam0,@SqlParam1) FROM [WH].[DeliveryNote] AS [T0]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectColumnsFromTableWithAlias()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>("T100");
            b.Select<DeliveryNote>(i => i.Number, i => i.Date);

            var gen = b.GetSqlText();
            var spec = "SELECT [T100].[Number], [T100].[Date] FROM [WH].[DeliveryNote] AS [T100]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectJoinTwoTables()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();
            b.AddJoin<DeliveryNote, DeliveryNoteDetail>((i, id) => i.Id == id.DeliveryNoteId);

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] INNER JOIN [WH].[DeliveryNoteDetail] AS [T1] ON ([T0].[Id] = [T1].[DeliveryNote_Id])";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectLeftJoinTwoTables()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();
            b.AddJoin<DeliveryNote, DeliveryNoteDetail>(JoinType.Left, (i, id) => i.Id == id.DeliveryNoteId);

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] LEFT JOIN [WH].[DeliveryNoteDetail] AS [T1] ON ([T0].[Id] = [T1].[DeliveryNote_Id])";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectJoinTwoTablesWithAliases()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>("T11");
            b.AddJoin<DeliveryNote, DeliveryNoteDetail>("T22", (i, id) => i.Id == id.DeliveryNoteId || i.TotalAmount >= id.UnitPrice);

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T11] INNER JOIN [WH].[DeliveryNoteDetail] AS [T22] ON (([T11].[Id] = [T22].[DeliveryNote_Id]) OR ([T11].[TotalAmount] >= [T22].[UnitPrice]))";

            Assert.Equal(spec, gen);
        }

        [Fact]
        public void SelectColumnsFromJoinTwoTablesWithAliases()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>("T11");
            b.AddJoin<DeliveryNote, DeliveryNoteDetail>("T22", (i, id) => i.Id == id.DeliveryNoteId || i.TotalAmount >= id.UnitPrice);
            b.Select<DeliveryNote>((i) => i.Date, (i) => i.Number);
            b.Select<DeliveryNoteDetail>((id) => id.UnitPrice, (id) => id.Amount);
            b.Select<DeliveryNote, DeliveryNoteDetail>((i, id) => i.TotalAmount + id.Amount);

            var gen = b.GetSqlText();
            var spec = "SELECT [T11].[Date], [T11].[Number], [T22].[UnitPrice], [T22].[Amount], ([T11].[TotalAmount] + [T22].[Amount]) FROM [WH].[DeliveryNote] AS [T11] INNER JOIN [WH].[DeliveryNoteDetail] AS [T22] ON (([T11].[Id] = [T22].[DeliveryNote_Id]) OR ([T11].[TotalAmount] >= [T22].[UnitPrice]))";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectSelfTableJoin()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<User>("T11");
            b.AddJoin<User, User>("T22", (c1, c2) => c1.CreatedBy == c2.Id);

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [LoB].[SecurityProfile] AS [T11] INNER JOIN [LoB].[SecurityProfile] AS [T22] ON ([T11].[CreatedBy_Id] = [T22].[Id])";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectWithWhere()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();
            b.Where<DeliveryNote>(i => i.Number == null && i.Year == "B" && i.Id > 2 && i.TotalAmount == 0);

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] WHERE (((([T0].[Number] IS NULL) AND ([T0].[Year] = @SqlParam0)) AND ([T0].[Id] > @SqlParam1)) AND ([T0].[TotalAmount] = @SqlParam2))";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters["SqlParam0"], "B");
            Assert.Equal(b.Parameters["SqlParam1"], Convert.ToInt64(2));
            Assert.Equal(b.Parameters["SqlParam2"], (Decimal)0);
        }

        [Fact]
        public void SelectWithWhereWithLike()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<Supplier>();
            b.Where<Supplier>(i => i.Name.Contains("Javier"));

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[Supplier] AS [T0] WHERE [T0].[Name] LIKE '%' + @SqlParam0 + '%'";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters["SqlParam0"], "Javier");
        }
        [Fact]
        public void SelectWithWhereWithStartsWith()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<Supplier>();
            b.Where<Supplier>(i => i.Name.StartsWith("Javier"));

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[Supplier] AS [T0] WHERE [T0].[Name] LIKE @SqlParam0 + '%'";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters["SqlParam0"], "Javier");
        }
        [Fact]
        public void SelectWithWhereWithEndsWith()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<Supplier>();
            b.Where<Supplier>(i => i.Name.EndsWith("Javier"));

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[Supplier] AS [T0] WHERE [T0].[Name] LIKE '%' + @SqlParam0";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters["SqlParam0"], "Javier");
        }
        [Fact]
        public void SelectWithWhereWithLikeAndSubString()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<Supplier>();
            b.Where<Supplier>(i => i.Name.Substring(0).Contains("Javier"));

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[Supplier] AS [T0] WHERE SUBSTRING([T0].[Name],@SqlParam0,@SqlParam1) LIKE '%' + @SqlParam2 + '%'";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters["SqlParam0"], 0);
            Assert.Equal(b.Parameters["SqlParam1"], int.MaxValue);
            Assert.Equal(b.Parameters["SqlParam2"], "Javier");
        }
        [Fact]
        public void SelectWithWhereWithInvertedLike()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<Supplier>();
            var name = "Javier Ros Moreno";
            b.Where<Supplier>(i => name.Contains(i.Name));

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[Supplier] AS [T0] WHERE @SqlParam0 LIKE '%' + [T0].[Name] + '%'";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters["SqlParam0"], "Javier Ros Moreno");
        }
        [Fact]
        public void SelectWithWhereWithScopeConstants()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();
            var year = "B";
            var id = 2;
            var total = 0;
            string number = "23";
            b.Where<DeliveryNote>(i => i.Number == number && i.Year == year && i.Id > id && i.TotalAmount == total);

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] WHERE (((([T0].[Number] = @SqlParam0) AND ([T0].[Year] = @SqlParam1)) AND ([T0].[Id] > @SqlParam2)) AND ([T0].[TotalAmount] = @SqlParam3))";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters["SqlParam0"], "23");
            Assert.Equal(b.Parameters["SqlParam1"], "B");
            Assert.Equal(b.Parameters["SqlParam2"], Convert.ToInt32(2));
            Assert.Equal(b.Parameters["SqlParam3"], (Int32)0);
        }
        [Fact]
        public void SelectWithWhereWithNullScopeConstants()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>("T0");
            string year = null;
            long? id = null;
            decimal? total = null;
            string number = null;
            b.Where<DeliveryNote>(i => i.Number == number && i.Year == year && i.Id == id && i.TotalAmount == total);

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] WHERE (((([T0].[Number] IS NULL) AND ([T0].[Year] IS NULL)) AND ([T0].[Id] IS NULL)) AND ([T0].[TotalAmount] IS NULL))";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters.Count, 0);
        }
        [Fact]
        public void SelectColumnsJoinsAndWhere()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();
            b.AddJoin<DeliveryNote, DeliveryNoteDetail>((i, id) => i.Id == id.DeliveryNoteId);
            b.AddJoin<DeliveryNote, Supplier>((i, c) => i.SupplierId == c.Id);
            b.Select<DeliveryNote, Supplier>((i, c) => i.Number, (i, c) => c.Name, (i, c) => i.Date, (i, c) => i.TotalAmount);
            b.Select<DeliveryNoteDetail>((id) => id.Size);
            b.Where<Supplier>(c => c.IsDeleted);
            b.Where<DeliveryNote, DeliveryNoteDetail>((i, id) => i.Year == "B" && id.IsDeleted == false);
            b.Where<DeliveryNote, DeliveryNoteDetail>(WhereOperator.Or, (i, id) => i.Year == "Z");

            var gen = b.GetSqlText();
            var spec = "SELECT [T0].[Number], [T2].[Name], [T0].[Date], [T0].[TotalAmount], [T1].[Size] FROM [WH].[DeliveryNote] AS [T0] INNER JOIN [WH].[DeliveryNoteDetail] AS [T1] ON ([T0].[Id] = [T1].[DeliveryNote_Id]) INNER JOIN [WH].[Supplier] AS [T2] ON ([T0].[SupplierId] = [T2].[Id]) WHERE ([T2].[IsDeleted] = @SqlParam0) AND (([T0].[Year] = @SqlParam1) AND ([T1].[IsDeleted] = @SqlParam2)) OR ([T0].[Year] = @SqlParam3)";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters["SqlParam0"], true);
            Assert.Equal(b.Parameters["SqlParam1"], "B");
            Assert.Equal(b.Parameters["SqlParam2"], false);
            Assert.Equal(b.Parameters["SqlParam3"], "Z");
        }
        [Fact]
        public void SelectWithOrderBy()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();
            b.OrderBy<DeliveryNote>(OrderByDirection.Desc, i => i.Date);
            b.OrderBy<DeliveryNote>(i => i.Year, i => i.SupplierId);

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] ORDER BY [T0].[Date] DESC, [T0].[Year] ASC, [T0].[SupplierId] ASC";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectWithSubQuery()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            var f = false;
            var subQuery = new SelectBuilder().SetMainTable<Supplier>().Where<Supplier>(c => c.IsDeleted == f).Select<Supplier>(c => c.Id);

            b.SetMainTable<DeliveryNote>();
            b.Where<DeliveryNote>(i => subQuery.Contains(i.SupplierId) && i.Number == "hola");

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] WHERE ([T0].[SupplierId] IN (SELECT [T0].[Id] FROM [WH].[Supplier] AS [T0] WHERE ([T0].[IsDeleted] = @SqlParam0)) AND ([T0].[Number] = @SqlParam1))";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectWithMultipleSubQueryAndParameters()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            var const1 = false;
            var const2 = false;
            var subQuery1 = new SelectBuilder().SetMainTable<Supplier>().Where<Supplier>(c => c.IsDeleted == const1).Select<Supplier>(c => c.Id);
            var subQuery2 = new SelectBuilder().SetMainTable<Supplier>().Where<Supplier>(c => c.IsDeleted == const2).Where<Supplier>(c => subQuery1.Contains(c.Id)).Select<Supplier>(c => c.Id);

            b.SetMainTable<DeliveryNote>();
            b.Where<DeliveryNote>(i => subQuery1.Contains(i.SupplierId) && i.Number == "hola1");
            b.Where<DeliveryNote>(i => subQuery2.Contains(i.SupplierId) && i.Number == "hola2");

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] WHERE ([T0].[SupplierId] IN (SELECT [T0].[Id] FROM [WH].[Supplier] AS [T0] WHERE ([T0].[IsDeleted] = @SqlParam0)) AND ([T0].[Number] = @SqlParam1)) AND ([T0].[SupplierId] IN (SELECT [T0].[Id] FROM [WH].[Supplier] AS [T0] WHERE ([T0].[IsDeleted] = @SqlParam2) AND [T0].[Id] IN (SELECT [T0].[Id] FROM [WH].[Supplier] AS [T0] WHERE ([T0].[IsDeleted] = @SqlParam3))) AND ([T0].[Number] = @SqlParam4))";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters["SqlParam0"], const1);
            Assert.Equal(b.Parameters["SqlParam1"], "hola1");
            Assert.Equal(b.Parameters["SqlParam2"], const2);
            Assert.Equal(b.Parameters["SqlParam3"], const1);
            Assert.Equal(b.Parameters["SqlParam4"], "hola2");
        }
        // [Fact]
        // public void SelectWithSubQueryWithParamPerRow()
        // {
        //     SqlConfiguration.SetDialect(new TestDialect());
        //     SetModeling();
        //     var b = new ExpressionSelectBuilder();
        //     var subQuery = new ExpressionSelectBuilder().SetMainTable<Supplier>().Select<Supplier>(c => c.Id);

        //     b.SetMainTable<DeliveryNote>();
        //     b.Where<DeliveryNote>(i => subQuery.Where<Supplier>(s => s.Id == i.SupplierId).Contains(i.SupplierId));

        //     var gen = b.GetSqlText();
        //     var spec = "SELECT * FROM [WH].[DeliveryNote] AS [T0] WHERE [T0].[SupplierId] IN (SELECT [T0].[Id] FROM [WH].[Supplier] AS [T0] WHERE [T0].[Id] = [T0].[SupplierId])";

        //     Assert.Equal(spec, gen);
        // }

        [Fact]
        public void SelectProjection()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();
            b.AddJoin<DeliveryNote, Supplier>((dn, s) => dn.SupplierId == s.Id);
            b.Select<DeliveryNote, Supplier>((dn, s) => new DeliveryNoteDto
            {
                Number = dn.Number,
                TotalAmount = dn.TotalAmount,
                Date = dn.Date,
                SupplierName = s.Name
            });

            var gen = b.GetSqlText();
            var spec = "SELECT [T0].[Number] AS Number, [T0].[TotalAmount] AS TotalAmount, [T0].[Date] AS Date, [T1].[Name] AS SupplierName FROM [WH].[DeliveryNote] AS [T0] INNER JOIN [WH].[Supplier] AS [T1] ON ([T0].[SupplierId] = [T1].[Id])";

            Assert.Equal(spec, gen);
        }

        [Fact]
        public void SelectGroupByHaving()
        {
            Configuration.SetDialect(new TestDialect());
            SetModeling();
            var b = new SelectBuilder();
            b.SetMainTable<DeliveryNote>();
            b.Select<DeliveryNote>(dn => dn.SupplierId);
            b.Select<DeliveryNote>(dn => DbFunctions.Sum(dn.TotalAmount));
            b.Select<DeliveryNote>(dn => DbFunctions.Count(dn.TotalAmount));
            b.Select<DeliveryNote>(dn => DbFunctions.Min(dn.TotalAmount));
            b.Select<DeliveryNote>(dn => DbFunctions.Max(dn.TotalAmount));
            b.GroupBy<DeliveryNote>(dn => dn.SupplierId);
            b.Having<DeliveryNote>(dn => DbFunctions.Sum(dn.TotalAmount) > 10);

            var gen = b.GetSqlText();
            var spec = "SELECT [T0].[SupplierId], SUM([T0].[TotalAmount]), COUNT([T0].[TotalAmount]), MIN([T0].[TotalAmount]), MAX([T0].[TotalAmount]) FROM [WH].[DeliveryNote] AS [T0] GROUP BY [T0].[SupplierId] HAVING (SUM([T0].[TotalAmount]) > @SqlParam0)";

            Assert.Equal(spec, gen);
            Assert.Equal(b.Parameters["SqlParam0"], (decimal)10);
        }
    }
}
