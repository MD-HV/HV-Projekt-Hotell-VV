using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAPI.Models
{
    public class OrderProduct
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Amount { get; set; }

        // Navigationsproperties
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
