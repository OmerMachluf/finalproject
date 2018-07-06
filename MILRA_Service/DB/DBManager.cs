using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using MongoDB.Bson;

namespace MILRA_Service.DB
{
    class DBManager
    {
        
        string sqlCon;


        public DBManager()
        {

            MongoDBUtils.Connect();
          //  AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());
           // sqlCon =

           //        "Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True";

            //       @"Data Source=.\SQLEXPRESS;" +
            //       @"AttachDbFilename=|DataDirectory|\MilraDB.mdf;
            //       Integrated Security=True;
            //       Connect Timeout=30;
            //       User Instance=True";
            //  conn = new SqlConnection(sqlCon);

            //  conn.ConnectionString =
            //      "Server =.\\SQLExpress; AttachDbFilename =|DataDirectory|MilraDB.mdf; Database = MilraDB; Trusted_Connection = Yes;";

          //  @"Data Source = (localdb)\v12.0;" +
          //       "User Instance=true;" +
          //       "Integrated Security=true;" +
          //      @"AttachDbFilename = C:\mf\projectTfs\MILRA\MILRA_Service\MILRA_Service\DB\MilraDB.mdf;";
        }

        public async Task<List<RecordObject>> getRecordsAfterAsync(long time)
        {

            List<RecordObject> records = new List<RecordObject>();

            //try
            //{
            //    using (SqlConnection conn = new SqlConnection(sqlCon))
            //    {
            //        if (conn.State == ConnectionState.Closed)
            //        {
            //            conn.Open();


            //            using (SqlCommand command = new SqlCommand("SELECT time,operation,keyStroke,window FROM Records WHERE time > @t Order By time", conn))
            //            {
            //                //
            //                // Add new SqlParameter to the command.
            //                //
            //                command.Parameters.Add(new SqlParameter("t", time));
            //                SqlDataReader reader;
            //                //  SqlCommand command =
            //                //       new SqlCommand("SELECT time,operation,keyStroke,window " +
            //                //                      "FROM Records WHERE time>'" +
            //                //                       time +
            //                //                       "' Order By time;", conn);


            //                reader = command.ExecuteReader();

            //                if (reader.HasRows)
            //                {
            //                    while (reader.Read())
            //                    {
            //                        records.Add(new RecordObject(reader.GetInt64(0),
            //                            reader.GetString(1), reader.GetString(2), reader.GetString(3)));

            //                    }
            //                }
            //                else
            //                {
            //                    Console.WriteLine("No rows found.");
            //                }
            //            }
            //        }
            //    }
            //}
            //catch(Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //}

            records = await MongoDBUtils.RetriveRecordsAfter(time);
            return records;
            }

        internal List<PredictionObject> getPredictionsAfter(long time)
        {
            List<PredictionObject> predictions = new List<PredictionObject>();
            //SqlDataReader reader;

            //using (SqlConnection conn = new SqlConnection(sqlCon))
            //{
            //    if (conn.State == ConnectionState.Closed)
            //    {
            //        conn.Open();
            //        SqlCommand command =
            //            new SqlCommand("SELECT time,prediction " +
            //                           "FROM Records WHERE time > " +
            //                           time +
            //                           " Order By time;", conn);

            //        reader = command.ExecuteReader();

            //        if (reader.HasRows)
            //        {
            //            while (reader.Read())
            //            {
            //                predictions.Add(new PredictionObject(reader.GetInt64(0),
            //                    reader.GetDouble(1)));

            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("No rows found.");
            //        }
            //    }
            //}

          //  predictions = MongoDBUtils.retriveRecordsAfter(time);

            return predictions;
        }

        public void insertRecord(RecordObject record)
        {
            //using (SqlConnection conn = new SqlConnection(sqlCon))
            //{
            //    if (conn.State == ConnectionState.Closed)
            //    {
            //        conn.Open();
            //        SqlCommand command =
            //            new SqlCommand("INSERT INTO Records " +
            //                           "VALUES(" +
            //                           record.Time + ", " +
            //                           record.Operation + ", " +
            //                           record.KeyWord + ", " +
            //                           record.Window + ");",
            //                           conn);

            //        command.ExecuteReader();
            //    }
            //}

            MongoDBUtils.injectRecord(record);
        }

        public void insertPrediction(PredictionObject pred)
        {
            using (SqlConnection conn = new SqlConnection(sqlCon))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    SqlCommand command =
                        new SqlCommand("INSERT INTO Predictions " +
                                       "VALUES(" +
                                       pred.TimeStamp + ", " +
                                       pred.Prediction + ");",
                                       conn);

                    command.ExecuteReader();
                }
            }
        }

    }
}
