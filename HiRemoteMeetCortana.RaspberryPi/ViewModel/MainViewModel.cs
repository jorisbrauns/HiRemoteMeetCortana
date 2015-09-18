using Microsoft.AspNet.SignalR.Client;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HiRemoteMeetCortana.RaspberryPiWin10.ViewModel
{
    [ImplementPropertyChanged]
    internal class MainViewModel
    {
        private readonly CoreDispatcher _dispatcher;

        public MainViewModel()
        {
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            var connection = new HubConnection("http://hiremotemeetcortana.azurewebsites.net/signalR");

            var hubProxy = connection.CreateHubProxy("RaspberryPiHub");
            hubProxy.On<int, bool>("Action", Action);

            connection.Start();
        }

        public string Response { get; set; }

        private async void Action(int port, bool value)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                Response = string.Format("Port: {0}, Value: {1}", port, value);
                //var gpioController = IOAction.CreateInstance(port);
                //gpioController.WriteIO(port, value);
            });
        }
    }
}
