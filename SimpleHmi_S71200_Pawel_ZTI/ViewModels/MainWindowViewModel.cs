using GalaSoft.MvvmLight;
using Prism.Commands;
using Prism.Mvvm;
using SimpleHmi_S71200_Pawel_ZTI.PlcService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleHmi_S71200_Pawel_ZTI.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public string IpAddress
        {
            get { return _ipAddress; }
            set { SetProperty(ref _ipAddress, value); }
        }
        private string _ipAddress;

        //Safety_ok
        public bool Safety_ok
        {
            get { return _Safety_ok; }
            set { SetProperty(ref _Safety_ok, value); }
        }
        private bool _Safety_ok;


        public bool Pump1_Contactor
        {
            get { return _pump1_Contactor; }
            set { SetProperty(ref _pump1_Contactor, value); }
        }
        private bool _pump1_Contactor;

        public bool High_level_Limit_1
        {
            get { return _high_level_Limit_1; }
            set { SetProperty(ref _high_level_Limit_1, value); }
        }
        private bool _high_level_Limit_1;

        public bool Low_Level_Limit_1
        {
            get { return _low_Level_Limit_1; }
            set { SetProperty(ref _low_Level_Limit_1, value); }
        }
        private bool _low_Level_Limit_1;

        public int Tank1_Level
        {
            get { return _tank1Level; }
            set { SetProperty(ref _tank1Level, value); }
        }
        private int _tank1Level;
        
        public int Set_Tank1_Level
        {
            get { return _set_Tank1_Level; }
            set { SetProperty(ref _set_Tank1_Level, value); }
        }
        private int _set_Tank1_Level;

        public bool Pump2_Contactor
        {
            get { return _pump2_Contactor; }
            set { SetProperty(ref _pump2_Contactor, value); }
        }
        private bool _pump2_Contactor;

        public bool High_level_Limit_2
        {
            get { return _high_level_Limit_2; }
            set { SetProperty(ref _high_level_Limit_2, value); }
        }
        private bool _high_level_Limit_2;

        public bool Low_Level_Limit_2
        {
            get { return _low_Level_Limit_2; }
            set { SetProperty(ref _low_Level_Limit_2, value); }
        }
        private bool _low_Level_Limit_2;

        public int Tank2_Level
        {
            get { return _tank2Level; }
            set { SetProperty(ref _tank2Level, value); }
        }
        private int _tank2Level;

        public int Set_Tank2_Level
        {
            get { return _set_Tank2_Level; }
            set { SetProperty(ref _set_Tank2_Level, value); }
        }
        private int _set_Tank2_Level;

        public ConnectionStates ConnectionState
        {
            get { return _connectionState; }
            set { SetProperty(ref _connectionState, value); }
        }
        private ConnectionStates _connectionState;

        public TimeSpan ScanTime
        {
            get { return _scanTime; }
            set { SetProperty(ref _scanTime, value); }
        }
        private TimeSpan _scanTime;

        public ICommand ConnectCommand { get; private set; }

        public ICommand DisconnectCommand { get; private set; }

        public ICommand EmergencyCommand { get; private set; }

        public ICommand ResetCommand { get; private set; }

        public ICommand StartCommand { get; private set; }

        public ICommand StartPump1Command { get; private set; }

        public ICommand StopPump1Command { get; private set; }

        public ICommand StartPump2Command { get; private set; }

        public ICommand StopPump2Command { get; private set; }

        S7PlcService _plcService;

        public MainWindowViewModel()
        {
            _plcService = new S7PlcService();
            ConnectCommand = new DelegateCommand(Connect);
            DisconnectCommand = new DelegateCommand(Disconnect);
            EmergencyCommand = new DelegateCommand(async () => { await Emergency(); });
            ResetCommand = new DelegateCommand(async () => { await Reset(); });
            StartCommand = new DelegateCommand(async () => { await Start(); });
            StartPump1Command = new DelegateCommand(async () => { await StartPump1(); });
            StopPump1Command = new DelegateCommand(async () => { await StopPump1(); });
            StartPump2Command = new DelegateCommand(async () => { await StartPump2(); });
            StopPump2Command = new DelegateCommand(async () => { await StopPump2(); });

            IpAddress = "192.168.0.1";

            OnPlcServiceValuesRefreshed(null, null);
            _plcService.ValuesRefreshed += OnPlcServiceValuesRefreshed;
        }

        private void OnPlcServiceValuesRefreshed(object sender, EventArgs e)
        {
            Safety_ok = _plcService.Safety_ok;
            ConnectionState = _plcService.ConnectionState;
            Pump1_Contactor = _plcService.Pump1_Contactor;
            High_level_Limit_1 = _plcService.High_level_Limit_1;
            Low_Level_Limit_1 = _plcService.Low_Level_Limit_1;
            Tank1_Level = _plcService.Tank1_Level;
            Set_Tank1_Level = _plcService.Set_Tank1_Level;
            Pump2_Contactor = _plcService.Pump2_Contactor;
            High_level_Limit_2 = _plcService.High_level_Limit_2;
            Low_Level_Limit_2 = _plcService.Low_Level_Limit_2;
            Tank2_Level = _plcService.Tank2_Level;
            Set_Tank2_Level = _plcService.Set_Tank2_Level;
            ScanTime = _plcService.ScanTime;
        }

        //Buttons function
        private void Connect()
        {
            _plcService.Connect(IpAddress, 0, 1);
        }

        private void Disconnect()
        {
            _plcService.Disconnect();
        }

        private async Task Emergency()
        {
            await _plcService.WriteEmergency();
        }

        private async Task Reset()
        {
            await _plcService.WriteReset();
        }

        private async Task Start()
        {
            await _plcService.WriteStart();
        }

        private async Task StartPump1()
        {
            await _plcService.WriteStartPump1();
        }

        private async Task StopPump1()
        {
            await _plcService.WriteStopPump1();
        }

        private async Task StartPump2()
        {
            await _plcService.WriteStartPump2();
        }

        private async Task StopPump2()
        {
            await _plcService.WriteStopPump2();
        }
    }
}
