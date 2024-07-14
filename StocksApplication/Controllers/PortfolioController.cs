using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksApplication.Extensions;
using StocksApplication.Interfaces;
using StocksApplication.Models;

namespace StocksApplication.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        public PortfolioController(UserManager<AppUser> userManager,
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepository = stockRepository;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepository.GetUserPortfolioAsync(appUser);
            return Ok(userPortfolio);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string stockSymbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var stock = await _stockRepository.GetBySymbolAsync(stockSymbol);

            if (stock == null)
            {
                return BadRequest("Stock not found");
            }

            var userPortfolio = await _portfolioRepository.GetUserPortfolioAsync(appUser);
            if (userPortfolio != null && userPortfolio.Any(e => e.Symbol == stockSymbol))
            {
                return BadRequest("The stock already in portfolio");
            }

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id,
            };

            await _portfolioRepository.CreateAsync(portfolioModel);

            if (userPortfolio == null)
            {
                return StatusCode(500, "Could not create");
            }

            return Created();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string stockSymbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepository.GetUserPortfolioAsync(appUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol == stockSymbol).ToList();

            if (filteredStock.Count() == 1)
            {
                await _portfolioRepository.DeleteAsync(appUser, stockSymbol);
            }
            else
            {
                return BadRequest("Stock not in portfolio");
            }

            return Ok();
        }
    }
}
