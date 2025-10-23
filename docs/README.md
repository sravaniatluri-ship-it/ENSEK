ENSEK API Test Automation Solution Documentation
Overview
ENSEK API Test Automation is a C# automated testing solution built using Reqnroll, Playwright, and NUnit.
It is designed to perform BDD-style API automation in a scalable, maintainable structure.

Project Structure
Folder			--- File	Description
Features		---	Contains .feature files written in Gherkin syntax describing test scenarios.
StepDefinitions ---	Contains C# step definitions corresponding to feature steps.
Hooks			---	Setup and teardown logic for API contexts.
Helpers         ---	Contains helper methods for configuration, API handling, and assertions.
Config.qa.json	--- Holds environment URLs, credentials, and configuration data.
Reqnroll_Playwright_API_Automation_Test.csproj	Project configuration and dependency management file.

Tools and Technologies
•	.NET 6 or higher: Framework runtime for automation project
•	Playwright for .NET: Automation framework for browser and API testing
•	NUnit: Test execution frameworks
•	Reqnroll: Lightweight alternative runner to SpecFlow
•	System.Text.Json: JSON parsing for API validation
•	FluentAssertions: For expressive, fluent assertions

🚀 Getting Started
1. Install .NET 8 SDK and Visual Studio 2022+.
2. Run `dotnet restore` to install dependencies.
4. Build with `dotnet build`.
5. Execute tests using dotnet test command:
   dotnet test --filter "Category=api" /p:ENVIRONMENT=qa --logger "trx;LogFileName=ensek-api-tests.trx" --- to execute only API

📦 Dependencies

    <PackageReference Include="coverlet.collector" Version="6.0.0" />
    <PackageReference Include="FluentAssertions" Version="8.7.1" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="Microsoft.Playwright" Version="1.55.0" />
    <PackageReference Include="NUnit" Version="4.4.0" />
    <PackageReference Include="NUnit.Analyzers" Version="3.9.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="5.1.0" />
    <PackageReference Include="Reqnroll" Version="3.0.2" />
    <PackageReference Include="Reqnroll.NUnit" Version="3.0.2" />
    <PackageReference Include="Reqnroll.Tools.MsBuild.Generation" Version="3.0.2" />