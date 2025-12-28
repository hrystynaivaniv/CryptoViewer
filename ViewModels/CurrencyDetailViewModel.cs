using CryptoViewer.Models;
using CryptoViewer.Services;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CryptoViewer.ViewModels
{
    public class CurrencyDetailViewModel : BaseViewModel
    {
        private readonly ICoinService _coinService;
        private PlotModel _chartModel;

        public Currency Currency { get; }
        public ICommand BackCommand { get; }

        public PlotModel ChartModel
        {
            get => _chartModel;
            set => SetProperty(ref _chartModel, value);
        }

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

            ChartModel = new PlotModel { Title = "Price History (7 Days)" };
            _ = LoadChartData();
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