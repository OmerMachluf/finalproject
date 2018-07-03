using EyeDenticaService.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EyeDenticaService
{
    public static class Data2Csv
    {

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
        private static string fileName;
        public static List<String> USER = new List<String>();
        public static List<double> lstP = new List<double>();
        public static List<double> lstH = new List<double>();
        public static List<double> lstHD = new List<double>();
        public static EYEDENTICA_MODE EyeDenticaMode = EYEDENTICA_MODE.NONE;


        static Data2Csv()
        {

            // Init Global Variables
            /*File.Delete("C:\\Users\\krist\\new\\temp\\training.csv");
            File.Copy("C:\\Users\\krist\\new\\temp\\start.csv", "C:\\Users\\krist\\new\\temp\\training.csv");

            File.WriteAllText("C:\\Users\\krist\\new\\temp\\predict.csv", string.Format("{0},{1},{2},{3}\n", "P_Time", "H_Time", "H_D_Time", "user"));
        */}


        private static void writeChunk()
        {
            if (EyeDenticaMode != EYEDENTICA_MODE.DTOUR)
            {
                if ((lstP.Count > 0) && (lstH.Count > 0))
                {
                    File.AppendAllText(fileName,
                        string.Format("{0},{1},{2},{3}\n",
                        string.Format("{0}", lstP.Average().ToString()),
                        string.Format("{0}", lstH.Average().ToString()),
                        string.Format("{0}", lstHD.Count > 0 ? lstHD.Average().ToString() : "null"),
                        classificationUserName));
                    lstP.Clear();
                    lstH.Clear();
                    lstHD.Clear();
                }
            }
        }

        public static void processLogs(List<RecordObject> rawData)
        {
            if (EyeDenticaMode == EYEDENTICA_MODE.NONE)
            {
                return;
            }
            else if (EyeDenticaMode == EYEDENTICA_MODE.LEARN)
            {
                classificationUserName = "PcOwner";
                fileName = "C:\\Users\\krist\\new\\temp\\training.csv";
            }
            else
            {
                classificationUserName = "NotPcOwner";
                fileName = "C:\\Users\\krist\\new\\temp\\predict.csv";
            }

            //  Start
            if (rawData.Count > 0)
            {

                // Going over new records
                foreach (RecordObject currRawLine in rawData)
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
        }
    }
}