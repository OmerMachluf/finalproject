using System.Collections.Concurrent;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EyeDenticaService.DB
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
            MongoDBUtils.Database = Client.GetDatabase("EyeDenticaDB");
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



        public static List<RecordObject> retriveRecordsAfter(long time)
        {
            List<RecordObject> recordsToReturn = new List<RecordObject>();
            
            var filter = Builders<BsonDocument>.Filter.Gt("Time", time);
            var sort = Builders<BsonDocument>.Sort.Ascending("Time");
            var result = MongoDBUtils.RecordsCollection.Find(filter).Sort(sort).ToListAsync();

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