using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiRemoteMeetCortana.RemoteCortana.models
{
    public class Settings
    {
        public bool Daily { get; set; }
        public DateTime TimeToWake { get; set; }
        public bool IsOn { get; set; }
    }
}
