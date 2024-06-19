using ccsa_rfid_client.Action;
using ccsa_rfid_client.Models;
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

    public class MainVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        private void selfINotifierPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            #region Rfid Tag update
            if (e.PropertyName == nameof(CurrentAccount))
            {
                AccountLoginChanged?.Invoke(CurrentAccount);
            }
            else if (e.PropertyName == nameof(MyWindowVisible))
            {
                WindowVisibilityChanged?.Invoke(MyWindowVisible);
            }
            #endregion
        }

        #region Events
        internal delegate void AccountLoginChangedHandler(Account? account);
        internal event AccountLoginChangedHandler? AccountLoginChanged;

        internal event Action<bool>? WindowVisibilityChanged;
        #endregion

        public MainVM()
        {
            PropertyChanged += selfINotifierPropertyChanged;
            AccountLoginChanged += (Account? acc) =>
            {
                var isVisible = acc == null;
                MyWindowVisible = isVisible;
            };
        }

        private bool _windowVisible = true;
        public bool MyWindowVisible
        {
            get => _windowVisible;
            private set
            {
                _windowVisible = value;
                OnPropertyChanged();
            }
        }

        private Account? account;
        public Account? CurrentAccount
        {
            get => account;
            set
            {
                account = value;
                OnPropertyChanged();
            }
        }
    }
}
