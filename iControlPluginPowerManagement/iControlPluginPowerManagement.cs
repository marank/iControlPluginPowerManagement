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

        public string Version {
            get {
                return "0.0.1";
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

        private string _configpath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins", "iControlPluginPowerManagement.config");
        private Dictionary<string, object> _settings;

        public bool Init() {
            if (System.IO.File.Exists(_configpath)) {
                _settings = Host.DeserializeJSON(_configpath);
                if (_settings.ContainsKey("enabled") && Convert.ToBoolean(_settings["enabled"]) == false) {
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

        public void Enable() {
            _settings["enabled"] = "true";
            Host.SerializeJSON(_configpath, _settings);
        }

        public void Disable() {
            _settings["enabled"] = "false";
            Host.SerializeJSON(_configpath, _settings);
        }
    }
}
