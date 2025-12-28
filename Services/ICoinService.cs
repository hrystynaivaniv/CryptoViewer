using CryptoViewer.Models;

namespace CryptoViewer.Services
{
    public interface ICoinService
    {
        public Task<List<Currency>> GetTopCurrenciesAsync();
    }
}
