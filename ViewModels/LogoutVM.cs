using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ccsa_rfid_client.ViewModels
{
    internal class LogoutVM : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        private void selfINotifierPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        public LogoutVM()
        {
            PropertyChanged += selfINotifierPropertyChanged;
        }

        private string _stdName = "";
        public string StdName
        {
            get => _stdName;
            set
            {
                _stdName = value;
                OnPropertyChanged();
            }
        }
    }
}
