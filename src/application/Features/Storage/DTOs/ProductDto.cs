using Domain.Enums;
using Domain.Interfaces;

namespace Application.Features.Storage.DTOs
{
    public class ProductDto : IHasId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public UnitOfMeasurement Unit { get; set; }
    }
}