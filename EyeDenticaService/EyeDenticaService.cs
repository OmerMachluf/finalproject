using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Configuration;
using System.ServiceModel;
using System.Diagnostics;

namespace EyeDenticaService
{
    public partial class EyeDenticaService : ServiceBase
    {
        DataService dataService;
        public ServiceHost dataServiceHost = null;
        public const string BasePath = @"C:\Users\krist\new\temp\";
        Thread watchdog, logsProcessor;

        enum Commands
        {
            StartBuilding = 200,
            StartTesting = 201,
            StartDtour = 202
        }

        public EyeDenticaService()
        {
            this.ServiceName = "EyeDenticaService";
            InitializeComponent();
        }

        private void initializeFiles()
        {
            try
            {
                /*if (!File.Exists(Consts.PREDICTION_FILE_PATH))
                {
                    StreamWriter sw = new StreamWriter(Consts.PREDICTION_FILE_PATH, true);
                    sw.WriteLine("0");
                }*/

                File.WriteAllText(BasePath + "mode.csv", "indication\n-\n");
            }
            catch(Exception e)
            {
                Console.WriteLine("Break here");
            }
        }

        private void Watchdog()
        {
            Thread.CurrentThread.Name = "RunningAgentKL";
            while (true)
            {
                ProcessHandler.CheckAndStartProcess(Consts.AGENT_NAME, @Consts.AGENT_LOCATION + Consts.AGENT_NAME + ".exe");
                ProcessHandler.CheckAndStartProcess(Consts.KEY_LOGGER_NAME, @Consts.KEY_LOGGER_LOCATION + Consts.KEY_LOGGER_NAME + ".exe");
                Thread.Sleep(2000);
            }
        }

        public static void updatePrediction()
        {
            double dPrediction;
            Thread.CurrentThread.Name = "PredictionUpdatesFromTextFile";
            while (true)
            {
                try
                {
                    dPrediction = double.Parse(File.ReadAllLines(BasePath + "prediction.txt")[0]) * 100;
                    DataService.PredictionRate = dPrediction;
                }

                catch (Exception e)
                {
                    Console.WriteLine("Break here");
                }

                Thread.Sleep(200);
            }
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            initializeFiles();
            
            watchdog = new Thread(new ThreadStart(Watchdog));
            watchdog.IsBackground = true;
            watchdog.Start();
            if (logsProcessor==null)
            {
                logsProcessor = new Thread(new ThreadStart(Data2Csv.processLogs));
                logsProcessor.Start();
            }

            // Update prediction funftion
            new Thread(updatePrediction).Start();

            PythonScriptRunner.StartRInstances_SeparateProcess(BasePath + "Mlscript.py");

            initDataService();
        }

        private void initDataService()
        {
            if (dataServiceHost != null)
            {
                dataServiceHost.Close();
            }

            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Name = "binding1";
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.Security.Mode = BasicHttpSecurityMode.None;

            Uri baseAddress = new Uri("http://localhost:8000/DataService/service");
            Uri address = new Uri("http://localhost:8000/DataService/service/mex");

            // Create a ServiceHost for the CalculatorService type and provide the base address.
            dataServiceHost = new ServiceHost(typeof(DataService));

           // dataServiceHost.AddServiceEndpoint(typeof(IDataService), binding, address);

            dataServiceHost.Open();
        }

        protected override void OnStop()
        {
            base.OnStop();

            watchdog.Abort();
            logsProcessor.Abort();
            ProcessHandler.CheckAndSKillProcess("Agent");
            ProcessHandler.CheckAndSKillProcess("EyeKeyLoggerP");
            
            if (dataServiceHost != null)
            {
                dataServiceHost.Close();
                dataServiceHost = null;
            }
        }

        protected override void OnCustomCommand(int command)
        {
            base.OnCustomCommand(command);
            dataService = new DataService();

            switch (command)
            {
                case (int)Commands.StartBuilding:
                    Data2Csv.EyeDenticaMode = Data2Csv.EYEDENTICA_MODE.LEARN;
                    break;
                case (int)Commands.StartTesting:
                    Data2Csv.EyeDenticaMode = Data2Csv.EYEDENTICA_MODE.MONITOR;
                    break;
                case (int)Commands.StartDtour:
                    Data2Csv.EyeDenticaMode = Data2Csv.EYEDENTICA_MODE.DTOUR;
                    break;
                default:
                    break;
            }
        }
    }
}
