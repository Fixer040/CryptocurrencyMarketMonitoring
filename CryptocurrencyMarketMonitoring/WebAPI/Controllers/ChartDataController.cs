﻿using CryptocurrencyMarketMonitoring.Abstractions.Services;
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
        public ChartDataController(ILogger<OverviewController> logger, IChartDataService chartDataService)
        {
            _logger = logger;
            _chartDataService = chartDataService;
        }

        [HttpGet("{currency}/{vsCurrency}/{intervalType}")]
        public async Task<IActionResult> GetCryptocurrencyList(string currency, string vsCurrency, IntervalType intervalType)
        {
            var data = await _chartDataService.GetChartDataAsync(currency, vsCurrency, intervalType);
            return Ok(data);
        }

        private readonly ILogger<OverviewController> _logger;
        private IChartDataService _chartDataService;
    }
}
