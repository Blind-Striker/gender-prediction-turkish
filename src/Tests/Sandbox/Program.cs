using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTableExt;
using GenderPrediction.Train;
using GenderPrediction.Turkish;
using GenderPrediction.Turkish.Contracts;
using GenderPrediction.Turkish.Models;

namespace Sandbox
{
    public class Program
    {
        public static void Main()
        {
            IGenderPredictionService genderPredictionService = new GenderPredictionService();
            var genderPredictionModels = genderPredictionService.Predict(GetRandomSample(Names.NameGender, 250).Select(pair => pair.Key).ToArray());

            var tableRows = genderPredictionModels
                .OrderByDescending(model => model.UnisexProbability)
                .Select(genderPredictionModel => new TableRow
                {
                    Name = genderPredictionModel.Name,
                    PredictedGender = GetShortenedGender(genderPredictionModel.PredictedGender),
                    TestDataGender = GetShortenedGender(Names.NameGender[genderPredictionModel.Name]),
                    ScoreMale = (genderPredictionModel.Score.First(pair => pair.Key == Gender.Male).Value * 100).ToString("00.##"),
                    ScoreFemale = (genderPredictionModel.Score.First(pair => pair.Key == Gender.Female).Value * 100).ToString("00.##"),
                    UnisexProbability = genderPredictionModel.UnisexProbability
                })
                .ToList();

            ConsoleTableBuilder
                .From(tableRows)
                .WithColumn("Name", "P.Gender", "T.Gender", "Score Male", "Score Female", "U.Probability")
                .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                .ExportAndWriteLine();
        }

        private class TableRow
        {
            public string Name { get; set; }

            public string PredictedGender { get; set; }

            public string TestDataGender { get; set; }

            public string ScoreMale { get; set; }

            public string ScoreFemale { get; set; }

            public float UnisexProbability { get; set; }
        } 

        private static string GetShortenedGender(Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return "Male";
                case Gender.Female:
                    return "Female";
                case Gender.Unisex:
                    return "Unisex";
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }

        private static string GetShortenedGender(string gender)
        {
            switch (gender)
            {
                case "E":
                    return "Male";
                case "K":
                    return "Female";
                case "U":
                    return "Unisex";
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }
        }

        private static IDictionary<TKey, TValue> GetRandomSample<TKey, TValue>(IDictionary<TKey, TValue> list, int sampleSize)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (sampleSize > list.Count)
            {
                throw new ArgumentException("sampleSize may not be greater than list count", nameof(sampleSize));
            }

            var indices = new Dictionary<int, int>();
            var rnd = new Random();

            IDictionary<TKey, TValue> newDic = new Dictionary<TKey, TValue>(); 

            for (var i = 0; i < sampleSize; i++)
            {
                var j = rnd.Next(i, list.Count);
                if (!indices.TryGetValue(j, out var index))
                {
                    index = j;
                }

                newDic.Add(list.ElementAt(index));

                if (!indices.TryGetValue(i, out index)) index = i;
                {
                    indices[j] = index;
                }
            }

            return newDic;
        }
    }
}
