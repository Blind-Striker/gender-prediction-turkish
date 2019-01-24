using GenderPrediction.Turkish.Contracts;

namespace GenderPrediction.Turkish
{
    public static class GenderPredictionStandalone
    {
        public static IGenderPredictionService Create()
        {
            var predictionEngine = new GenderPredictionEngine();
            var genderPredictionService = new GenderPredictionService(predictionEngine);

            return genderPredictionService;
        }
    }
}
