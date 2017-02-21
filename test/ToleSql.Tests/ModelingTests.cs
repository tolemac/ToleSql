using Xunit;

namespace ToleSql.Tests
{
    public class ModelingTests
    {
        [Fact]
        public void CanDefineSeveralModelsAndColumns()
        {
            Modeling.ResetModeling();
            Modeling.Model<Supplier>().SetSchema("CUS").SetTable("CUSTOMER");
            Modeling.Model(typeof(Supplier)).SetColumnName("Name", "NOMBRE");
            Modeling.Model<Supplier>().Ignore(c => c.PhoneNumber2);
            Modeling.Model<DeliveryNote>().SetSchema("WH");
            Modeling.Model<DeliveryNote>().SetTable("INVOICE");

            Assert.Equal("CUS", Modeling.Model<Supplier>().SchemaName);
            Assert.Equal("CUSTOMER", Modeling.Model<Supplier>().TableName);
            Assert.Equal("WH", Modeling.Model<DeliveryNote>().SchemaName);
            Assert.Equal("INVOICE", Modeling.Model<DeliveryNote>().TableName);
            Assert.Equal("NOMBRE", Modeling.Model<Supplier>().Column(c => c.Name).ColumnName);
            Assert.Equal("NOMBRE", Modeling.Model(typeof(Supplier)).Column("Name").ColumnName);
            Assert.Equal(true, Modeling.Model<Supplier>().Column(c => c.PhoneNumber2).Ignored);

            Assert.Equal(2, Modeling.Model<Supplier>().PropertyCount);
            Assert.Equal(2, Modeling.ModelListCount);
        }
        [Fact]
        public void CanSetModelingByBuilder()
        {
            Modeling.ResetModeling();
            Modeling.DefaultSchema = "KKK";
            Modeling.Model<Supplier>().SetSchema("CUS").SetTable("CUSTOMER");

            var b = new SelectBuilder();
            b.Modeling.Model<Supplier>().SetSchema("dbo").SetTable("MyTable");
            b.From<Supplier>();

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [dbo].[MyTable] AS [T0]";

            Assert.Equal(spec, gen);
        }
        [Fact]
        public void SelectWithDefaultSchema()
        {
            Modeling.ResetModeling();
            Modeling.DefaultSchema = "DBO";
            Configuration.SetDialect(new TestDialect());
            var b = new SelectFrom<DeliveryNote>();

            var gen = b.GetSqlText();
            var spec = "SELECT * FROM [DBO].[DeliveryNote] AS [T0]";

            Assert.Equal(spec, gen);
        }
    }
}