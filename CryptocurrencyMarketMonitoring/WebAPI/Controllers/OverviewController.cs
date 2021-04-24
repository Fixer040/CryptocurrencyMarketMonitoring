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
    public class OverviewController : ControllerBase
    {
        public OverviewController(ILogger<OverviewController> logger, IOverviewService cryptocurrencyOverviewService)
        {
            _logger = logger;
            _cryptocurrencyOverviewService = cryptocurrencyOverviewService;
        }


        [HttpGet("currencies")]
        public IActionResult GetCryptocurrencyList()
        {
            return Ok(_cryptocurrencyOverviewService.GetSupportedCurrencies());
        }

        [HttpGet("overview/{currency}")]
        public IActionResult GetCryptocurrencyList(string currency = "usd")
        {
            return Ok(_cryptocurrencyOverviewService.GetOverview(currency));
        }

        private readonly ILogger<OverviewController> _logger;
        private IOverviewService _cryptocurrencyOverviewService;
    }
}
