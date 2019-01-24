# Turkish Names Gender Prediction

GenderPrediction.Turkish is a library that provides prediction of gender from Turkish names built on [ML.NET](https://github.com/dotnet/machinelearning)
Both training application and library included in source code.

[![NuGet](https://img.shields.io/nuget/v/GenderPrediction.Turkish.svg)](https://www.nuget.org/packages/GenderPrediction.Turkish)

## Supported Platforms

* .NET 4.6.1 (Desktop / Server)
* [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
* .NET Core 2.0, 2.1, 2.2

## Continuous integration

<!-- | Build server                | Platform      | Build status                                                                                                                                                        | Integration tests                                                                                                                                                   |
|-----------------------------|---------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Azure Pipelines             | Windows       | [![Build status](https://denizirgindev.visualstudio.com/simplecast-api-client-dotnet/_apis/build/status/Build%20Simplecast%20Windows)](https://denizirgindev.visualstudio.com/simplecast-api-client-dotnet/_build/latest?definitionId=6) | |
| Azure Pipelines             | Ubuntu        | [![Build status](https://denizirgindev.visualstudio.com/simplecast-api-client-dotnet/_apis/build/status/Build%20Simplecast%20Ubuntu16)](https://denizirgindev.visualstudio.com/simplecast-api-client-dotnet/_build/latest?definitionId=4) | |
| Azure Pipelines             | MacOS         | [![Build status](https://denizirgindev.visualstudio.com/simplecast-api-client-dotnet/_apis/build/status/Build%20Simplecast%20macOS)](https://denizirgindev.visualstudio.com/simplecast-api-client-dotnet/_build/latest?definitionId=5) | |
| AppVeyor                    | Windows       | [![Build status](https://ci.appveyor.com/api/projects/status/pe8ln6mov67p3ayb?svg=true)](https://ci.appveyor.com/project/Blind-Striker/simplecast-api-client-dotnet)           | |
| Travis                      | Linux / MacOS | [![Build Status](https://travis-ci.com/Blind-Striker/simplecast-api-client-dotnet.svg?token=Vj8PGFoMvzHtyPjfWb4P&branch=master)](https://travis-ci.com/Blind-Striker/simplecast-api-client-dotnet)  | | -->

## Table of Contents

1. [Installation](#installation)
2. [Usage](#usage)
    - [Standalone Initialization](#standalone-initialization)
    - [Microsoft.Extensions.DependencyInjection Initialization](#microsoftextensionsdependencyinjection-initialization)
    - [Call Endpoints](#call-endpoints)
    - [Synchronous Wrapper](#synchronous-wrapper)
3. [License](#license)

## Installation

[![NuGet](https://img.shields.io/nuget/v/GenderPrediction.Turkish.svg)](https://www.nuget.org/packages/GenderPrediction.Turkish) 

Following commands can be used to install GenderPrediction.Turkish, run the following command in the Package Manager Console

```
Install-Package GenderPrediction.Turkish
```

Or use `dotnet cli`

```
dotnet GenderPrediction.Turkish
```
## Usage

GenderPrediction.Turkish can be used with any DI library, or it can be used standalone.

### Standalone Initialization

If you do not want to use any DI framework, you have to instantiate `GenderPredictionStandalone` as follows.

```csharp
IGenderPredictionService genderPredictionService = GenderPredictionStandalone.Create();
```

### Microsoft.Extensions.DependencyInjection Initialization

First, you need to install `Microsoft.Extensions.DependencyInjection` NuGet package as follows

```
dotnet add package Microsoft.Extensions.DependencyInjection
```

Register necessary dependencies to `ServiceCollection` as follows

```csharp
var services = new ServiceCollection();
services.AddSingleton<IGenderPredictionEngine, GenderPredictionEngine>();
services.AddTransient<IGenderPredictionService, GenderPredictionService>();

ServiceProvider buildServiceProvider = services.BuildServiceProvider();

var genderPredictionService = buildServiceProvider.GetRequiredService<IGenderPredictionService>();
```

After library is initialized, it is very easy to use

```csharp
GenderPredictionModel model = genderPredictionService.Predict("Deniz");

string name = genderPredictionModel.Name;
Gender predictedGender = genderPredictionModel.PredictedGender;
float maleProbability = genderPredictionModel.Score[Gender.Male];
float femaleProbability = genderPredictionModel.Score[Gender.Female];
float unisexProbability = genderPredictionModel.UnisexProbability
```

Or

```csharp
string[] names = new[] {"Dilek", "Hasan", "Mehmet", "İbrahim"};
IEnumerable<GenderPredictionModel> model = genderPredictionService.Predict(names);
```

## License
Licensed under MIT, see [LICENSE](LICENSE) for the full text.
