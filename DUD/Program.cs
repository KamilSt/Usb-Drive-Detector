using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UsbDriveDetector;

namespace DUD
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceChangeNotifier.Start();

            Console.ReadLine();
        }
    }



}
