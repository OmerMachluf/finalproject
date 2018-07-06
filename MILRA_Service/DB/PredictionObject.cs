using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MILRA_Service.DB
{
    class PredictionObject
    {

        public long TimeStamp { get; set; }
        public double Prediction { get; set; }

        public PredictionObject(long time, double pred)
        {
            TimeStamp = time;
            Prediction = pred;
        }
    }
}
