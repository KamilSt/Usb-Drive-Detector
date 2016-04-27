using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

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

        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Stop UDD Service", EventLogEntryType.Information);
        }
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("Continue UDD Service", EventLogEntryType.Information);

        }
    }
}
