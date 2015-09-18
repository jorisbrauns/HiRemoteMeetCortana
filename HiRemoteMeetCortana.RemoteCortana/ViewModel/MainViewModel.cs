using HiRemoteMeetCortana.RemoteCortana.Common;
using HiRemoteMeetCortana.RemoteCortana.models;
using HiRemoteMeetCortana.Services;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace HiRemoteMeetCortana.RemoteCortana.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _lightUpDaily;
        public bool LightUpDaily
        {
            get
            {
                return _lightUpDaily;
            }
            set
            {
                _lightUpDaily = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _timeToWake;
        public TimeSpan TimeToWake
        {
            get
            {
                return _timeToWake;
            }
            set
            {
                _timeToWake = value;
                OnPropertyChanged();
            }
        }

        private bool _disableAlarm;
        public bool DisableAlarm
        {
            get
            {
                return _disableAlarm;
            }
            set
            {
                _disableAlarm = value;
                OnPropertyChanged();
            }
        }

        protected override void InitCommands()
        {
            SaveCommand = CreateLightweightCommand("Save", SaveCommandAction);
        }

        public ICommand SaveCommand { get; set; }

        private async void SaveCommandAction()
        {
            DateTime timetowake = new DateTime(2012, 01, 01) + TimeToWake;
            
            await _wakupLightService.Save(new Settings
            {
                Daily =  LightUpDaily,
                IsOn = DisableAlarm,
                TimeToWake = timetowake
            });

            MessageDialog md = new MessageDialog("Change has been saved!", "Light Alarm");
            md.Commands.Add(new UICommand("OK"));
            await md.ShowAsync();

        }

        public DispatcherService _dispatcher;
        private WakupLightService _wakupLightService;

        public MainViewModel()
        {
            _wakupLightService = new WakupLightService();
            InitData();
        }

        private async void InitData()
        {
            var settings = await _wakupLightService.get();

            var dispatcher = App.Disptacher;
            _dispatcher = new DispatcherService(dispatcher);
            _dispatcher.SafeAction(() =>
            {
                DateTime dt = settings.TimeToWake;
                LightUpDaily = settings.Daily;
                DisableAlarm = settings.IsOn;
                TimeToWake = dt - dt.Date;
            });

        }
    }
}
