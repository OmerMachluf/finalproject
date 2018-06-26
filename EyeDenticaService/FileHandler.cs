using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeDenticaService
{
    public static class FileHandler
    {
        public static void WriteToLog(string strWhatToWrite)
        {
            try
            { 
                StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Logfile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + strWhatToWrite);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
    }
}
