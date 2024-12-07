using ProductSalesDemo.Models;

namespace ProductSalesDemo.ViewModel
{
    public class ProductionSalesViewModel
    {
        public int? CustomerId { get; set; }

        public string FirstName { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public int? Quantity { get; set; }

        public decimal? Discount { get; set; }

        public string? StoreName { get; set; }
    }
}
