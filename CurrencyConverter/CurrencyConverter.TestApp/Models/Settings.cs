using System.Text.Json.Serialization;

namespace CurrencyConverter.TestApp.Models
{
    internal class Settings
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }
    }
}