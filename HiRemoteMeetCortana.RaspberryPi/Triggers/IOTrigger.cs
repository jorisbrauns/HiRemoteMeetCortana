using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiRemoteMeetCortana.RaspberryPiWin10.Triggers
{
    class IOTrigger : TriggerBase
    {
        public int IOPort { get; set; }

        public override bool evaluate()
        {
            IOAction action = IOAction.CreateInstance(0);
            return action.IsIOLow(IOPort);
        }
    }
}
