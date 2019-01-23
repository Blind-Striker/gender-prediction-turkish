using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GenderPrediction.Turkish.Contracts;
using GenderPrediction.Turkish.Models;
using GenderPrediction.Turkish.Properties;
using Microsoft.ML;
using Microsoft.ML.Core.Data;

namespace GenderPrediction.Turkish
{
    public class GenderPredictionService : IGenderPredictionService
    {
        private readonly PredictionEngine<GenderClassificationData, GenderPredictionResult> _predictionEngine;

        public GenderPredictionService()
        {
            using (Stream stream = new MemoryStream(Resources.logistic_regression_model))
            {
                var mlContext = new MLContext();
                ITransformer model = mlContext.Model.Load(stream);
                _predictionEngine = model.CreatePredictionEngine<GenderClassificationData, GenderPredictionResult>(mlContext);
            }            
        }

        public GenderPredictionModel Predict(string name)
        {
            var formattedName = string.Join("",
                    name.Trim().Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                .Replace("ı", "i")
                .ToUpperInvariant();

            GenderPredictionResult genderPredictionResult = _predictionEngine.Predict(new GenderClassificationData() {Name = formattedName });

            Gender predictedGender = genderPredictionResult.Class == "1" ? Gender.Male : Gender.Female;
            var genderScores = genderPredictionResult.Score
                .Select((score, index) => new KeyValuePair<Gender, float>(index == 0 ? Gender.Male : Gender.Female, score))
                .ToDictionary(x => x.Key, x => x.Value);

            var unisexProbability = (genderPredictionResult.Score.Min() / genderPredictionResult.Score.Max()) * 100;
            return new GenderPredictionModel(name, predictedGender, genderScores, unisexProbability);
        }

        public IEnumerable<GenderPredictionModel> Predict(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                yield return Predict(name);
            }
        }
    }
}
