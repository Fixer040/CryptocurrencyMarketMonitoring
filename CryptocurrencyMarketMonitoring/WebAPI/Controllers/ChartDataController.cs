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
        public async Task<IActionResult> GetCryptocurrencyList(string ticker, [FromQuery]int intervalType)
        {
            var data = await _chartDataService.GetChartDataAsync(ticker, intervalType);
            return Ok(data);
        }

        private readonly ILogger<CryptocurrencyOverviewController> _logger;
        private IChartDataService _chartDataService;
    }
}
