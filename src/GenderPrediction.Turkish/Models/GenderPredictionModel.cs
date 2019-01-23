using System.Collections.Generic;
using System.Collections.Immutable;

namespace GenderPrediction.Turkish.Models
{
    public class GenderPredictionModel
    {
        public GenderPredictionModel(string name, Gender predictedGender, IDictionary<Gender, float> score, float unisexProbability)
        {
            Name = name;
            PredictedGender = predictedGender;
            Score = score.ToImmutableDictionary();
            UnisexProbability = unisexProbability;
        }

        public string Name { get; }

        public Gender PredictedGender { get; }

        public IImmutableDictionary<Gender, float> Score { get; }

        public float UnisexProbability { get; }
    }
}
