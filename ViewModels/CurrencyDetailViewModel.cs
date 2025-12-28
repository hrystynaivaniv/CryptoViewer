using CryptoViewer.Models;
using System.Windows.Input;

namespace CryptoViewer.ViewModels
{
    public class CurrencyDetailViewModel : BaseViewModel
    {
        public Currency Currency { get; }
        public ICommand BackCommand { get; }

        public string Name => Currency.Name;
        public string Symbol => Currency.Symbol?.ToUpper();
        public string PriceDisplay => $"{Currency.CurrentPrice:N2} $";
        public string VolumeDisplay => $"{Currency.TotalVolume:N0} $";
        public string ChangeDisplay => $"{Currency.PriceChange24h:P2}";
        public string ChangeColor => Currency.PriceChange24h >= 0 ? "Green" : "Red";

        public CurrencyDetailViewModel(Currency currency, ICommand backCommand)
        {
            Currency = currency ?? throw new System.ArgumentNullException(nameof(currency));
            BackCommand = backCommand;

        }
    }
}