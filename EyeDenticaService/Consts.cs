using System;
using System.Configuration;

namespace EyeDenticaService
{
    public static class Consts
    {
        // Program consts              
        public static long CHUNK_PERIOD;
        public static long MAX_PRESSING_DELTA;
        public static long MAX_HOVERING_DELTA;
        public static long MAX_DELETING_DELTA;
        public static string CURRENT_USER;

        public static string PREDICTION_FILE_PATH;
        public static string KEY_LOGGER_LOCATION;
        public static string KEY_LOGGER_NAME;
        public static string LOG_2_CSV_LOCATION;
        public static string LOG_2_CSV_NAME;
        public static string AGENT_LOCATION;
        public static string AGENT_NAME;

        static Consts()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationManager.RefreshSection(config.AppSettings.SectionInformation.Name);

            CHUNK_PERIOD = new TimeSpan(0, 0, int.Parse(ConfigurationManager.AppSettings["CHUNK_PERIOD"])).Ticks;
            MAX_PRESSING_DELTA = new TimeSpan(0, 0, int.Parse(ConfigurationManager.AppSettings["MAX_PRESSING_DELTA"])).Ticks;
            MAX_HOVERING_DELTA = new TimeSpan(0, 0, int.Parse(ConfigurationManager.AppSettings["MAX_HOVERING_DELTA"])).Ticks;
            MAX_DELETING_DELTA = new TimeSpan(0, 0, int.Parse(ConfigurationManager.AppSettings["MAX_DELETING_DELTA"])).Ticks;
            CURRENT_USER = ConfigurationManager.AppSettings["CURRENT_USER"];

            PREDICTION_FILE_PATH = (ConfigurationManager.AppSettings["PREDICTION_FILE_PATH"]).ToString();

            KEY_LOGGER_LOCATION = (ConfigurationManager.AppSettings["KeyLoggerLocation"]).ToString();
            KEY_LOGGER_NAME = (ConfigurationManager.AppSettings["KeyLoggerName"]).ToString();
            AGENT_LOCATION = (ConfigurationManager.AppSettings["AgentLocation"]).ToString();
            AGENT_NAME = (ConfigurationManager.AppSettings["AgentName"]).ToString();
        }
    }
}