using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenderPrediction.Turkish.TestData
{
    public class TableRow
    {
        public string Name { get; set; }

        public string PredictedGender { get; set; }

        public string TestDataGender { get; set; }

        public string ScoreMale { get; set; }

        public string ScoreFemale { get; set; }

        public float UnisexProbability { get; set; }
    }
}
