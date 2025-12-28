using System.Windows;
using CryptoViewer.ViewModels;
using CryptoViewer.Services; 

namespace CryptoViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ICoinService coinService = new CoinGeckoService();

            this.DataContext = new MainViewModel(coinService);
        }
    }
}