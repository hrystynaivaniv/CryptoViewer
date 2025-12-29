using CryptoViewer.Models;

namespace CryptoViewer.Services
{
    public interface ICoinService
    {
        public Task<List<Currency>> GetTopCurrenciesAsync();

        public Task<List<double[]>> GetCoinHistoryAsync(string id, int days);
        public Task<List<MarketInfo>> GetCoinMarketsAsync(string id);
    }
}
