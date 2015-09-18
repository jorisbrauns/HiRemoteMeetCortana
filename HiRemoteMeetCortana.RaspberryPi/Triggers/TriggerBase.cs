using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiRemoteMeetCortana.RaspberryPiWin10
{
    public abstract class TriggerBase
    {
        public int GPIOPort { get; set; }
        public double Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public abstract bool evaluate();
    }
}
