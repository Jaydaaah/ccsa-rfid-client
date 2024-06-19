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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ccsa_rfid_client.Views
{
    /// <summary>
    /// Interaction logic for rfidView.xaml
    /// </summary>
    public partial class rfidView : UserControl
    {
        private rfidVM rfidVM;
        internal rfidVM RfidVM { get => rfidVM; }

        public rfidView()
        {
            InitializeComponent();
            rfidVM = (rfidVM)DataContext;

            Loaded += (object? sender, RoutedEventArgs e) =>
            {
                Focus();
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.PreviewTextInput += UserControl_PreviewTextInput;
                }
            };
        }

        private void UserControl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                var num = int.Parse(e.Text);
                rfidVM.RfidTag += num.ToString();
            }
            catch
            {
                rfidVM.RfidTag = "";
            }
        }

        internal void clearRfid() => rfidVM.RfidTag = "";
    }
}
