using CryptoViewer.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CryptoViewer.ViewModels
{
    public class ConverterViewModel : BaseViewModel
    {
        private Currency? _fromCurrency;
        private Currency? _toCurrency;
        private double _amount = 1;
        private double _result;

        public ObservableCollection<Currency> Currencies { get; }
        public ICommand BackCommand { get; }

        public Currency? FromCurrency
        {
            get => _fromCurrency;
            set { if (SetProperty(ref _fromCurrency, value)) Calculate(); }
        }

        public Currency? ToCurrency
        {
            get => _toCurrency;
            set { if (SetProperty(ref _toCurrency, value)) Calculate(); }
        }

        public double Amount
        {
            get => _amount;
            set { if (SetProperty(ref _amount, value)) Calculate(); }
        }

        public double Result
        {
            get => _result;
            private set => SetProperty(ref _result, value);
        }

        public ConverterViewModel(IEnumerable<Currency> currencies, ICommand backCommand)
        {
            Currencies = new ObservableCollection<Currency>(currencies);
            BackCommand = backCommand;
            
            FromCurrency = Currencies.FirstOrDefault();
            ToCurrency = Currencies.Skip(1).FirstOrDefault();
        }

        private void Calculate()
        {
            if (FromCurrency == null || ToCurrency == null || ToCurrency.CurrentPrice == 0)
            {
                Result = 0;
                return;
            }

            Result = (Amount * (double)(FromCurrency.CurrentPrice ?? 0)) / (double)(ToCurrency.CurrentPrice ?? 1);
        }
    }
}