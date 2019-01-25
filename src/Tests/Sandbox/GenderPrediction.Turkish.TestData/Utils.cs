using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenderPrediction.Turkish.Models;

namespace GenderPrediction.Turkish.TestData
{
    public static class Utils
    {
        public static string GetShortenedGender(Gender gender)
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

        public static string GetShortenedGender(string gender)
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

        public static IDictionary<TKey, TValue> GetRandomSample<TKey, TValue>(IDictionary<TKey, TValue> list, int sampleSize)
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
