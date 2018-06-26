using System.Diagnostics;


namespace EyeDenticaService
{
    public static class ProcessHandler
    {
        private static Process IsAlreadyRunning(string strProcessName)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Equals(strProcessName))
                {
                    return clsProcess;
                }
            }

            return null;
        }

        public static void CheckAndStartProcess(string strProcessName, string strProcessPath)
        {
            if (IsAlreadyRunning(strProcessName) == null)
            {
                CreateProcessAsUserWrapper.LaunchChildProcess(strProcessPath, null);
            }
        }
        public static void CheckAndSKillProcess(string strProcessName)
        {
            Process pToKill = IsAlreadyRunning(strProcessName);

            if (pToKill != null)
            {
                pToKill.Kill();
            }
        }
    }
}
