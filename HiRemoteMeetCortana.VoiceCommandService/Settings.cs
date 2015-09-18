using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiRemoteMeetCortana.VoiceCommands
{
    public sealed class Settings
    {
        public bool Daily { get; set; }
        public DateTimeOffset TimeToWake { get; set; }
        public bool IsOn { get; set; }
    }
}
