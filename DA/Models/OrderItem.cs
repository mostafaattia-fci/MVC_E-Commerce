using DAL.Models;

namespace DA.Models
{
    public class OrderItem : BaseModel
    {
        public string OrderId { get; set; }
        public Order? Order { get; set; }

        public string ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
