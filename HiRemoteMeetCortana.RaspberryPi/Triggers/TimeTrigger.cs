using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiRemoteMeetCortana.RaspberryPiWin10.Triggers
{
    public class TimeTrigger : TriggerBase
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public override bool evaluate()
        {
            throw new NotImplementedException();
        }
    }
}
