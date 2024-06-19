using ccsa_rfid_client.Models;
using ccsa_rfid_client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static ccsa_rfid_client.extras.Toolwindowdll;

namespace ccsa_rfid_client.Views
{
    /// <summary>
    /// Interaction logic for logoutWindow.xaml
    /// </summary>
    public partial class logoutWindow : Window
    {
        private LogoutVM logoutVM;
        private System.Action CloseAction { get; set; }

        public logoutWindow(System.Action closeAction)
        {
            InitializeComponent();
            logoutVM = (LogoutVM)DataContext;

            CloseAction = closeAction;

            Show();
            Visibility = Visibility.Collapsed;
        }

        internal void AccountChangeHandler(Account? account)
        {
            if (account != null)
            {
                logoutVM.StdName = account.StdName;
                Visibility = Visibility.Visible;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = 0;

            SetToolWindowExemption(this);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PreventClosing)
                e.Cancel = true;
        }

        private void Window_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (logoutVM.StdName != String.Empty)
            {
                CloseAction();
            }
            this.Visibility = Visibility.Collapsed;
        }
    }
}
