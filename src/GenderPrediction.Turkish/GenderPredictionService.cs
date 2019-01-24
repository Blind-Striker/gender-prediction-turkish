using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using GenderPrediction.Turkish.Contracts;
using GenderPrediction.Turkish.Models;

namespace GenderPrediction.Turkish
{
    public class GenderPredictionService : IGenderPredictionService
    {
        private readonly IGenderPredictionEngine _predictionEngine;

        public GenderPredictionService(IGenderPredictionEngine predictionEngine)
        {
            _predictionEngine = predictionEngine;
        }

        public GenderPredictionModel Predict(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var formattedName = string.Join(string.Empty,
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
            if (names == null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            foreach (var name in names)
            {
                yield return Predict(name);
            }
        }
    }
}
