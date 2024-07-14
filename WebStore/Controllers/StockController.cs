using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using StocksApplication.Data;
using StocksApplication.Dtos.Stock;
using StocksApplication.Helpers;
using StocksApplication.Interfaces;
using StocksApplication.Mappers;
using StocksApplication.Models;

namespace StocksApplication.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStockRepository _stockRepository;
        public StockController(ApplicationDbContext dbContext, IStockRepository stockRepository)
        {
            _dbContext = dbContext;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var stocks = await _stockRepository.GetAllAsync(query);
            var stocksDto = stocks.Select(stock => stock.ToStockDto());

            if (stocks == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(stocks);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Stock? stock = await _stockRepository.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(stock.ToStockDto());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepository.CreateAsync(stockModel);
            return CreatedAtAction(
                nameof(GetById), 
                new { id = stockModel.Id },
                stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            var stockModel = await _stockRepository.UpdateAsync(id, stockDto);

            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _stockRepository.DeleteAsync(id);

            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
