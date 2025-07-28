using Domain.Enums;

namespace Domain.Models
{
    public class ProductStore
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Store Store { get; set; }
        public int StoreId { get; set; }

        public float Cant { get; set; }
    }
}