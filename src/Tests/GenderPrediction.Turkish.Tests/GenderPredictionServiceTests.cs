using System;
using System.Collections.Generic;
using System.Linq;
using GenderPrediction.Turkish.Contracts;
using GenderPrediction.Turkish.Models;
using Moq;
using Xunit;

namespace GenderPrediction.Turkish.Tests
{
    public class GenderPredictionServiceTests
    {
        [Fact]
        public void Predict_Should_Throw_ArgumentNullException_If_Given_Name_Is_Null_Or_Empty()
        {
            var  genderPredictionService = new GenderPredictionService(null);

            Assert.Throws<ArgumentNullException>(() => genderPredictionService.Predict(string.Empty));
            Assert.Throws<ArgumentNullException>(() => genderPredictionService.Predict((string) null));
        }

        [Theory]
        [InlineData("Yiğit", "YIGIT", "1")]
        [InlineData("Deniz", "DENIZ", "1")]
        [InlineData("Çiğdem", "CIGDEM", "0")]
        [InlineData("Özge", "OZGE", "0")]
        public void Predict_Should_Convert_Name_To_Upper_English_Character(string name, string normalizedName, string @class)
        {
            var predictionEngineMock = new Mock<IGenderPredictionEngine>(MockBehavior.Strict);

            var genderPredictionResult = new GenderPredictionResult() {Class = @class, Score = new float[] {50, 50}};

            predictionEngineMock
                .Setup(engine => engine.Predict(It.Is<GenderClassificationData>(data => data.Name == normalizedName)))
                .Returns(genderPredictionResult);

            GenderPredictionService genderPredictionService = new GenderPredictionService(predictionEngineMock.Object);
            GenderPredictionModel predictionModel = genderPredictionService.Predict(name);

            predictionEngineMock.Verify(engine => engine.Predict(It.IsAny<GenderClassificationData>()), Times.Once());
        }

        [Theory]
        [InlineData("Ayşe", "0")]
        [InlineData("Erol", "1")]
        public void Predict_Should_Convert_GenderPredictionResult_Class_String_Value_To_Right_Gender_Enum_Value(string name, string @class)
        {
            var predictionEngineMock = new Mock<IGenderPredictionEngine>(MockBehavior.Strict);

            var genderPredictionResult = new GenderPredictionResult() { Class = @class, Score = new float[] { 50, 50 } };

            predictionEngineMock
                .Setup(engine => engine.Predict(It.IsAny<GenderClassificationData>()))
                .Returns(genderPredictionResult);

            GenderPredictionService genderPredictionService = new GenderPredictionService(predictionEngineMock.Object);
            GenderPredictionModel predictionModel = genderPredictionService.Predict(name);

            Assert.Equal(@class == "1" ? Gender.Male : Gender.Female, predictionModel.PredictedGender);

            predictionEngineMock.Verify(engine => engine.Predict(It.IsAny<GenderClassificationData>()), Times.Once());
        }

        [Theory]
        [InlineData("Deniz","0" , 45.2, 55.8)]
        [InlineData("Muzaffer", "1", 80.5, 19.5)]
        public void Predict_Should_Return_Gender_Scores_With_Right_Order_First_Male_Second_Female(string name, string @class, float maleScore, float femaleScore)
        {
            var predictionEngineMock = new Mock<IGenderPredictionEngine>(MockBehavior.Strict);

            var genderPredictionResult = new GenderPredictionResult() { Class = @class, Score = new[] { maleScore, femaleScore } };

            predictionEngineMock
                .Setup(engine => engine.Predict(It.IsAny<GenderClassificationData>()))
                .Returns(genderPredictionResult);

            GenderPredictionService genderPredictionService = new GenderPredictionService(predictionEngineMock.Object);
            GenderPredictionModel predictionModel = genderPredictionService.Predict(name);

            var predictedMaleScore = predictionModel.Score[Gender.Male];
            var predictedFemaleScore = predictionModel.Score[Gender.Female];

            Assert.Equal(maleScore, predictedMaleScore);
            Assert.Equal(femaleScore, predictedFemaleScore);

            predictionEngineMock.Verify(engine => engine.Predict(It.IsAny<GenderClassificationData>()), Times.Once());
        }

        [Fact]
        public void Predict_Should_Throw_ArgumentNullException_If_Given_Names_Is_Null()
        {
            var genderPredictionService = new GenderPredictionService(null);

            Assert.Throws<ArgumentNullException>(() =>
            {
                var genderPredictionModels = genderPredictionService.Predict((IList<string>) null);
                genderPredictionModels.ToList();
            });
        }

        [Fact]
        public void Predict_Should_Return_Same_Number_Of_Amount_Predictions_As_Given_Names()
        {
            var predictionEngineMock = new Mock<IGenderPredictionEngine>(MockBehavior.Strict);

            var genderPredictionResult = new GenderPredictionResult() { Class = "1", Score = new float[] { 50, 50 } };

            predictionEngineMock
                .Setup(engine => engine.Predict(It.IsAny<GenderClassificationData>()))
                .Returns(genderPredictionResult);

            IList<string> names = new List<string>(){"Deniz", "Engin", "Serhat", "Fatma", "Hatice"};

            GenderPredictionService genderPredictionService = new GenderPredictionService(predictionEngineMock.Object);
            IList<GenderPredictionModel> genderPredictionModels = genderPredictionService.Predict(names).ToList();

            Assert.Equal(names.Count, genderPredictionModels.Count);
            predictionEngineMock.Verify(engine => engine.Predict(It.IsAny<GenderClassificationData>()), Times.Exactly(names.Count));
        }
    }
}
