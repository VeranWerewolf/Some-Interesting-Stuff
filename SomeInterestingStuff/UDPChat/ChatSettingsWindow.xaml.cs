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
using System.Windows.Shapes;

namespace SomeInterestingStuff.UDPChat
{
    /// <summary>
    /// Interaction logic for ChatSettingsWindow.xaml
    /// </summary>
    public partial class ChatSettingsWindow : Window
    {
        public event EventHandler<SettingsProvidedEventArgs> SettingsProvided;

        public ChatSettingsWindow()
        {
            InitializeComponent();
            IPTextBox.Text = "127.0.0.1";
            RPortTextBox.Text = "2755";
            LPortTextBox.Text = "2755";
            NameTextBox.Text = "Nickname here";
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void ProvideSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!InputChecker.LanSettingsChecker.IsValidIP(IPTextBox.Text))
            {
                MessageBox.Show("IP is not valid.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!InputChecker.LanSettingsChecker.IsValidPort(int.Parse(RPortTextBox.Text)))
            {
                MessageBox.Show("Remote Port is not valid.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!InputChecker.LanSettingsChecker.IsValidPort(int.Parse(LPortTextBox.Text)))
            {
                MessageBox.Show("Local Port is not valid.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.RiseSettingsProvidedEvent(IPTextBox.Text, int.Parse(RPortTextBox.Text), int.Parse(LPortTextBox.Text), NameTextBox.Text);
                this.Close();
            }
        }

        private void RiseSettingsProvidedEvent(string remoteAddress, int remotePort, int localPort, string name)
        {
            SettingsProvided?.Invoke(this, new SettingsProvidedEventArgs
            {
                Address = remoteAddress,
                RPort = remotePort,
                LPort = localPort,
                Name = name
            });
        }
    }

    public class SettingsProvidedEventArgs : EventArgs
    {
        public string Address { get; set; } // хост для отправки данных
        public int RPort { get; set; } // порт для отправки данных
        public int LPort { get; set; } // локальный порт для прослушивания входящих подключений 
        public string Name { get; set; } //nickname
    }
    
}
