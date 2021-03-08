namespace CurrencyConverter.TestApp.Models.Tests
{
    internal interface ITest
    {
        int ExpectedStatusCode { get; }
        string RequestString { get; }

        ITest NewRequest(string request);
    }
}
