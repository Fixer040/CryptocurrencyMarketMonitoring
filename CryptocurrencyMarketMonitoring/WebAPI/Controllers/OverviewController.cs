using CryptocurrencyMarketMonitoring.Abstractions;
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
        public IActionResult GetSupportedCurrencies()
        {
            return Ok(_cryptocurrencyOverviewService.GetSupportedCurrencies());
        }

        [HttpGet("overview-all/{currency}")]
        public IActionResult GetCryptocurrencyOverviewAll(string currency = "usd")
        {
            return Ok(_cryptocurrencyOverviewService.GetOverviewAll(currency));
        }

        [HttpGet("overview/{currency}/{vscurrency}")]
        public IActionResult GetCryptocurrencyOverview(string currency, string vsCurrency = "usd")
        {
            return Ok(_cryptocurrencyOverviewService.GetOverview(currency, vsCurrency));
        }

        private readonly ILogger<OverviewController> _logger;
        private IOverviewService _cryptocurrencyOverviewService;
    }
}
