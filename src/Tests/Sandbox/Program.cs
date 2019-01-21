using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GenderPrediction.Train;
using GenderPrediction.Turkish;
using GenderPrediction.Turkish.Model;
using Microsoft.ML;
using Microsoft.ML.Core.Data;

namespace Sandbox
{
    public class Program
    {
        private const string TrainingDataFile = "./turkish-names-sample-data.csv";
        private const string TestDataFile = "./turkish-names-test.csv";
        private const string ModelPath = "./Model.zip";

        public static async Task Main(string[] args)
        {
            var mlContext = new MLContext();

            //TextLoader textReader = mlContext.Data.CreateTextReader<GenderClassificationData>(false, ',');
            //IDataView trainingDataView = textReader.Read(TrainingDataFile);

            //var estimatorChain = mlContext.Transforms.Text.FeaturizeText("Name", "Features")
            //    .Append(mlContext.Transforms.Conversion.MapValueToKey("Label"))
            //    .Append(mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent())
            //    //.Append(mlContext.Transforms.Conversion.MapKeyToValue(("PredictedLabel", "Data")));
            //    .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            //var model = estimatorChain.Fit(trainingDataView);

            //var predictionEngine = model.CreatePredictionEngine<GenderClassificationData, GenderPredictionResult>(mlContext);

            //using (var fs = File.Create(ModelPath))
            //{
            //    mlContext.Model.Save(model, fs);
            //}

            ITransformer model;
            using (var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                model = mlContext.Model.Load(stream);
            }

            var predictionEngine = model.CreatePredictionEngine<GenderClassificationData, GenderPredictionResult>(mlContext);

            var classificationDatas = Names.FirstNames.Take(100).Select(s => new GenderClassificationData() { Name = s.ToUpperInvariant() });

            foreach (var genderClassificationData in classificationDatas)
            {
                var genderPrediction = predictionEngine.Predict(genderClassificationData);
                string predictedGender = genderPrediction.Class == "1" ? "Erkek" : "Kadın";

                Console.WriteLine($"{genderClassificationData.Name} Predicted Gender {predictedGender}");

                for (int i = 0; i < genderPrediction.Score.Length; i++)
                {
                    string scoreClassName = i == 0 ? "Erkek" : "Kadın";

                    Console.Write($"({scoreClassName})={genderPrediction.Score[i]}");
                }
                Console.WriteLine();
            }
        }

        internal static async Task TrainAsync(string trainingDataFile, string modelPath)
        {

        }
    }
}
