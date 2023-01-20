using ChileafBleXamarin.Interfaces;
using ChileafDemo.Chileaf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ChileafDemo.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        private int _hr;
        public int HeartRate
        {
            get => _hr;
            set
            {
                _hr = value;
                OnPropertyChanged();
            }
        }

        private List<int> _rrIntervals = new List<int>();
        public List<int> RRIntervals
        {
            get => _rrIntervals;
            set
            {
                _rrIntervals = value;
                OnPropertyChanged(nameof(RRIntervalsString));
            }
        }

        public string RRIntervalsString => String.Join(", ", _rrIntervals);

        private int _batteryLevel;
        public int BatteryLevel
        {
            get => _batteryLevel;
            set
            {
                _batteryLevel = value;
                OnPropertyChanged();
            }
        }

        private int _energy;
        public int EnergyExpanded
        {
            get => _energy;
            set
            {
                _energy = value;
                OnPropertyChanged();
            }
        }

        private bool? _deviceContact = false;
        public bool? ContactDetected
        {
            get => _deviceContact;
            set
            {
                _deviceContact = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ContactSupport));
            }
        }

        public bool ContactSupport
        {
            get => _deviceContact != null;
        }

        private int _step;
        public int Step
        {
            get => _step;
            set
            {
                _step = value;
                OnPropertyChanged();
            }
        }

        private int _distance;
        public int Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnPropertyChanged();
            }
        }

        private int _calorie;
        public int Calorie
        {
            get => _calorie;
            set
            {
                _calorie = value;
                OnPropertyChanged();
            }
        }

        private int _rssi;
        public int Rssi
        {
            get => _rssi;
            set
            {
                _rssi = value;
                OnPropertyChanged();
            }
        }

        private string _systemId;
        public string SystemId
        {
            get => _systemId;
            set
            {
                _systemId = value;
                OnPropertyChanged();
            }
        }

        private string _modelName;
        public string ModelName
        {
            get => _modelName;
            set
            {
                _modelName = value;
                OnPropertyChanged();
            }
        }

        private string _serialNumber;
        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                _serialNumber = value;
                OnPropertyChanged();
            }
        }

        private string _firmwareVersion;
        public string FirmwareVersion
        {
            get => _firmwareVersion;
            set
            {
                _firmwareVersion = value;
                OnPropertyChanged();
            }
        }

        private string _hardwareVersion;
        public string HardwareVersion
        {
            get => _hardwareVersion;
            set
            {
                _hardwareVersion = value;
                OnPropertyChanged();
            }
        }

        private string _softwareVersion;
        public string SoftwareVersion
        {
            get => _softwareVersion;
            set
            {
                _softwareVersion = value;
                OnPropertyChanged();
            }
        }

        private string _vendorName;
        public string VendorName
        {
            get => _vendorName;
            set
            {
                _vendorName = value;
                OnPropertyChanged();
            }
        }

        private string _sensorLocation;
        public string SensorLocation
        {
            get => _sensorLocation;
            set
            {
                _sensorLocation = value;
                OnPropertyChanged();
            }
        }

        private bool _deviceConnected;
        public bool DeviceConnected
        {
            get => _deviceConnected;
            set
            {
                _deviceConnected = value;
                OnPropertyChanged();
            }
        }

        private bool _isSearching;
        public bool IsSearching
        {
            get => _isSearching;
            set
            {
                _isSearching = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<IBTDevice> _btDevices;
        public ObservableCollection<IBTDevice> BTDevices
        {
            get => _btDevices;
            set
            {
                _btDevices = value;
                OnPropertyChanged();
            }
        }

        private IBTDevice _currentDevice;
        public IBTDevice CurrentDevice
        {
            get => _currentDevice;
            set
            {
                _currentDevice = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand ConnectDisconnectCommand { get; set; }

        private IApiProvider _apiProvider;
        private ISensorHelper _sensorHelper;
        private static TimeSpan _searchTime = TimeSpan.FromSeconds(5);
        private ApiCallbackImpl _apiCallback;

        public MainPageViewModel()
        {
            _apiProvider = DependencyService.Get<IApiProvider>();
            _sensorHelper = DependencyService.Get<ISensorHelper>();
            _apiCallback = new ApiCallbackImpl();

            SearchCommand = new Command(SearchDevice);
            ConnectDisconnectCommand = new Command(ConnectDisconnectDevice);

            _apiProvider.SetApiCallback(_apiCallback);
            SetCallbacks();

            _apiProvider.SetDebug(true);
        }

        private void SearchDevice()
        {
            BTDevices = null;
            IsSearching = true;
            _sensorHelper.EnableSensor((enable) =>
            {
                _apiProvider.StartSearch(OnDeviceFounded, _searchTime);
            });
        }

        private void ConnectDisconnectDevice()
        {
            _apiProvider.Disconnect();
            if (_deviceConnected)
            {
                Name = null;
                Address = null;
                Rssi = 0;
                SystemId = null;
                VendorName = null;
                ModelName = null;
                SerialNumber = null;
                FirmwareVersion = null;
                SoftwareVersion = null;
                HardwareVersion = null;
                SensorLocation = null;
                CurrentDevice = null;
                EnergyExpanded = 0;
                ContactDetected = false;
                return;
            }
            _sensorHelper.EnableSensor(async (enable) =>
            {
                if (enable && _currentDevice != null)
                {
                    Name = _currentDevice.Name;
                    Address = _currentDevice.Address;
                    _apiProvider.Connect(_currentDevice);
                    var info = await _apiProvider.GetDeviceInfoAsync();
                    Rssi = info.Rssi;
                    SystemId = info.SystemId;
                    VendorName = info.VendorName;
                    ModelName = info.ModelName;
                    SerialNumber = info.SerialNumber;
                    FirmwareVersion = info.FirmwareVersion;
                    SoftwareVersion = info.SoftwareVersion;
                    HardwareVersion = info.HardwareVersion;
                    SensorLocation = info.SensorLocation.ToString();
                }
            });
        }

        private void OnDeviceFounded(List<IBTDevice> devices)
        {
            IsSearching = false;
            BTDevices = new ObservableCollection<IBTDevice>(devices);
        }

        private void SetCallbacks()
        {
            _apiCallback.ConnectedAction = (state) =>
            {
                DeviceConnected = state;
            };
            _apiCallback.BatteryAction = (level) =>
            {
                BatteryLevel = level;
            };
            _apiCallback.HeartRateDataAction = (data) =>
            {
                HeartRate = data.HeartRate;
                RRIntervals = data.RrIntervals;
                EnergyExpanded = data.EnergyExpanded ?? -1;
                ContactDetected = data.ContactDetected;
            };
            _apiCallback.SportDataAction = (data) =>
            {
                Step = data.Step;
                Distance = data.Distance;
                Calorie = data.Calorie;
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
