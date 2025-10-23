using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using Reqnroll_Playwright_API_Automation_Test.Hooks;

namespace Reqnroll_Playwright_API_Automation_Test.Helpers
{
    public class APIHelper
    {
        private readonly PlaywrightWorld _world;
        public readonly ConfigLoader _config;

        public APIHelper(PlaywrightWorld world)
        {
            _world = world;
            // Get environment from system environment variable or default to "qa"
            var env = Environment.GetEnvironmentVariable("ENVIRONMENT")
                      ?? GetEnvironmentFromCommandLine()
                      ?? "qa";

            _config = new ConfigLoader(env);
            Console.WriteLine($"🌍 Running tests in '{env}' environment");
        }

        /// <summary>
        /// Sends a POST request to the specified endpoint.
        /// </summary>
        public async Task<IAPIResponse> PostAsync(string endpoint)
        {
            var options = await GetApiRequestOptions();
            var response = await _world.Api!.PostAsync(_config.ApiBaseUrl + endpoint, options);
            await LogResponse(response);
            return response;
        }

        /// <summary>
        /// Sends a PUT request to the specified endpoint.
        /// </summary>
        public async Task<IAPIResponse> PutAsync(string endpoint)
        {
            var options = await GetApiRequestOptions();
            var response = await _world.Api.PutAsync(_config.ApiBaseUrl + endpoint, options);
            await LogResponse(response);
            return response;
        }

        /// <summary>
        /// Sends a GET request to the specified endpoint.
        /// </summary>
        public async Task<IAPIResponse> GetAsync(string endpoint)
        {
            var options = await GetApiRequestOptions();
            var response = await _world.Api.GetAsync(_config.ApiBaseUrl + endpoint, options);
            await LogResponse(response);
            return response;
        }

        /// <summary>
        /// Parses an API response as JObject.
        /// </summary>
        public async Task<JObject> ParseResponseAsJson(IAPIResponse response)
        {          
            var body = await response.TextAsync();
            TestContext.WriteLine($"API Response: {body}");
            return JObject.Parse(body);
        }

        /// <summary>
        /// Parses an API response as JArray.
        /// </summary>
        public async Task<JArray> ParseResponseAsJsonArray(IAPIResponse response)
        {
            var body = await response.TextAsync();
            TestContext.WriteLine($"API Response: {body}");
            return JArray.Parse(body);
        }

        /// <summary>
        /// Logs the response text to TestContext.
        /// </summary>
        private async Task LogResponse(IAPIResponse response)
        {
            var text = await response.TextAsync();
            TestContext.WriteLine($"Status: {response.Status}, OK: {response.Ok}");
            TestContext.WriteLine($"Response Body: {text}");
        }
        //  Helper function to build request options
        public async Task<APIRequestContextOptions> GetApiRequestOptions()
        {
            var accessToken = await LoginAndGetTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
                throw new Exception("API login failed. Cannot continue scenario.");
            return new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                     { "Accept", "application/json" },
                     { "Authorization", $"Bearer {accessToken}" }
                }
            };
        }
       
       

        #region ---------- Helpers ----------
        private string? GetEnvironmentFromCommandLine()
        {
            var arg = Environment.GetCommandLineArgs()
                                 .FirstOrDefault(a => a.StartsWith("/p:ENVIRONMENT=", StringComparison.OrdinalIgnoreCase));

            if (arg is null) return null;

            var parts = arg.Split('=');
            if (parts.Length == 2) return parts[1].Trim();

            return null;
        }
        #endregion
        #region ---------- Login Helper ----------
        private async Task<string?> LoginAndGetTokenAsync()
        {
            var requestContext = await _world.Playwright.APIRequest.NewContextAsync();
            var response = await requestContext.PostAsync($"{_config.ApiBaseUrl}/ENSEK/login", new()
            {
                Headers = new Dictionary<string, string>
            {
                { "Accept", "application/json" },
                { "Content-Type", "application/json" }
            },
                DataObject = new
                {
                    username = _config.Username,
                    password = _config.Password
                }
            });

            var responseText = await response.TextAsync();

            if (!response.Ok)
            {
                Console.WriteLine($"❌ Login failed! Status: {response.Status}");
                Console.WriteLine(responseText);
                return null;
            }

            var json = JObject.Parse(responseText);
            var accessToken = json["access_token"]?.ToString();

            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("❌ Access token missing in login response.");
                return null;
            }

            Console.WriteLine($"✅ Access Token retrieved successfully");
            return accessToken;
        }
        #endregion
        public async Task<IAPIResponse> AccessAPIwithinvalidCredentials(string strUsername, string strPassword)
        {
            var requestContext = await _world.Playwright.APIRequest.NewContextAsync();
            var response = await requestContext.PostAsync($"{_config.ApiBaseUrl}/ENSEK/login", new()
            {
                Headers = new Dictionary<string, string>
            {
                { "Accept", "application/json" },
                { "Content-Type", "application/json" }
            },
                DataObject = new
                {
                    username = strUsername,
                    password = strPassword
                }
            });
    
            return response;
        }
    }
}
