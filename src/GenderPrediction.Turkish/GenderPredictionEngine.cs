using System;
using System.IO;
using System.Reflection;
using System.Threading;
using GenderPrediction.Turkish.Contracts;
using GenderPrediction.Turkish.Models;
using Microsoft.ML;
using Microsoft.ML.Core.Data;

namespace GenderPrediction.Turkish
{
    public class GenderPredictionEngine : IGenderPredictionEngine
    {
        private readonly Lazy<PredictionEngineBase<GenderClassificationData, GenderPredictionResult>> _predictionEngineLazy;

        public GenderPredictionEngine()
        {
            _predictionEngineLazy = new Lazy<PredictionEngineBase<GenderClassificationData, GenderPredictionResult>>(
               () =>
               {
                   Assembly assembly = Assembly.GetExecutingAssembly();
                   var resourceName = "GenderPrediction.Turkish.TrainedModel.logistic-regression-model.zip";

                   using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                   {
                       var mlContext = new MLContext();
                       ITransformer model = mlContext.Model.Load(stream);
                       return model.CreatePredictionEngine<GenderClassificationData, GenderPredictionResult>(mlContext);
                   }
               }, LazyThreadSafetyMode.ExecutionAndPublication);
        }


        public GenderPredictionResult Predict(GenderClassificationData genderClassificationData)
        {
            return _predictionEngineLazy.Value.Predict(genderClassificationData);
        }
    }
}
