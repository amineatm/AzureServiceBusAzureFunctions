using System.ComponentModel.DataAnnotations;

namespace ServiceBusShared.Models
{
    public class Product
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Reference { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
    }
}
