using System;
using System.Threading.Tasks;
using CurrencyConverter.TestApp.Models.Tests;

namespace CurrencyConverter.TestApp
{
    class Program
    {
        #region valid requests
        private static readonly string _validRequest1 = "rates/usd/uah?amount=10";
        private static readonly string _validRequest2 = "rates/eur/usd?amount=200";
        private static readonly string _validRequest3 = "rates/uah/usd?amount=0";
        #endregion

        #region invalid requests
        private static readonly string _invalidRequest1 = "rates/zzz/uah?amount=";
        private static readonly string _invalidRequest2 = "rates/usd/olol";
        private static readonly string _invalidRequest3 = "rates/zzz/aaa?amount=1";
        #endregion

        static async Task Main(string[] args)
        {
            var validator = TestValidator.GetInstance();
            var validTest = new ValidTest(_validRequest1);
            var invalidTest = new InvalidTest(_invalidRequest1);

            await validator.TestRegister();

            await validator.TestCurrencyExchanger(validTest);
            await validator.TestCurrencyExchanger(validTest.NewRequest(_validRequest2));
            await validator.TestCurrencyExchanger(validTest.NewRequest(_validRequest3));

            await validator.TestCurrencyExchanger(invalidTest);
            await validator.TestCurrencyExchanger(invalidTest.NewRequest(_invalidRequest2));
            await validator.TestCurrencyExchanger(invalidTest.NewRequest(_invalidRequest3));
        }
    }
}
