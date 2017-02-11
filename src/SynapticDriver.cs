using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace synaptic_driver_reviver
{
    public class SynapticDriver
    {
        private readonly HashSet<string> _synapticProcs = new HashSet<string>();
        public SynapticDriver()
        {
            _synapticProcs.Add("SynTPEnh");
            _synapticProcs.Add("SynTPEnhService"); // "SynTPEnhService"
            _synapticProcs.Add("SynTPEnhHelper");
        }

        public void Revive()
        {
            ResetProcs();
            RestartService("SynTPEnhService", 3000);
        }

        public void ResetProcs()
        {
            try
            {
                // SynTPEnh.exe
                // SynTPEnhService.exe
                // SynTPHelper.exe
                foreach (Process proc in Process.GetProcesses())
                {
                    string procName = proc.ProcessName;
                    if (_synapticProcs.Contains(procName))
                    {
                        proc.Kill();
                    }
                }
                string procPath = string.Empty;// @"C:\Program Files\Synaptics\SynTP\SynTPEnh.exe";
                if (!File.Exists(procPath))
                {
                    procPath = Path.Combine(GetInstallPahViaRegistry(), "SynTPEnh.exe");
                }
                Process.Start(procPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RestartService(string serviceName, int timeoutMilliseconds)
        {
            return;
            var service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                // Must run as admin/highest privilege
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                // ...
                Console.WriteLine(ex.Message);
            }
        }

        private static string GetInstallPahViaRegistry()
        {
            // HKEY_LOCAL_MACHINE\SOFTWARE\Synaptics\SynTP\Install
            using (var regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Synaptics\SynTP\Install", true))
            {
                if (regKey != null)
                {
                    return (string)regKey.GetValue("ProgDir");
                }
            }
            return null;
        }
    }
}
