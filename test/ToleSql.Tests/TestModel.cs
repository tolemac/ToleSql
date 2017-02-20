using System;

namespace ToleSql.Tests
{
    public class DeliveryNoteDto
    {
        public virtual string Number { get; set; }
        public virtual Decimal TotalAmount { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string SupplierName { get; set; }
    }
    public class User
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual long CreatedBy { get; set; }

    }
    public class Supplier
    {
        public virtual long Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Town { get; set; }
        public virtual string City { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string PhoneNumber2 { get; set; }
        public virtual string Fax { get; set; }
        public virtual string Email { get; set; }
        public virtual string TaxCode { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
    public class DeliveryNote
    {
        public virtual long Id { get; set; }
        public virtual string Number { get; set; }
        public virtual long WarehouseId { get; set; }

        public virtual long? SupplierId { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Decimal TotalAmount { get; set; }
        public virtual string Year { get; set; }
        public virtual bool IsProcessed { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
    public class DeliveryNoteDetail
    {
        public virtual long Id { get; set; }
        public virtual long DeliveryNoteId { get; set; }
        public virtual long ProductId { get; set; }
        public virtual string Size { get; set; }
        public virtual int Amount { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual decimal BasePrice { get { return UnitPrice * Amount; } }
        public virtual string Location { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
    public class Product
    {
        public virtual long Id { get; set; }
        public virtual long SupplierProductId { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal PredefinedPrice { get; set; }
        public virtual decimal CostPrice { get; set; }
        public virtual long SeasonId { get; set; }
        public virtual string Sku { get; set; }
        public virtual bool IsDeleted { get; set; }
    }

    public class EquivalentProduct
    {
        public long ProductId { get; set; }
        public long EquivalentId { get; set; }
    }
}