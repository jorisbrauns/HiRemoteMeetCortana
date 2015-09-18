using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace pi_wake_up_light
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Stopwatch stopwatch;
        public MainPage()
        {
            this.InitializeComponent();
            GpioController controller = GpioController.GetDefault();




            var servoPin = controller.OpenPin(26);
            servoPin.SetDriveMode(GpioPinDriveMode.Output);
            stopwatch = Stopwatch.StartNew();
            int teller1 = 1;
            int teller2 = 1;
            int teller3 = 0;
            while (true)
            {
                
                    servoPin.Write(GpioPinValue.High);
                
                Wait(teller1);
                //The pulse if over and so set the pin to low and then wait until it's time for the next pulse
                servoPin.Write(GpioPinValue.Low);
                
                Wait(teller2);
                teller3++;
                if (teller3==10)
                {
                    Debug.WriteLine("step up " + teller3);
                    teller3 = 0;
                    teller2++;
                }
            }



            // timer = ThreadPoolTimer.CreatePeriodicTimer(this.Tick, TimeSpan.FromSeconds(2));
        }
        private void Wait(double milliseconds)
        {
            long initialTick = stopwatch.ElapsedTicks;
            long initialElapsed = stopwatch.ElapsedMilliseconds;
            double desiredTicks = milliseconds / 1000.0 * Stopwatch.Frequency;
            double finalTick = initialTick + desiredTicks;
            while (stopwatch.ElapsedTicks < finalTick)
            {

            }
        }
    }
}
