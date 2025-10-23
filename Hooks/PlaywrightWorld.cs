using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reqnroll_Playwright_API_Automation_Test.Hooks
{
    public class PlaywrightWorld
    {
        public IPlaywright? Playwright { get; set; }
        public IAPIRequestContext? Api { get; set; }
        public IBrowser? Browser { get; set; }
        public IPage? Page { get; set; }
    }
}
