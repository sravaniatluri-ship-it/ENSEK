using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using Reqnroll;
using Reqnroll_Playwright_API_Automation_Test.Hooks;


[Binding]
[Parallelizable(ParallelScope.Self)]
public class Hooks
{
    private readonly PlaywrightWorld _world;
   

    public Hooks(PlaywrightWorld world)
    {
        _world = world;

       
    }

    #region ---------- BeforeScenario ----------
    [BeforeScenario("@api")]
    public async Task BeforeApiScenario()
    {
        // Initialize Playwright if not already
        if (_world.Playwright is null)
            _world.Playwright = await Playwright.CreateAsync();

        // Initialize API context if not already
        if (_world.Api is null)
        {
            _world.Api = await _world.Playwright.APIRequest.NewContextAsync();
        }
    }
    #endregion

   

    #region ---------- AfterScenario ----------
    [AfterScenario]
    public async Task AfterScenario()
    {
        if (_world.Api is not null)
        {
            await _world.Api.DisposeAsync();
            _world.Api = null;
        }

        _world.Playwright?.Dispose();
        _world.Playwright = null;
    }
    #endregion

    
}

