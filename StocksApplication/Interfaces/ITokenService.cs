using StocksApplication.Models;

namespace StocksApplication.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user);
    }
}
