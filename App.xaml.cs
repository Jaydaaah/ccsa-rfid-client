using ccsa_rfid_client.Action;
using System.Configuration;
using System.Data;
using System.Windows;
using static ccsa_rfid_client.extras.Toolwindowdll;

namespace ccsa_rfid_client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex? _mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string appName = "CCSARFIDClient";
            bool createdNew;

            // Attempt to create a named mutex
            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                // If mutex already existed, another instance is running
                PreventClosing = false;
                Current.Shutdown();
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                return;
            }

            api_calls.PingServer((isAlive) =>
            {
                if (!isAlive)
                {
                    var result = MessageBox.Show("Server is offline or can't connect, Check the client_config.json file inside Config directory if it is connecting to the server", "CCSA RFID lock closing", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    PreventClosing = false;
                    Current.Shutdown();
                } else
                {
                    PreventClosing = true;
                }
            });

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Release the mutex on application exit
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
            base.OnExit(e);
        }
    }

}
