using System.Collections.Concurrent;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MILRA_Service.DB
{
    public static class MongoDBUtils
    {
        private static bool ActivateMongo = true;
        private static MongoClient Client;
        private static IMongoDatabase Database;
        private static IMongoCollection<BsonDocument> RecordsCollection;
        private static IMongoCollection<BsonDocument> PredictionsCollection;
        private static IMongoCollection<BsonDocument> FitchersCollection;

        static MongoDBUtils()
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
            MongoDBUtils.Connect();
        }

        public static void Connect()
        {
            MongoDBUtils.Client = new MongoClient();
            MongoDBUtils.Database = Client.GetDatabase("MilraDB");
            string targetRecordsCollection = "Records";
            bool alreadyExistsRecords = Database.ListCollections().ToList().Any(x => x.GetElement("name").Value.ToString() == targetRecordsCollection);
            if (!alreadyExistsRecords)
            {
                Database.CreateCollection(targetRecordsCollection);
            }
            MongoDBUtils.RecordsCollection = Database.GetCollection<BsonDocument>(targetRecordsCollection);
            string targetPredictionsCollection = "Predictions";
            bool alreadyExistsPredictions = Database.ListCollections().ToList().Any(x => x.GetElement("name").Value.ToString() == targetPredictionsCollection);
            if (!alreadyExistsPredictions)
            {
                Database.CreateCollection(targetPredictionsCollection);
            }
            MongoDBUtils.PredictionsCollection = Database.GetCollection<BsonDocument>(targetPredictionsCollection);
            string targetFitchersCollection = "Fitchers";
            bool alreadyExistsFitchers = Database.ListCollections().ToList().Any(x => x.GetElement("name").Value.ToString() == targetFitchersCollection);
            if (!alreadyExistsFitchers)
            {
                Database.CreateCollection(targetFitchersCollection);
            }
            MongoDBUtils.FitchersCollection = Database.GetCollection<BsonDocument>(targetFitchersCollection);
        }



        public async static Task<List<RecordObject>> RetriveRecordsAfter(long time)
        {
            List<RecordObject> recordsToReturn = new List<RecordObject>();
            MongoDBUtils.RecordsCollection = Database.GetCollection<BsonDocument>("Records");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Gt("Time", time);
            long timeO = 0;
            string operation = string.Empty, key = string.Empty, window = string.Empty;
            var sort = Builders<BsonDocument>.Sort.Ascending("Time");
            var result = await MongoDBUtils.RecordsCollection.Find(filter).Sort(sort).ToListAsync();
            foreach (BsonDocument res in result)
            {
                foreach (BsonElement el in res)
                {
                    if (el.Name.Equals("Time"))
                        timeO = (long)el.Value;
                    else if (el.Name.Equals("Operation"))
                        operation = (string)el.Value;
                    else if (el.Name.Equals("Key"))
                        key = (string)el.Value;
                    else if (el.Name.Equals("Window"))
                        window = (string)el.Value;
                }

                RecordObject obj = new RecordObject(timeO, operation, key, window);
                recordsToReturn.Add(obj);
            }
            return recordsToReturn;
        }

        public static void injectRecord(RecordObject rec)
        {
            try
            {
                BsonDocument doc = new BsonDocument
                    {
                         { "Time", rec.Time },
                         { "Operation", rec.Operation },
                         { "Key", rec.KeyWord },
                         { "Window", rec.Window }
                    };

                MongoDBUtils.RecordsCollection.InsertOne(doc);
            }
            catch
            {
                MongoDBUtils.Connect();
            }
        }
    }
}