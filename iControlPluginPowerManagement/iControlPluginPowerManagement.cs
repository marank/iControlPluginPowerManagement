using System;

using System.Runtime.InteropServices;

namespace iControlPluginPowerManagement {
    public class iControlPlugin : iControlPluginInterface.IiControlPlugin {

        [DllImport("user32")]
        public static extern void LockWorkStation();

        public string Name {
            get {
                return "Power Management";
            }
        }

        public string Author {
            get {
                return "Matthias Rank";
            }
        }

        private iControlPluginInterface.IiControlPluginHost pluginHost;
        public iControlPluginInterface.IiControlPluginHost Host {
            set {
                pluginHost = value;
            }
            get {
                return pluginHost;
            }
        }


        public void Handle(string[] commands, string ip) {
            switch (commands[0]) {
                case "shutdown":
                    pluginHost.Log("Shutting down", this);
                    System.Diagnostics.Process.Start("shutdown.exe", "-s -f -t 0");
                    break;

                case "restart":
                    System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 0");
                    break;

                case "logoff":
                    System.Diagnostics.Process.Start("shutdown.exe", "-l -f -t 0");
                    break;

                case "sperren":
                    LockWorkStation();
                    break;

                case "hibernate":
                    System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Hibernate, true, true);
                    break;

                case "standby":
                case "stand-by":
                    System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Suspend, true, true);
                    break;

                default:
                    break;
            }
        }
    }
}
