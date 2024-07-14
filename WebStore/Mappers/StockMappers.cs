using StocksApplication.Dtos.Stock;
using StocksApplication.Models;

namespace StocksApplication.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            var stockDto = new StockDto 
            { 
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
            };
            return stockDto;
        } 

        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockDto)
        {
            var stock = new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                MarketCap = stockDto.MarketCap,
                Industry = stockDto.Industry,
            };
            return stock;
        }

        public static Stock ToStockFromUpdateDto(this UpdateStockRequestDto stockDto, Stock stock)
        {
            stock.Symbol = stockDto.Symbol;
            stock.CompanyName = stockDto.CompanyName;
            stock.Purchase = stockDto.Purchase;
            stock.LastDiv = stockDto.LastDiv;
            stock.MarketCap = stockDto.MarketCap;
            stock.Industry = stockDto.Industry;
            return stock;
        }
    }
}
