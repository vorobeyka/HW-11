namespace CurrencyConverter.TestApp.Models.Tests
{
    internal class ValidTest : ITest
    {
        public int ExpectedStatusCode { get; } = 200;
        public string RequestString { get; private set; }

        public ValidTest(string requestString)
        {
            RequestString = requestString;
        }

        public ITest NewRequest(string request)
        {
            RequestString = request;
            return this;
        }
    }
}
