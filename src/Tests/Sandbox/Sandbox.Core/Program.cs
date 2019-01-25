using System.Linq;
using ConsoleTableExt;
using GenderPrediction.Turkish;
using GenderPrediction.Turkish.Contracts;
using GenderPrediction.Turkish.Models;
using GenderPrediction.Turkish.TestData;

namespace Sandbox.Core
{
    public class Program
    {
        public static void Main()
        {
            IGenderPredictionService genderPredictionService = GenderPredictionStandalone.Create();
            var genderPredictionModels = genderPredictionService.Predict(Utils.GetRandomSample(Names.NameGender, 250).Select(pair => pair.Key).ToArray());

            var tableRows = genderPredictionModels
                .OrderByDescending(model => model.UnisexProbability)
                .Select(genderPredictionModel => new TableRow
                {
                    Name = genderPredictionModel.Name,
                    PredictedGender = Utils.GetShortenedGender(genderPredictionModel.PredictedGender),
                    TestDataGender = Utils.GetShortenedGender(Names.NameGender[genderPredictionModel.Name]),
                    ScoreMale = (genderPredictionModel.Score.First(pair => pair.Key == Gender.Male).Value * 100).ToString("00.##"),
                    ScoreFemale = (genderPredictionModel.Score.First(pair => pair.Key == Gender.Female).Value * 100).ToString("00.##"),
                    UnisexProbability = genderPredictionModel.UnisexProbability
                })
                .ToList();

            ConsoleTableBuilder
                .From(tableRows)
                .WithColumn("Name", "P.Gender", "T.Gender", "Score Male", "Score Female", "U.Probability")
                .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                .ExportAndWriteLine();
        }
    }
}
