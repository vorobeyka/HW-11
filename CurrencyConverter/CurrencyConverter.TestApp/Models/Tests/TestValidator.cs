using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrencyConverter.TestApp.Models.Tests
{
    internal class TestValidator
    {
        private readonly string _settingsPath = "settings.json";
        private readonly HttpClient _client = new HttpClient();
        private readonly string _registerRequest = "auth/register";
        private static TestValidator _instance;

        private TestValidator()
        {
            var json = File.ReadAllText(_settingsPath);
            var settings = JsonSerializer.Deserialize<Settings>(json);
            _client.BaseAddress = new Uri(settings.Address);
        }

        public static TestValidator GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TestValidator();
            }
            return _instance;
        }

        public async Task TestRegister()
        {
            Console.WriteLine("Register test:");
            var json = JsonSerializer.Serialize(new User("login", "password"));
            var response = await _client.PostAsync(_registerRequest,
                           new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            WriteResult(_registerRequest, (int)response.StatusCode, 401, content);
        }

        public async Task TestCurrencyExchanger(ITest test)
        {
            Console.WriteLine("Currency exchanger test:");
            var response = await _client.GetAsync(test.RequestString);
            var content = await response.Content.ReadAsStringAsync();

            WriteResult(test.RequestString, (int)response.StatusCode, test.ExpectedStatusCode, content);
        }

        private void WriteResult(string request, int statusCode, int expectedStatusCode, string content)
        {
            Console.WriteLine($"Request: {request}");
            Console.WriteLine($"Received status code: {statusCode}");
            Console.WriteLine($"Expected status code: {expectedStatusCode}");
            if (statusCode == expectedStatusCode)
            {
                Console.WriteLine("Test SUCCESS!");
            }
            else
            {
                Console.WriteLine("Test FAILED!");
            }
            Console.WriteLine($"Response content:\n{content}");
            Console.WriteLine(new string('_', 25));
        }
    }
}
