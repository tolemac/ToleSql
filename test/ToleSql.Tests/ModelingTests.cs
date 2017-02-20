using ToleSql;
using Xunit;

namespace ToleSql.Tests
{
    public class ModelingTests
    {
        // [Fact]
        // public void CanDefineSeveralModelsAndColumns()
        // {
        //     Modeling.ResetModeling();
        //     Modeling.Model<Supplier>().SetSchema("CUS").SetTable("CUSTOMER");
        //     Modeling.Model(typeof(Supplier)).SetColumnName("Name", "NOMBRE");
        //     Modeling.Model<Supplier>().Ignore(c => c.PhoneNumber2);
        //     Modeling.Model<DeliveryNote>().SetSchema("WH");
        //     Modeling.Model<DeliveryNote>().SetTable("INVOICE");

        //     Assert.Equal(Modeling.Model<Supplier>().SchemaName, "CUS");
        //     Assert.Equal(Modeling.Model<Supplier>().TableName, "CUSTOMER");
        //     Assert.Equal(Modeling.Model<DeliveryNote>().SchemaName, "WH");
        //     Assert.Equal(Modeling.Model<DeliveryNote>().TableName, "INVOICE");
        //     Assert.Equal(Modeling.Model<Supplier>().Column(c => c.Name).ColumnName, "NOMBRE");
        //     Assert.Equal(Modeling.Model(typeof(Supplier)).Column("Name").ColumnName, "NOMBRE");
        //     Assert.Equal(Modeling.Model<Supplier>().Column(c => c.PhoneNumber2).Ignored, true);

        //     Assert.Equal(Modeling.Model<Supplier>()._properties.Count, 2);
        //     Assert.Equal(Modeling._modelList.Count, 2);
        //     Modeling.ResetModeling();
        // }
    }
}