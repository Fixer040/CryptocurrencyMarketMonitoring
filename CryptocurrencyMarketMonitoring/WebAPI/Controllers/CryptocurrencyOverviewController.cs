using CryptocurrencyMarketMonitoring.Abstractions.Services;
using CryptocurrencyMarketMonitoring.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyMarketMonitoring.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptocurrencyOverviewController : ControllerBase
    {
        public CryptocurrencyOverviewController(ILogger<CryptocurrencyOverviewController> logger, ICryptocurrencyOverviewService cryptocurrencyOverviewService)
        {
            _logger = logger;
            _cryptocurrencyOverviewService = cryptocurrencyOverviewService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetCryptocurrencyList()
        {
            return Ok(await _cryptocurrencyOverviewService.GetCryptocurrencyListAsync());
        }

        private readonly ILogger<CryptocurrencyOverviewController> _logger;
        private ICryptocurrencyOverviewService _cryptocurrencyOverviewService;
    }
}
