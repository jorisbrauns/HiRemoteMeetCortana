using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiRemoteMeetCortana.RaspberryPiWin10
{
   public  class Rule
    {
        public Rule()
        {
            Priority = Priority.LOW;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<TriggerBase> Trigger { get; set; }
        public List<LoadOutputs> Output { get; set; }
        public Priority Priority { get; set; }

        //public bool VacationModeTrue { get; set; }

    }

    public enum Priority
    {
        HIGH,MEDIUM,LOW
    }

}
