using Microsoft.EntityFrameworkCore;
using StocksApplication.Data;
using StocksApplication.Dtos.Stock;
using StocksApplication.Helpers;
using StocksApplication.Interfaces;
using StocksApplication.Mappers;
using StocksApplication.Models;

namespace StocksApplication.Repository
{
    public class StockRepository : IStockRepository
    {
        ApplicationDbContext _dbContext;
        public StockRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _dbContext.StocksContainer
                .Include(x => x.Comments)
                .ThenInclude(a => a.AppUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? 
                        stocks.OrderByDescending(x => x.Symbol) : 
                        stocks.OrderBy(x => x.Symbol);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _dbContext.StocksContainer.AddAsync(stockModel);
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _dbContext.StocksContainer.FirstOrDefaultAsync(x => x.Id == id);
            
            if (stockModel == null)
            {
                return null;
            }
            
            _dbContext.StocksContainer.Remove(stockModel);
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _dbContext.StocksContainer.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stockModel = await _dbContext.StocksContainer.FirstOrDefaultAsync(x => x.Id == id);

            if (stockModel == null)
            {
                return null;
            }

            stockModel = stockDto.ToStockFromUpdateDto(stockModel);
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<bool> IsStockExist(int id)
        {
            return await _dbContext.StocksContainer.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _dbContext.StocksContainer.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }
    }
}
