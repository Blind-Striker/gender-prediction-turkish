using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GenderPrediction.Turkish.Contracts;
using Xunit;

namespace GenderPrediction.Turkish.Tests
{
    public class GenderPredictionStandaloneTests
    {
        [Fact]
        public void Create_Should_Create_IGenderPredictionService_With_Default_GenderPredictionEngine()
        {
            IGenderPredictionService genderPredictionService = GenderPredictionStandalone.Create();

            FieldInfo fieldInfo = genderPredictionService.GetType()
                .GetField("_predictionEngine", BindingFlags.NonPublic | BindingFlags.Instance);

            object predictionEngine = fieldInfo?.GetValue(genderPredictionService);

            Assert.IsType<GenderPredictionService>(genderPredictionService);
            Assert.NotNull(fieldInfo);
            Assert.NotNull(predictionEngine);
            Assert.Equal(typeof(GenderPredictionEngine), predictionEngine.GetType());
        }
    }
}
