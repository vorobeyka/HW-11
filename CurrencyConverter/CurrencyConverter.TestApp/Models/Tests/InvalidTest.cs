namespace CurrencyConverter.TestApp.Models.Tests
{
    internal class InvalidTest : ITest
    {
        public int ExpectedStatusCode { get; } = 400;
        public string RequestString { get; private set; }

        public InvalidTest(string requestString)
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
