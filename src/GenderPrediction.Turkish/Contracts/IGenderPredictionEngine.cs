using GenderPrediction.Turkish.Models;

namespace GenderPrediction.Turkish.Contracts
{
    public interface IGenderPredictionEngine
    {
        GenderPredictionResult Predict(GenderClassificationData genderClassificationData);
    }
}