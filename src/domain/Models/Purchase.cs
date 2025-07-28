using System.Collections.Generic;
using System.Text.Json;
using System.Linq;

namespace Domain.Models
{
    public class OneProductPurchase
    {
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public float ProductCant { get; set; }
        public float TotalAmount => ProductPrice * ProductCant;
    }

    public class Purchase
    {
        public User Client { get; set; }
        public string ClientId { get; set; }
        public User Employee { get; set; }
        public string EmployeeId { get; set; }
        public DateTime When { get; set; }

        public string DescriptionText { get; set; }
        
        public IEnumerable<OneProductPurchase> Description => 
            JsonSerializer.Deserialize<IEnumerable<OneProductPurchase>>(DescriptionText);
        public float TotalAmount => Description.Sum(p => p.TotalAmount);
    }
}