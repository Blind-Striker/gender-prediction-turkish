using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GenderPrediction.Turkish;
using GenderPrediction.Turkish.Model;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;

namespace GenderPrediction.Trainer
{
    class Program
    {
        //private const string TrainingDataFile = "./Data/turkish-names-sample-data.csv";
        private const string TrainingDataFile = @"E:\machine learning\gender-prediction-turkish\src\Data\turkish-names-data.csv";
        private const string TestDataFile = "./Data/turkish-names-test.csv";
        private const string ModelPath = "./Data/Model.zip";

        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            var model = File.Exists(ModelPath) ? LoadModel(mlContext) : Train(mlContext);

            var predictionEngine = model.CreatePredictionEngine<GenderClassificationData, GenderPredictionResult>(mlContext);

            var testData = File.ReadLines(TestDataFile)
                .Select(l =>
                {
                    string[] nameGenderPair = l.Split(',');

                    return new GenderClassificationData()
                    {
                        Name = string.Join("",
                            nameGenderPair[0].Trim().Normalize(NormalizationForm.FormD).Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)).Replace("ı", "i").ToUpperInvariant(),
                        Label = nameGenderPair[1].Trim()
                    };
                })
                .Where((data => data.Label == "U"));

            foreach (var testName in testData)
            {
                var genderPrediction = predictionEngine.Predict(testName);
                string predictedGender = genderPrediction.Class == "1" ? "Erkek" : "Kadın";

                Console.WriteLine($"{testName.Name} Predicted Gender {predictedGender}");

                for (int i = 0; i < genderPrediction.Score.Length; i++)
                {
                    string scoreClassName = i == 0 ? "Erkek" : "Kadın";

                    Console.Write($"({scoreClassName})={genderPrediction.Score[i]}");
                }
                Console.WriteLine();
            }
        }

        private static ITransformer Train(MLContext mlContext)
        {
            TextLoader textReader = mlContext.Data.CreateTextReader<GenderClassificationData>(false, ',');
            IDataView trainingDataView = textReader.Read(TrainingDataFile);

            var estimatorChain = mlContext.Transforms.Text.FeaturizeText("Name", "Features")
                .Append(mlContext.Transforms.Conversion.MapValueToKey("Label"))
                .Append(mlContext.MulticlassClassification.Trainers.StochasticDualCoordinateAscent())
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
