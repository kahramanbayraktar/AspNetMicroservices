using Ordering.Domain.Common;

namespace Ordering.Domain.Entities
{
    public class Order : EntityBase
    {
        public string? UserName { get; set; }
        // TODO: No store type was specified for the decimal property 'TotalPrice' on entity type 'Order'.
        // This will cause values to be silently truncated if they do not fit in the default precision and scale.
        // Explicitly specify the SQL server column type that can accommodate all the values in 'OnModelCreating' using 'HasColumnType',
        // specify precision and scale using 'HasPrecision', or configure a value converter using 'HasConversion'.
        public decimal TotalPrice { get; set; }

        // BillingAddress
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? AddressLine { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }

        // Payment
        public string? CardName { get; set; }
        public string? CardNumber { get; set; }
        public string? Expiration { get; set; }
        public string? CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}
