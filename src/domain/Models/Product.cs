using Domain.Enums;

namespace Domain.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public float Price { get; set; }
        public IEnumerable<ProductStore> Stores { get; set; }

        public UnitOfMeasurement Unit { get; set; }
    }
}