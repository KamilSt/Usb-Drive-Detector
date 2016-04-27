using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace UsbDriveDetector
{
   public  class DeviceChangeNotifier : Form
    {
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVTYP_VOLUME = 0x00000002;
        public delegate void DeviceNotifyDelegate(Message msg);
        public static event DeviceNotifyDelegate DeviceNotify;
        private static DeviceChangeNotifier mInstance;

        public static void Start()
        {
            Thread t = new Thread(runForm);
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }
        public static void Stop()
        {
            if (mInstance == null) throw new InvalidOperationException("Notifier not started");
            DeviceNotify = null;
            mInstance.Invoke(new MethodInvoker(mInstance.endForm));
        }
        private static void runForm()
        {
            Application.Run(new DeviceChangeNotifier());
        }

        private void endForm()
        {
            this.Close();
        }
        protected override void SetVisibleCore(bool value)
        {
            // Prevent window getting visible
            if (mInstance == null) CreateHandle();
            mInstance = this;
            value = false;
            base.SetVisibleCore(value);
        }
        protected override void WndProc(ref Message m)
        {
            // Trap WM_DEVICECHANGE
            if (m.Msg == 0x219)
            {
                switch ((int)m.WParam)
                {
                    case DBT_DEVICEARRIVAL:
                        Console.WriteLine("New Device Arrived");

                        int devType = Marshal.ReadInt32(m.LParam, 4);
                        if (devType == DBT_DEVTYP_VOLUME)
                        {
                            DevBroadcastVolume vol;
                            vol = (DevBroadcastVolume)
                               Marshal.PtrToStructure(m.LParam,
                               typeof(DevBroadcastVolume));
                            Console.WriteLine("Mask is " + vol.Mask);
                        }

                        break;

                    case DBT_DEVICEREMOVECOMPLETE:
                        Console.WriteLine("Device Removed");
                        break;

                }
               
                //DeviceNotifyDelegate handler = DeviceNotify;
                //if (handler != null) handler(m);
            }
            base.WndProc(ref m);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct DevBroadcastVolume
    {
        public int Size;
        public int DeviceType;
        public int Reserved;
        public int Mask;
        public Int16 Flags;
    }
}
