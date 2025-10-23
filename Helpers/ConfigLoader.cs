using Newtonsoft.Json.Linq;

namespace Reqnroll_Playwright_API_Automation_Test.Helpers
{
    public class ConfigLoader
    {
        public string ApiBaseUrl { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public ConfigLoader(string environment)
        {
            var projectDir = Directory.GetParent(AppContext.BaseDirectory).Parent?.Parent?.Parent?.FullName;
            var fileName = Path.Combine(projectDir ?? ".", "TestData", $"config.{environment}.json");

            if (!File.Exists(fileName))
                throw new FileNotFoundException($"Configuration file '{fileName}' not found.");

            var json = File.ReadAllText(fileName);
            var jObject = JObject.Parse(json);

            ApiBaseUrl = jObject["ApiBaseUrl"]?.ToString() ?? throw new Exception("ApiBaseUrl missing in config");
            Username = jObject["Username"]?.ToString() ?? throw new Exception("Username missing in config");
            Password = jObject["Password"]?.ToString() ?? throw new Exception("Password missing in config");
        }
    }

}
