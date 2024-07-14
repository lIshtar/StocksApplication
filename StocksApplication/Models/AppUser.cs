using Microsoft.AspNetCore.Identity;

namespace StocksApplication.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
