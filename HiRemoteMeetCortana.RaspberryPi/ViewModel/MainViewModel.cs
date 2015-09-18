using Microsoft.AspNet.SignalR.Client;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Core;

namespace HiRemoteMeetCortana.RaspberryPiWin10.ViewModel
{
    [ImplementPropertyChanged]
    internal class MainViewModel
    {
        private readonly CoreDispatcher _dispatcher;
        private int dutycycle=0;
        private int totalticks;

        public MainViewModel()
        {
            
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            
            var connection = new HubConnection("http://hiremotemeetcortana.azurewebsites.net/signalR");

            var hubProxy = connection.CreateHubProxy("RaspberryPiHub");
            hubProxy.On<DateTime, bool, bool>("Action", Action);

            connection.Start();
        }

        public string Response { get; set; }

        private async void Action(DateTime tijd, bool daily, bool state)
        {
            var controller = GpioController.GetDefault();


            var activePin = controller.OpenPin(26);
            activePin.SetDriveMode(GpioPinDriveMode.Output);
            var stopwatch = Stopwatch.StartNew();
            int teller1 = 0;

            long totalticks = DateTime.Now.AddMinutes(30).Ticks - DateTime.Now.Ticks;
           
            bool on = false;
            while (true)
            {
                if (CheckTime(tijd))
                {
                    //tijd ligt in tijdspanne
                    if (on)
                    {

                        activePin.Write(GpioPinValue.Low);
                        if (teller1 == dutycycle)
                        {
                            on = false;
                        }
                    }
                    else
                    {
                        on = !on;
                    }

                }
                else
                {
                    activePin.Write(GpioPinValue.Low);
                }
                activePin.Write(GpioPinValue.High);


                }


            }
        private bool CheckTime(DateTime tijd)
        {
            var now = DateTime.Now;
            var start = tijd.AddMinutes(-30);

            if ((now > start) && (now < tijd))
            {
                dutycycle =(int) (DateTime.Now.Ticks - start.Ticks) / totalticks * 100;
                return true;
            }
            return false;
        }
    }
}
