using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using UsbDriveDetector;

namespace UDDService
{
    public partial class Service1 : ServiceBase
    {
        private const string eventLogSource = "serviceUsbDriveDetector";
        private const string eventLogLog = "Service Usb Drive Detector";

        public Service1()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists(eventLogSource))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventLogSource, eventLogLog);
            }
            eventLog1.Source = eventLogSource;
            eventLog1.Log = eventLogLog;
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Start UDD Service", EventLogEntryType.Information);
            DeviceChangeNotifier.Start();
            DeviceChangeNotifier.DeviceNotify += DeviceChangeNotifier_DeviceNotify;
            DeviceChangeNotifier.DeviceNotifyVolume += DeviceChangeNotifier_DeviceNotifyVolume;     
        }

        private void DeviceChangeNotifier_DeviceNotifyVolume(System.Windows.Forms.Message msg, DevBroadcastVolume vol)
        {
            string result = string.Empty;
            result += "DeviceType:: " + vol.DeviceType+"\n";
            result += "Flags:: " + vol.Flags + "\n";
            result += "Mask:: " + vol.Mask + "\n";
            result += "Reserved:: " + vol.Reserved + "\n";
            result += "Size:: " + vol.Size + "\n";

            eventLog1.WriteEntry(result, EventLogEntryType.SuccessAudit);
        }

        private void DeviceChangeNotifier_DeviceNotify(System.Windows.Forms.Message msg)
        {
            eventLog1.WriteEntry(msg.ToString(), EventLogEntryType.SuccessAudit);
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Stop UDD Service", EventLogEntryType.Information);
            DeviceChangeNotifier.Stop();
        }
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("Continue UDD Service", EventLogEntryType.Information);
            DeviceChangeNotifier.Stop();
            DeviceChangeNotifier.Start();

        }
    }
}
