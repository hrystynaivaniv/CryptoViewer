using CryptoViewer.Models;
using System.Net.Http;
using System.Text.Json;

namespace CryptoViewer.Services
{
    public class CoinGeckoService : ICoinService
    {
        private readonly HttpClient _httpClient;

        public CoinGeckoService()
        {
            _httpClient = new HttpClient
            {
                DefaultRequestHeaders = { { "User-Agent", "CryptoViewerApp" } }
            };
        }

        public async Task<List<Currency>> GetTopCurrenciesAsync()
        {
            try
            {
                var url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=10&page=1&price_change_percentage=24h";

                var response = await _httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<List<Currency>>(json, options) ?? new List<Currency>();
            }
            catch (Exception ex)
            {
                throw new Exception("Data can not be loaded from CoinGecko", ex);
            }
        }

        public async Task<List<double[]>> GetCoinHistoryAsync(string id, int days)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/{id}/market_chart?vs_currency=usd&days={days}";
            var response = await _httpClient.GetStringAsync(url);

            using var doc = JsonDocument.Parse(response);
            var prices = doc.RootElement.GetProperty("prices").Deserialize<List<double[]>>();
            return prices;
        }
    }
}