using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Legacy;
using Reqnroll;
using Reqnroll_Playwright_API_Automation_Test.Helpers;
using Reqnroll_Playwright_API_Automation_Test.Hooks;

namespace Reqnroll_Playwright_API_Automation_Test.StepDefinitions
{
    [Binding]
    public class PurchaseApplicationAPIStepDefinitions
    {
        private readonly PlaywrightWorld _world;
        private readonly APIHelper _apiHelper;
        private IAPIResponse _response;
        private JObject _json;
        private JArray _orders;

        public PurchaseApplicationAPIStepDefinitions(PlaywrightWorld world)
        {
            _world = world;
            _apiHelper = new APIHelper(_world);
        }

        [Given("I reset the test data")]
        public async Task GivenIResetTheTestData()
        {
            _response = await _apiHelper.PostAsync("/ENSEK/reset");
            _json = await _apiHelper.ParseResponseAsJson(_response);
        }

        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int expectedStatus)
        {
            Assert.That(_response.Status, Is.EqualTo(expectedStatus),
                $"Expected status {expectedStatus} but got {_response.Status}");
        }

        [Then(@"the response message should contain ""(.*)""")]
        public async Task ThenTheResponseMessageShouldContain(string expected)
        {
            var body = await _response.TextAsync();
            StringAssert.Contains(expected, body, "Response message did not contain expected text.");
        }


        [When(@"I buy (.*) units of (.*)")]
        public async Task WhenIBuyFuel(string quantity, string energyType)
        {
            _response = await _apiHelper.PutAsync($"/ENSEK/buy/{energyType}/{quantity}");     
        }

        [When("I retrieve the list of orders")]
        public async Task WhenIRetrieveTheListOfOrders()
        {
            _response = await _apiHelper.GetAsync("/ENSEK/orders");
            _orders = await _apiHelper.ParseResponseAsJsonArray(_response);
        }

        [Then(@"the order should be listed with (.*) and (.*)")]
        public async Task ThenOrderShouldBeListed(string fuelType, int quantity)
        {
            // Validate all orders
            foreach (var order in _orders)
            {
                var fuel = order["fuel"]?.ToString();
                var qty = order["quantity"]?.Value<int?>();

                Assert.That(fuel, Is.Not.Null.And.Not.Empty, "Fuel value should not be null or empty");
                Assert.That(qty, Is.GreaterThan(0), $"Quantity should be > 0 but was {qty}");
            }

            // Find the expected order
            var match = _orders.FirstOrDefault(o =>
                string.Equals(o["fuel"]?.ToString(), fuelType, StringComparison.OrdinalIgnoreCase) &&
                o["quantity"]?.Value<int?>() == quantity);

            TestContext.WriteLine($"Matching order: {match}");
            Assert.That(match, Is.Not.Null, $"No order found with fuel='{fuelType}' and quantity={quantity}");
        }
        [Then("I count how many orders were created before today")]
        public async Task ThenICountHowManyOrdersWereCreatedBeforeToday()
        {
            Assert.That(_orders.Count, Is.GreaterThanOrEqualTo(0));
            var ordersArray = JArray.Parse(await _response.TextAsync());

            // Explicit GMT date
            var currentGmtDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("GMT")).Date;

            int count = ordersArray.Count(o =>
            {
                var timeValue = o["time"]?.ToString();
                if (!string.IsNullOrEmpty(timeValue))
                {
                    // Use flexible parse that supports "7 Feb" and "08 Feb"
                    if (DateTime.TryParse(timeValue,
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.AssumeUniversal,
                        out var created))
                    {
                        return created.Date < currentGmtDate;
                    }
                }
                return false;
            });

            Console.WriteLine($"?? Orders created before today (GMT): {count}");
        }

        [When("I access the API with invalid credentials {string} and {string}")]
        public async Task WhenIAccessTheAPIWithInvalidCredentialsAnd(string username, string password)
        
        {

            _response = await _apiHelper.AccessAPIwithinvalidCredentials(username,password);
            _json = await _apiHelper.ParseResponseAsJson(_response);
        }

    }

}
