using CryptoViewer.Models;
using CryptoViewer.Services;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;

namespace CryptoViewer.ViewModels
{
    public class CurrencyDetailViewModel : BaseViewModel
    {
        private readonly ICoinService _coinService;
        private PlotModel _chartModel;
        private ObservableCollection<MarketInfo> _markets;
        public ObservableCollection<MarketInfo> Markets
        {
            get => _markets;
            set => SetProperty(ref _markets, value);
        }
        public Currency Currency { get; }
        public ICommand BackCommand { get; }

        public PlotModel ChartModel
        {
            get => _chartModel;
            set => SetProperty(ref _chartModel, value);
        }


        public ICommand OpenMarketCommand { get; }
        public string Name => Currency.Name;
        public string Symbol => Currency.Symbol?.ToUpper();
        public string PriceDisplay => $"{Currency.CurrentPrice:N2} $";
        public string VolumeDisplay => $"Vol: {Currency.TotalVolume:N0} $";
        public string ChangeDisplay => $"{Currency.PriceChange24h / 100:P2}"; 
        public string ChangeColor => Currency.PriceChange24h >= 0 ? "Green" : "Red";

        public CurrencyDetailViewModel(Currency currency, ICommand backCommand, ICoinService coinService)
        {
            Currency = currency;
            BackCommand = backCommand;
            _coinService = coinService;

            OpenMarketCommand = new RelayCommand<string>(url =>
            {
                if (!string.IsNullOrEmpty(url))
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
            });

            ChartModel = new PlotModel { Title = "Price History (7 Days)" };
            _ = LoadChartData();
            _ = LoadMarketData();
        }

        private async Task LoadMarketData()
        {
            var marketList = await _coinService.GetCoinMarketsAsync(Currency.Id);
            Markets = new ObservableCollection<MarketInfo>(marketList);
        }

        private async Task LoadChartData()
        {
            var history = await _coinService.GetCoinHistoryAsync(Currency.Id, 7);
            if (history == null) return;

            var lineSeries = new LineSeries
            {
                Color = OxyColors.Blue,
                StrokeThickness = 2,
                MarkerType = MarkerType.None
            };

            foreach (var point in history)
            {
                lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(DateTimeOffset.FromUnixTimeMilliseconds((long)point[0]).DateTime), point[1]));
            }

            var model = new PlotModel();
            model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "dd.MM" });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Price USD" });
            model.Series.Add(lineSeries);

            ChartModel = model;
        }
    }
}