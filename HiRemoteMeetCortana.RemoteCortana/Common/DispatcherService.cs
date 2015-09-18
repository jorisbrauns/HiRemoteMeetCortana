using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HiRemoteMeetCortana.RemoteCortana.Common
{
    public class DispatcherService 
    {
        public CoreDispatcher Dispatcher { get; private set; }

        public DispatcherService(CoreDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        public async void SafeAction(Action action)
        {
            if (Dispatcher.HasThreadAccess)
            {
                action();
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            }
        }
    }
}
