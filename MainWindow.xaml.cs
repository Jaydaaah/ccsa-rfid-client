using ccsa_rfid_client.Action;
using ccsa_rfid_client.ViewModels;
using ccsa_rfid_client.Views;
using ccsa_rfid_client.Watchers;
using Notification.Wpf;
using Notification.Wpf.Classes;
using System.Windows;
using System.Windows.Input;
using static ccsa_rfid_client.extras.Toolwindowdll;

namespace ccsa_rfid_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainVM _viewModel;

        private logoutWindow logoutWindow;
        private NotificationManager notificationManager = new NotificationManager();

        private FileWatcher watcher = new FileWatcher();

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = (MainVM)DataContext;

            logoutWindow = new logoutWindow(() =>
            {
                var currentAccount = _viewModel.CurrentAccount;
                if (currentAccount != null)
                {
                    api_calls.CallLogout(currentAccount);
                }
                _viewModel.CurrentAccount = null;
            });

            rfidview.RfidVM.RfidSetEvent += (string rfid) =>
            {
                api_calls.SendRfid(rfid, (account) =>
                {
                    _viewModel.CurrentAccount = account;
                    rfidview.clearRfid();
                });
            };

            _viewModel.WindowVisibilityChanged += (isVisible) =>
            {
                Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            };

            _viewModel.AccountLoginChanged += (account) =>
            {
                if (account != null)
                {
                    var notifyImage = new NotificationImage
                    {
                        Position = ImagePosition.Top,
                        Source = account.Image
                    };
                    var content = new NotificationContent
                    {
                        Title = "Login Succesful",
                        Message = $"Name: {account.StdName}\n" +
                        $"CCSA ID: {account.CcsaId}\n" +
                        $"Course: {account.Course}\n",
                        Image = notifyImage
                    };
                    notificationManager.Show(content);
                }
            };

            _viewModel.AccountLoginChanged += logoutWindow.AccountChangeHandler;

            // File Watcher
            watcher.CreateFileAction = (fullname) =>
            {
                api_calls.CallAddFile(_viewModel.CurrentAccount, fullname);
            };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (PreventClosing)
                e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetToolWindowExemption(this);
        }

        private void Window_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                BringWindowToTop(this);
            }
        }
    }
}