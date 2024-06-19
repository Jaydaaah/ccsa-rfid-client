using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ccsa_rfid_client.ViewModels
{
    public class rfidVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        private void selfINotifierPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            #region Rfid Tag update
            if (e.PropertyName == nameof(RfidTag))
            {
                if (RfidTag.Length == 10)
                {
                    RfidSetEvent?.Invoke(RfidTag);
                }
                timer.Stop();
                timer.Start();
                IsLoading = RfidTag.Length > 0;
            }
            #endregion
        }

        #region Events
        internal event Action<string>? RfidSetEvent;
        #endregion

        #region Timeout Handler
        private DispatcherTimer timer = new DispatcherTimer();

        // timeout in seconds
        const double timeout = 0.3;
        #endregion

        public rfidVM()
        {
            PropertyChanged += selfINotifierPropertyChanged;

            timer.Interval = TimeSpan.FromSeconds(timeout);
            timer.Tick += (object? sender, EventArgs e) =>
            {
                RfidTag = "";
                timer.Stop();
            };
        }

        private string _rfidTag = "";
        public string RfidTag
        {
            get => _rfidTag;
            set
            {
                _rfidTag = value;
                OnPropertyChanged();
            }
        }

        private bool _isloading = false;
        public bool IsLoading
        {
            get => _isloading;
            set
            {
                _isloading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LoadingVisibility));
                OnPropertyChanged(nameof(RFIDVisibility));
            }
        }

        public Visibility LoadingVisibility
        {
            get => IsLoading ? Visibility.Visible : Visibility.Collapsed;
        }

        public Visibility RFIDVisibility
        {
            get => IsLoading ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
