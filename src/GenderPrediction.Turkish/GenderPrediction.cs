using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using GenderPrediction.Turkish.Contracts;
using GenderPrediction.Turkish.Model;
using Microsoft.ML;
using Microsoft.ML.Core.Data;

namespace GenderPrediction.Turkish
{
    public class GenderPrediction : IGenderPrediction
    {
        private readonly MLContext _mlContext;
        private readonly PredictionEngine<GenderClassificationData, GenderPredictionResult> _predictionEngine;

        public GenderPrediction()
        {
            _mlContext = new MLContext();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "GenderPrediction.Turkish.Data.model.zip";

            ITransformer model;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    model = _mlContext.Model.Load(stream);
                }
            }

            _predictionEngine = model.CreatePredictionEngine<GenderClassificationData, GenderPredictionResult>(_mlContext);
        }

        public GenderPredictionModel Predict(string name)
        {
            string formattedName = string.Join("",
                    name.Trim().Normalize(NormalizationForm.FormD)
                        .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)).Replace("ı", "i")
                .ToUpperInvariant();

            var genderPredictionResult = _predictionEngine.Predict(new GenderClassificationData() {Name = formattedName });
        }
    }
}
