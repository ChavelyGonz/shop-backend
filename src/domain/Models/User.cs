using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<Purchase> Purchases { get; set; }
    }
}