using StocksApplication.Dtos.Stock;
using StocksApplication.Helpers;
using StocksApplication.Models;

namespace StocksApplication.Interfaces
{
    public interface IStockRepository
    {
        public Task<List<Stock>> GetAllAsync(QueryObject query);
        public Task<Stock?> GetByIdAsync(int id);
        public Task<Stock?> GetBySymbolAsync(string symbol);
        public Task<Stock> CreateAsync(Stock stockModel);
        public Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        public Task<Stock?> DeleteAsync(int id);
        public Task<bool> IsStockExist(int id); 
    }
}
