using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.ServiceProcess;
using System.Windows.Forms.Integration;

namespace Agent
{
    public partial class AgentForm : Form
    {

        private static DataServiceManager DSManger;
        AgentBoard board;
        public const string BasePath = @"C:\Users\krist\new\temp\";

        enum OperationType
        {
            StartBuilding = 200,
            StartTesting = 201,
            StartDtour = 202
        }

        public delegate void PercentageHandler(double percentage);
        private static PercentageHandler handler;

        public AgentForm()
        {
            InitializeComponent();
            DSManger = new DataServiceManager();
            handler = new PercentageHandler(RefreshPercentage);
        }

        public static void CheckPercentage()
        {
            while (true)
            {
                System.Random rnd = new System.Random();
                //ProfileHonestyRate.Rate = rnd.NextDouble() * rnd.Next(0, 100);
                ProfileHonestyRate.Rate = DSManger.getPredictionFromService();
                handler(ProfileHonestyRate.Rate);
            }
        }

        public void RefreshPercentage(double percentage)
        {
            try
            {
                this.contextMenuStrip1.Items[0].Text = percentage.ToString() + '%';
                Thread.Sleep(1000);
            }
            catch
            {

            }

        }

        public async void SendSMS(string number, string message)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "number", number },
                    { "message", message }
                };

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync("http://expert-penguin-5qak.rapidapi.io/sendsms", content);

                var responseString = await response.Content.ReadAsStringAsync();
            }
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            //this.SendSMS();
            //Environment2.LockWorkstation();

            Thread backgroundThread = new Thread(CheckPercentage);
            backgroundThread.Start();
        }

        private void Form1_Shown(object sender, System.EventArgs e)
        {
            this.Hide();
        }

        private void btnStartLearning_Click(object sender, System.EventArgs e)
        {
            ServiceController Controller = new ServiceController("EyeDenticaService");

            // Refresh the controller to find out if the service is up
            Controller.Refresh();

            if (Controller.Status == ServiceControllerStatus.Stopped)
            {
                Controller.Start();
            }

            if (Controller.Status == ServiceControllerStatus.Running)
            {
                string str = string.Empty;
                if (this.btnStartLearning.Text.Equals("הפעל למידה"))
                {
                    this.btnStartLearning.Text = "התחל בחינה";
                    Controller.ExecuteCommand((int)OperationType.StartBuilding);
                    str = "indication\nTRUE";
                }
                else
                {
                    this.btnStartLearning.Text = "הפעל למידה";
                    Controller.ExecuteCommand((int)OperationType.StartTesting);
                    str =  "indication\nFALSE";
                }

                File.WriteAllText(BasePath + "mode.csv", str);
            }
        }

        private void btnStartDtour_Click(object sender, System.EventArgs e)
        {
            ServiceController Controller = new ServiceController("EyeDenticaService");

            // Refresh the controller to find out if the service is up
            Controller.Refresh();

            if (Controller.Status == ServiceControllerStatus.Stopped)
            {
                Controller.Start();
            }
        }

        private void ToolStripMenuItem2_Click(object sender, System.EventArgs e)
        {
            board = new AgentBoard();
            ElementHost.EnableModelessKeyboardInterop(board);
            board.Show();
        }
    }

    public static class Environment2
    {
        /// <summary>
        /// Locks the workstation's display. Locking a workstation protects it from unauthorized use.
        /// </summary>
        /// <returns>
        /// If the function succeeds, the return value is true. Because the function executes asynchronously, a true return value indicates that the operation has been initiated. It does not indicate whether the workstation has been successfully locked.
        /// If the function fails, the return value is false. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("User32.Dll", EntryPoint = "LockWorkStation"), Description("Locks the workstation's display. Locking a workstation protects it from unauthorized use.")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool LockWorkStation();

        /// <summary>
        /// Locks the workstation's display. Locking a workstation protects it from unauthorized use.
        /// </summary>
        /// <exception cref="Win32Exception">if the lock fails more information can be found in this Exception class</exception>
        public static void LockWorkstation()
        {
            if (!LockWorkStation())
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public static class ProfileHonestyRate
    {
        public static double Rate { get; set; }
    }
}
