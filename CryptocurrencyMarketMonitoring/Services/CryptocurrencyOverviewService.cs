using CryptocurrencyMarketMonitoring.Abstractions.Services;
using CryptocurrencyMarketMonitoring.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Binance.Net;

namespace CryptocurrencyMarketMonitoring.Services
{
    public class CryptocurrencyOverviewService : ICryptocurrencyOverviewService
    {

        public async Task<IEnumerable<Cryptocurrency>> GetCryptocurrencyListAsync()
        {
            var currencies = new List<Cryptocurrency>();

            BinanceClient.SetDefaultOptions(new Binance.Net.Objects.Spot.BinanceClientOptions()
            {
                ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("sDKjPTMjAtgW6jTD9cB4IiJG3B4qyprUEsBxL2HtErGNxoFRFzn51yOAxCCcGwQG", "PAUmjPzTSkx1R57ycXgI1vLcSrVpJQYzgjrL7Koch1TvORXJaO0zEwgmDGB8ovmQ")
            });


            var binanceClient = new BinanceClient();


            var names = new List<string>() { "Bitcoin", "Ethereum", "Cardano", "XRP", "Basic Attention Token", "Polkadot", "Chainlink", "Binance Coin", "Dogecoin" };
            var tickers = new List<string>() { "BTC", "ETH", "ADA", "XRP", "BAT", "DOT", "LINK", "BNB", "DOGE" };
            for (int i = 0; i < 9; i++)
            {
                var currency = new Cryptocurrency()
                {
                    CryptocurrencyId = i + 1,
                    Name = names[i],
                    Ticker = tickers[i],
                    LastDayPercentageMovement = new Random().NextDouble(),
                    LastWeekPercentageMovement = new Random().NextDouble(),
                    MarketCapUSD = new Random().Next(1000000, int.MaxValue),
                    VolumeUSD = new Random().Next(100000, int.MaxValue),
                    CirculatingSupply = new Random().Next(10000, int.MaxValue),
                };

                currency.PriceUSD = (await binanceClient.Spot.Market.GetCurrentAvgPriceAsync($"{currency.Ticker}USDT")).Data.Price;


                currencies.Add(currency);
            }

            return currencies;
        }

    }
}
