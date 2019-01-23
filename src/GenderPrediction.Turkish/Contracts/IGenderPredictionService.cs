using System.Collections.Generic;
using GenderPrediction.Turkish.Models;

namespace GenderPrediction.Turkish.Contracts
{
    public interface IGenderPredictionService
    {
        GenderPredictionModel Predict(string name);

        IEnumerable<GenderPredictionModel> Predict(IEnumerable<string> names);
    }
}