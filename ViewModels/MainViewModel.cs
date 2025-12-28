using CryptoViewer.Models;
using CryptoViewer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CryptoViewer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICoinService _coinService;
        private List<Currency> _allCurrencies = new List<Currency>();
        private string _searchText = string.Empty;
        private object _currentViewModel;

        public ObservableCollection<Currency> Currencies { get; } = new ObservableCollection<Currency>();

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    ApplyFilter();
            }
        }

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand OpenCurrencyDetailCommand { get; }
        public ICommand GoBackCommand { get; }

        public MainViewModel(ICoinService coinService)
        {
            _coinService = coinService;

            OpenCurrencyDetailCommand = new RelayCommand<Currency>(OpenCurrencyDetail);
            GoBackCommand = new RelayCommand<object>(_ => ShowMainPage());

            CurrentViewModel = this;
            _ = LoadCurrenciesAsync();
        }

        public void ShowMainPage()
        {
            CurrentViewModel = this;
        }

        private async Task LoadCurrenciesAsync()
        {
            var data = await _coinService.GetTopCurrenciesAsync();
            _allCurrencies = data ?? new List<Currency>();
            RefreshList(_allCurrencies);
        }

        private void ApplyFilter()
        {
            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allCurrencies
                : _allCurrencies.Where(c =>
                    c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    c.Symbol.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            RefreshList(filtered);
        }

        private void RefreshList(IEnumerable<Currency> list)
        {
            Currencies.Clear();
            foreach (var item in list)
            {
                Currencies.Add(item);
            }
        }

        private void OpenCurrencyDetail(Currency currency)
        {
            if (currency != null)
            {
                CurrentViewModel = new CurrencyDetailViewModel(currency, GoBackCommand, _coinService);
            }
        }
    }
}