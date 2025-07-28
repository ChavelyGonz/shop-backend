using Domain.Enums;

namespace Domain.Models
{
    public class Store
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public IEnumerable<ProductStore> Products { get; set; }
    }
}