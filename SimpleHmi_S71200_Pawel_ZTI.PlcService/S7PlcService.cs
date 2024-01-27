using Sharp7;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace SimpleHmi_S71200_Pawel_ZTI.PlcService
{
    public class S7PlcService
    {
        private readonly S7Client _client;
        private readonly System.Timers.Timer _timer;
        private DateTime _lastScanTime;

        private volatile object _locker = new object();

        public S7PlcService()
        {
            _client = new S7Client();
            _timer = new System.Timers.Timer();
            _timer.Interval = 100;
            _timer.Elapsed += OnTimerElapsed;
        }

        public ConnectionStates ConnectionState { get; private set; }

        public bool Safety_ok { get; private set; }
        public bool Pump1_Contactor { get; private set; }
        public bool High_level_Limit_1 { get; private set; }
        public bool Low_Level_Limit_1 { get; private set; }
        public int Tank1_Level { get; private set; }
        public int Set_Tank1_Level { get; private set; }
        public bool Pump2_Contactor { get; private set; }
        public bool High_level_Limit_2 { get; private set; }
        public bool Low_Level_Limit_2 { get; private set; }
        public int Tank2_Level { get; private set; }
        public int Set_Tank2_Level { get; private set; }
        public TimeSpan ScanTime { get; private set; }

        public event EventHandler ValuesRefreshed;

        public void Connect(string ipAddress, int rack, int slot)
        {
            try
            {
                ConnectionState = ConnectionStates.Connecting;
                int result = _client.ConnectTo(ipAddress, rack, slot);
                if (result == 0)
                {
                    ConnectionState = ConnectionStates.Online;
                    _timer.Start();
                }
                else
                {
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Connection error: " + _client.ErrorText(result));
                    ConnectionState = ConnectionStates.Offline;
                }
                OnValuesRefreshed();
            }
            catch
            {
                ConnectionState = ConnectionStates.Offline;
                OnValuesRefreshed();
                throw;
            }
        }

        public void Disconnect()
        {
            if (_client.Connected)
            {
                _timer.Stop();
                _client.Disconnect();
                ConnectionState = ConnectionStates.Offline;
                OnValuesRefreshed();
            }
        }

        // Button Emergency
        public async Task WriteEmergency()
        {
            await Task.Run(() =>
            {
                if (_client.Connected)
                {
                    int writeResult = WriteBit("DB7.DBX0.0", true);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                    Thread.Sleep(30);
                    writeResult = WriteBit("DB7.DBX0.0", false);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                }
            });
        }

        // Button Reset Errors
        public async Task WriteReset()
        {
            await Task.Run(() =>
            {
                if (_client.Connected)
                {
                    int writeResult = WriteBit("DB7.DBX0.1", true);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                    Thread.Sleep(30);
                    writeResult = WriteBit("DB7.DBX0.1", false);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                }
            });
        }

        // Button Start Power Supply Relay 24V DC
        public async Task WriteStart()
        {
            await Task.Run(() =>
            {
                if (_client.Connected)
                {
                    int writeResult = WriteBit("DB7.DBX0.2", true);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                    Thread.Sleep(30);
                    writeResult = WriteBit("DB7.DBX0.2", false);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                }
            });
        }

        // Button Start Pump1
        public async Task WriteStartPump1()
        {
            await Task.Run(() =>
            {
                if (_client.Connected)
                {
                    int writeResult = WriteBit("DB7.DBX0.3", true);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                    Thread.Sleep(30);
                    writeResult = WriteBit("DB7.DBX0.3", false);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                }
            });
        }

        // Button Stop Pump1
        public async Task WriteStopPump1()
        {
            await Task.Run(() =>
            {
                if (_client.Connected)
                {
                    int writeResult = WriteBit("DB7.DBX0.4", true);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                    Thread.Sleep(30);
                    writeResult = WriteBit("DB7.DBX0.4", false);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                }
            });
        }

        // Button Start Pump2
        public async Task WriteStartPump2()
        {
            await Task.Run(() =>
            {
                if (_client.Connected)
                {
                    int writeResult = WriteBit("DB7.DBX0.5", true);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                    Thread.Sleep(30);
                    writeResult = WriteBit("DB7.DBX0.5", false);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                }
            });
        }

        // Button Stop Pump2
        public async Task WriteStopPump2()
        {
            await Task.Run(() =>
            {
                if (_client.Connected)
                {
                    int writeResult = WriteBit("DB7.DBX0.6", true);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                    Thread.Sleep(30);
                    writeResult = WriteBit("DB7.DBX0.6", false);
                    if (writeResult != 0)
                    {
                        Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Write error: " + _client.ErrorText(writeResult));
                    }
                }
            });
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                ScanTime = DateTime.Now - _lastScanTime;
                RefreshValues();
                OnValuesRefreshed();
            }
            finally
            {
                _timer.Start();
            }
            _lastScanTime = DateTime.Now;
        }

        private void RefreshValues()
        {
            lock (_locker)
            {
                var buffer = new byte[10];
                int result = _client.DBRead(7, 0, buffer.Length, buffer);
                if (result == 0)
                {
                    Pump1_Contactor = S7.GetBitAt(buffer, 1, 0);
                    Pump2_Contactor = S7.GetBitAt(buffer, 1, 1);
                    High_level_Limit_1 = S7.GetBitAt(buffer, 1, 2);
                    Low_Level_Limit_1 = S7.GetBitAt(buffer, 1, 3);
                    High_level_Limit_2 = S7.GetBitAt(buffer, 1, 4);
                    Low_Level_Limit_2 = S7.GetBitAt(buffer, 1, 5);
                    Safety_ok = S7.GetBitAt(buffer, 1, 6);
                    Tank1_Level = S7.GetIntAt(buffer, 2);  
                    Tank2_Level = S7.GetIntAt(buffer, 4);
                    //S7.SetDIntAt(buffer, 6, Set_Tank1_Level);      //TODO SET
                    //S7.SetDIntAt(buffer, 10, Set_Tank2_Level);     //TODO SET
                }
                else
                {
                    Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t Read error: " + _client.ErrorText(result));
                }
            }
        }

        /// <summary>
        /// Writes a bit at the specified address. Es.: DB1.DBX10.2 writes the bit in db 1, word 10, 3rd bit
        /// </summary>
        /// <param name="address">Es.: DB1.DBX10.2 writes the bit in db 1, word 10, 3rd bit</param>
        /// <param name="value">true or false</param>
        /// <returns></returns>
        private int WriteBit(string address, bool value)
        {
            var strings = address.Split('.');
            int db = Convert.ToInt32(strings[0].Replace("DB", ""));
            int pos = Convert.ToInt32(strings[1].Replace("DBX", ""));
            int bit = Convert.ToInt32(strings[2]);
            return WriteBit(db, pos, bit, value);
        }

        private int WriteBit(int db, int pos, int bit, bool value)
        {
            lock (_locker)
            {
                var buffer = new byte[1];
                S7.SetBitAt(ref buffer, 0, bit, value);
                return _client.WriteArea(S7Consts.S7AreaDB, db, pos + bit, buffer.Length, S7Consts.S7WLBit, buffer);
            }
        }

        private void OnValuesRefreshed()
        {
            ValuesRefreshed?.Invoke(this, new EventArgs());
        }


    }
}
