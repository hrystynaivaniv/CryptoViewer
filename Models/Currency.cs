using System.Text.Json.Serialization;

namespace CryptoViewer.Models
{
    public class Currency
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("current_price")]
        public decimal? CurrentPrice { get; set; }

        [JsonPropertyName("price_change_percentage_24h_in_currency")]
        public decimal? PriceChange24h { get; set; }

        [JsonPropertyName("total_volume")]
        public decimal? TotalVolume { get; set; }
    }
}
