using MILRA_Service.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MILRA_Service
{
    class DataService : IDataService
    {
        private Object recordsLocker;
        private Object predictionsLocker;
        private DBManager dbManager;
        private long recordsLastTImeStamp = 0;
        private long predictionsLastTImeStamp = 0;

        public static double PredictionRate { get; set; }

        public DataService ()
        {
            recordsLocker = new object();
            dbManager = new DBManager();
        }

        public async Task<List<RecordObject>> getNewRecords()
        {
            if (recordsLastTImeStamp == 0)
            {
                recordsLastTImeStamp = DateTime.Now.Ticks;
            }

            List<RecordObject> listToReturn = await dbManager.getRecordsAfterAsync(recordsLastTImeStamp);

            if (listToReturn.Count > 0)
            {
                recordsLastTImeStamp = listToReturn[listToReturn.Count - 1].Time;
            }

            return listToReturn;
        }

        public List<PredictionObject> getNewPredictions ()
            {
            List<PredictionObject> listToReturn;
            lock (predictionsLocker)
                {
                listToReturn = dbManager.getPredictionsAfter(predictionsLastTImeStamp);

                if (listToReturn.Count > 0)
                {
                    predictionsLastTImeStamp = listToReturn[listToReturn.Count - 1].TimeStamp;
                }
            }

            return listToReturn;
        }

        public void flushUserRecord(string record)
        {
            var recObj = new RecordObject(record);
            lock (recordsLocker)
            {
                dbManager.insertRecord(recObj);
            }
        }

        public void updatePrediction(double pred)
        {
            DataService.PredictionRate = pred;
        }

        public double getPrediction()
        {
            return DataService.PredictionRate ;
        }
    }
}
