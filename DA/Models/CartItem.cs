using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace DA.Models
{
    public class CartItem : BaseModel
    {
        [Required]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public string ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
