using EyeDenticaService.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LINQtoCSV;
using System.Text;
using System.Threading;

namespace EyeDenticaService
{
    public static class Data2Csv
    {
        public class CsvRow
        {
            public string Pi { get; set; }
            public string Hi { get; set; }
            public string Di { get; set; }

            public CsvRow()
            {
                Pi = P.ToString();
                Di = D.ToString();
                Hi = H.ToString();
            }
        }

        public static double PredDtour = 0;
        private enum INPUT_ORDER : int
        {
            TIME = 0,
            ACTION_TYPE,
            ACTION,
            WINDOW
        }


        public enum EYEDENTICA_MODE : int
        {
            LEARN = 0,
            MONITOR,
            DTOUR,
            NONE
        }

        // Variables 
        private static Dictionary<string, long> logger = new Dictionary<string, long>();
        private static long chunkStart = 0;
        private static long lastPress = 0;
        private static long lastDelete = 0;
        private static int numberOfElseP = 0, numberOfElseH = 0, numberOfElseD = 0;
        private static int numberOfP = 0, numberOfH = 0, numberOfD = 0;
        private static string classificationUserName;
        private const string basePath = @"C:\Users\krist\new\temp\";

        private static string fileName;
        private const string trainFile = basePath + "training.csv";
        private const string predictFile = basePath+ "predict.csv";
        private static DataService DM = new DataService();
        public static List<String> USER = new List<String>();
        public static List<double> lstP = new List<double>();
        public static List<double> lstH = new List<double>();
        public static List<double> lstHD = new List<double>();
        public static EYEDENTICA_MODE EyeDenticaMode = EYEDENTICA_MODE.NONE;
        public static double P, H, D;


        static Data2Csv()
        {

            // Init Global Variables
            /*File.Delete("C:\\Users\\krist\\new\\temp\\training.csv");
            File.Copy("C:\\Users\\krist\\new\\temp\\start.csv", "C:\\Users\\krist\\new\\temp\\training.csv");

            File.WriteAllText("C:\\Users\\krist\\new\\temp\\predict.csv", string.Format("{0},{1},{2},{3}\n", "P_Time", "H_Time", "H_D_Time", "user"));
        */
        }
        /* private static IEnumerable<CsvRow> GetLinesFromFile()
         {
             return File.ReadAllLines(fileName)
                    .Select(x => x.Split(','))
                    .Select(x => new CsvRow
                    {
                        Pi = x[0],
                        Hi = x[1],
                        Di = x[2]
                    });       
         }

         private static void WriteLinesToFile(List<CsvRow> csvRows)
         {
             string
             foreach (CsvRow csvRow in csvRows)
             {

             }
             File.WriteAllLines()

             return File.ReadAllLines(fileName)
                    .Select(x => x.Split(','))
                    .Select(x => new CsvRow
                    {
                        Pi = x[0],
                        Hi = x[1],
                        Di = x[2]
                    });
         }*/
        private static void writeChunk()
        {
            if (EyeDenticaMode != EYEDENTICA_MODE.DTOUR)
            {
                if ((lstP.Count > 0) && (lstH.Count > 0))
                {
                    /*CsvFileDescription csvFileDescription = new CsvFileDescription
                    {
                        SeparatorChar = '\t',
                        FirstLineHasColumnNames = true,
                        EnforceCsvColumnAttribute = true
                    };*/

                    P = lstP.Average();
                    H = lstH.Average();
                    D = lstHD.Count > 0 ? lstHD.Average() : 0;
                    string[] rowsO;
                    while (true)
                    {
                        try
                        {
                            rowsO = File.ReadAllLines(fileName);
                            break;
                        }
                        catch (Exception e)
                        {
                            Thread.Sleep(2000);
                        }
                    }

                    string row = string.Format("{0}, {1}, {2}", P, H, D);
                    string[] rows = new string[rowsO.Length + 1];
                    rowsO.CopyTo(rows, 0);
                    rows[rowsO.Length] = row;
                    while (true)
                    {
                        try
                        {
                            File.WriteAllLines(fileName, rows);
                            break;
                        }
                        catch (Exception e)
                        {
                            Thread.Sleep(2000);
                        }
                    }
                    /*CsvContext csvContext = new CsvContext();
                    CsvRow csvRow = new CsvRow();
                    List<CsvRow> csvRows = GetLinesFromFile().ToList<CsvRow>();
                    csvContext.Write<CsvRow>(csvRows, fileName, csvFileDescription);*/

                    lstP.Clear();
                    lstH.Clear();
                    lstHD.Clear();
                }
            }
        }

        public async static void processLogs()
        {
            Thread.CurrentThread.Name = "Log Processor";
            while (true)
            {
                if (EyeDenticaMode == EYEDENTICA_MODE.NONE)
                {
                    Thread.Sleep(3000);
                    continue;
                }
                else if (EyeDenticaMode == EYEDENTICA_MODE.LEARN)
                {
                    fileName = trainFile;
                }

                else
                {
                    fileName = predictFile;
                }

                List<RecordObject> records = await DM.getNewRecords();
                if (records.Count > 0)
                {
                    foreach (RecordObject currRawLine in records)
                    {
                        long currActionTime = currRawLine.Time;

                        // Pressing action

                        if (!"[Re]".Equals(currRawLine.Operation))
                        {
                            if ("[Pr]".Equals(currRawLine.Operation))
                            {
                                // If DELETE was pressed
                                if (("[Back]".Equals(currRawLine.KeyWord)) || ("[BS]".Equals(currRawLine.KeyWord)))
                                {
                                    long currPressingDeleteDelta = currActionTime - lastDelete;
                                    lastDelete = currActionTime;

                                    // If subtraction is more than a chunk size - create a new chunk
                                    if (currActionTime - chunkStart > Consts.CHUNK_PERIOD)
                                    {
                                        writeChunk();
                                        chunkStart = currActionTime;
                                    }

                                    // If pressing delta is less than max delta
                                    if (currPressingDeleteDelta < Consts.MAX_DELETING_DELTA)
                                    {
                                        numberOfD++;
                                        lstHD.Add(currPressingDeleteDelta);
                                    }
                                    else
                                        numberOfElseD++;

                                }
                                // If regular press
                                else
                                {
                                    logger.Remove(currRawLine.KeyWord);
                                    logger.Add(currRawLine.KeyWord, currActionTime);
                                    if (currRawLine.KeyWord != string.Empty)
                                    {
                                        var a = File.ReadAllLines(@"C:\Users\krist\new\temp\all.txt");
                                        string[] b = new string[a.Length + 1];
                                        a.CopyTo(b, 0);
                                        b[a.Length] = currRawLine.KeyWord;
                                        File.WriteAllLines(@"C:\Users\krist\new\temp\all.txt", b);
                                    }
                                    long currPressingDelta = currActionTime - lastPress;
                                    lastPress = currActionTime;

                                    // If subtraction is more than a chnk size - create a new chank
                                    if (currActionTime - chunkStart > Consts.CHUNK_PERIOD)
                                    {
                                        writeChunk();
                                        chunkStart = currActionTime;
                                    }

                                    // If less than max delta
                                    if (currPressingDelta < Consts.MAX_HOVERING_DELTA)
                                    {
                                        lstH.Add(currPressingDelta);
                                        numberOfH++;
                                    }
                                    else
                                        numberOfElseH++;
                                }
                            }
                        }
                        // Release action
                        else if ("[Re]".Equals(currRawLine.Operation))
                        {
                            if (currActionTime - chunkStart > Consts.CHUNK_PERIOD)
                            {
                                writeChunk();
                                chunkStart = currActionTime;
                            }

                            if (logger.ContainsKey(currRawLine.KeyWord))
                            {
                                long currPressingPeriod = currActionTime - logger[currRawLine.KeyWord];

                                // If less than max delta
                                if (currPressingPeriod < Consts.MAX_PRESSING_DELTA)
                                {
                                    numberOfP++;
                                    lstP.Add(currPressingPeriod);
                                }
                                else
                                    numberOfElseP++;

                                logger.Remove(currRawLine.KeyWord);
                            }
                        }
                    }
                }

                Thread.Sleep(4500);
            }
        }
    }
}