using System.IO;
using GenderPrediction.Turkish.Models;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;

namespace GenderPrediction.Trainer
{
    internal static class Program
    {
        private const string TrainingDataFile = "./Data/turkish-names-sample-data.csv";       
        private const string TestDataFile = "./Data/turkish-names-test.csv";
        private const string ModelPath = "./Data/Model.zip";

        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            ITransformer model = File.Exists(ModelPath) ? LoadModel(mlContext) : Train(mlContext);

            model.CreatePredictionEngine<GenderClassificationData, GenderPredictionResult>(mlContext);
        }

        private static ITransformer Train(MLContext mlContext)
        {
            TextLoader textReader = mlContext.Data.CreateTextReader<GenderClassificationData>(false, ',');
            IDataView trainingDataView = textReader.Read(TrainingDataFile);

            var estimatorChain = mlContext.Transforms.Text.FeaturizeText("Name", "Features")
                .Append(mlContext.Transforms.Conversion.MapValueToKey("Label"))
                .Append(mlContext.MulticlassClassification.Trainers.LogisticRegression())
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var model = estimatorChain.Fit(trainingDataView);

            using (var fs = File.Create(ModelPath))
            {
                mlContext.Model.Save(model, fs);
            }

            return model;
        }

        private static ITransformer LoadModel(MLContext mlContext)
        {
            ITransformer model;
            using (var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                model = mlContext.Model.Load(stream);
            }

            return model;
        }
    }
}
