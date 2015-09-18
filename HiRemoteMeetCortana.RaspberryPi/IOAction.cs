using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace HiRemoteMeetCortana.RaspberryPiWin10
{
  public  class IOAction
    {

        private GpioController _controller;
        GpioPin pin = null;
        private static IOAction _instance;

        private IOAction(int port)
        {
            _controller = GpioController.GetDefault();
            pin = _controller.OpenPin(port);
            pin.SetDriveMode(GpioPinDriveMode.Output);
        }

        public static IOAction CreateInstance(int port)
        {
            if (_instance ==null)
            {
                _instance = new IOAction(port);
            }
            return _instance;
        }


        public bool IsIOLow(int port)
        {
            //var pin = _controller.OpenPin(port);
            var value = pin.Read();

            return value== GpioPinValue.Low;

        }

        public void WriteIO(int port,bool value)
        {
            //
            if (value)
            {
                pin.Write(GpioPinValue.High);
            }
            else
            {
                pin.Write(GpioPinValue.Low);
            }
        }

    }
}
