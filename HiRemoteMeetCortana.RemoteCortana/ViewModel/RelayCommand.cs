using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HiRemoteMeetCortana.RemoteCortana.ViewModel
{
    public class RelayCommand : ICommand
    {
        public string Display { get; set; }

        private Action execute;
        private Func<bool> canExecute;

        public RelayCommand(string display, Action execute, Func<bool> canExecute)
        {
            this.Display = display;
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(string display, Action execute)
          : this(display, execute, () => true)
        { }

        public void Execute(object parameter) => execute();

        public bool CanExecute(object parameter) => canExecute();

        public event EventHandler CanExecuteChanged;

        public void Raise()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
