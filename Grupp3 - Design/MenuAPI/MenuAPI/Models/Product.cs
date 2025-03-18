using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public string? ProductName { get; set; }

        [Required]
        public decimal? Price { get; set; }

        public bool IsVegetarian { get; set; } = false;

        public string? ImageURL { get; set; }
    }
}
