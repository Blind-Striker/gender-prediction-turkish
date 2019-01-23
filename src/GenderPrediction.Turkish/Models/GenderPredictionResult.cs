using Microsoft.ML.Data;

namespace GenderPrediction.Turkish.Models
{
    public class GenderPredictionResult
    {
        [ColumnName("PredictedLabel")]
        public string Class { get; set; }

        [ColumnName("Score")]
        public float[] Score { get; set; }
    }
}