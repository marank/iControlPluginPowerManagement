using System;
using System.Collections.Generic;
using iControlInterfaces;

using System.Runtime.InteropServices;

namespace iControlPluginPowerManagement {
    public class iControlPlugin : IiControlPlugin {

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

        private IiControlPluginHost pluginHost;
        public IiControlPluginHost Host {
            set {
                pluginHost = value;
            }
            get {
                return pluginHost;
            }
        }

        public bool Init() {
            string configFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", "iControlPluginPowerManagement.config");
            if (System.IO.File.Exists(configFile)) {
                Dictionary<string, string> settings = pluginHost.DeserializeJSON(configFile);
                bool value;
                if (settings.ContainsKey("enabled") && Boolean.TryParse(settings["enabled"], out value) && value == false) {
                    pluginHost.Log("Plugin disabled", this);
                    return false;
                }
            }
            return true;
        }


        public void Handle(string[] commands, IiControlClient client) {
            if (commands[0] == "pwrmngmt") {
                switch (commands[1]) {
                    case "shutdown":
                        pluginHost.Log("Shutting down", this);
                        System.Diagnostics.Process.Start("shutdown.exe", "-s -f -t 0");
                        break;

                    case "restart":
                        pluginHost.Log("Restarting", this);
                        System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 0");
                        break;

                    case "logoff":
                        pluginHost.Log("Logging of the current user", this);
                        System.Diagnostics.Process.Start("shutdown.exe", "-l -f -t 0");
                        break;

                    case "lock":
                        pluginHost.Log("Lock workstation", this);
                        LockWorkStation();
                        break;

                    case "hibernate":
                        pluginHost.Log("Hibernating", this);
                        System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Hibernate, true, true);
                        break;

                    case "standby":
                        pluginHost.Log("Going to stand by", this);
                        System.Windows.Forms.Application.SetSuspendState(System.Windows.Forms.PowerState.Suspend, true, true);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
