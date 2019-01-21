using Microsoft.ML.Data;

namespace GenderPrediction.Turkish.Model
{
    public class GenderPredictionResult
    {
        [ColumnName("PredictedLabel")]
        public string Class;

        [ColumnName("Score")]
        public float[] Score;
    }
}