using Domain.Enums;

namespace Application.Features.Storage.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public UnitOfMeasurement Unit { get; set; }
    }
}