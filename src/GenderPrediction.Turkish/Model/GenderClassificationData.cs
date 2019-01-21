using Microsoft.ML.Data;

namespace GenderPrediction.Turkish.Model
{
    public class GenderClassificationData
    {
        [LoadColumn(0), Column(ordinal: "0")]
        public string Name { get; set; }

        [LoadColumn(1), Column(ordinal: "1", name: "Label")]
        public string Label { get; set; }
    }
}
