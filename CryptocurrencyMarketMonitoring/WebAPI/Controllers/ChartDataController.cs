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
    public class ChartDataController : ControllerBase
    {
        public ChartDataController(ILogger<CryptocurrencyOverviewController> logger, IChartDataService chartDataService)
        {
            _logger = logger;
            _chartDataService = chartDataService;
        }

        [HttpGet("{ticker}")]
        public async Task<IActionResult> GetCryptocurrencyList(string ticker)
        {
            var data = await _chartDataService.GetChartDataAsync(ticker);
            return Ok(new { Items = data, Count = data.Count() });
        }

        private readonly ILogger<CryptocurrencyOverviewController> _logger;
        private IChartDataService _chartDataService;
    }
}
