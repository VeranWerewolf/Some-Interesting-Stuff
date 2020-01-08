using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace SomeInterestingStuff.WakeOnLan
{
    /// <summary>
    /// Interaction logic for WOLWindow.xaml
    /// </summary>
    public partial class WOLWindow : Window
    {
        List<TableHost> _hosts = new List<TableHost>();


        public WOLWindow()
        {
            InitializeComponent();
            // Получение ip-адреса.
            IPAddress[] allIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            // Показ адреса в label'е.
            StringBuilder ip = new StringBuilder();
            foreach (IPAddress i in allIP)
            {
                ip.Append(i.ToString()).Append(Environment.NewLine);
            }
            YourIPlabel.Content = ip.ToString();
        }

        /// <summary>
        /// Creating Host from right form. Returns null if validation failed.
        /// </summary>
        /// <returns></returns>
        private TableHost NewHost()
        {
            try
            {
                if (String.IsNullOrEmpty(PortTextBox.Text))
                {
                    return new TableHost(IPTextBox.Text, HostTextBox.Text, MacTextBox.Text);
                }
                else
                {
                    return new TableHost(IPTextBox.Text, int.Parse(PortTextBox.Text), HostTextBox.Text, MacTextBox.Text);
                }
            }
            catch (ArgumentException e)
            { MessageBox.Show(e.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error); return null; }
            catch { MessageBox.Show("Incorrect input!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error); return null; }
        }
        private byte[] MagicPacket(string preparedMAC)
        {
            //set sending bytes
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[1024];   // Magic packet is 102 bytes length, so 1024 is more than enough :-)  

            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeate MAC 16 times
            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                        byte.Parse(preparedMAC.Substring(i, 2),
                        System.Globalization.NumberStyles.HexNumber);
                    i += 2;
                }
            }
            //and return
            return bytes;
        }
        private void WakeFunction(TableHost HostToWake)
        {
            WOLClass client = new WOLClass();
            client.Connect(
                HostToWake.netipAdress,  //255.255.255.255 broadcast
                HostToWake.port); // port=9 is default

            //client.SetClientToBrodcastMode();
            
            //now send wake up packet
            int reterned_value = client.Send(MagicPacket(HostToWake.preparedMac), 1024);
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            //Here would be a check if PC is online
        }

        private void WakeButton_Click(object sender, RoutedEventArgs e)
        {
            TableHost HostToWake = NewHost();
            if (HostToWake == null)
            {
                return;
            }
            try
            {
                WakeFunction(HostToWake);
                MessageBox.Show("Magic packet send!", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch { MessageBox.Show("Magic packet sending error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            TableHost HostToAdd = NewHost();
            if (HostToAdd == null)
            {
                return;
            }
            //Далее, если всё успешно, добавляем все данные в список, после чего выводим всё в ListView
            Dispatcher.Invoke(new Action(() =>
            {
                _hosts.Add(HostToAdd);
                IPlistView.ItemsSource = null;
                IPlistView.ItemsSource = _hosts;
            }));
        }

        private void DeleteFromList_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                _hosts.Remove(_hosts[IPlistView.SelectedIndex]);
                IPlistView.ItemsSource = null;
                IPlistView.ItemsSource = _hosts;
            }));
        }

        private void ClearList_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                _hosts.Clear();
                IPlistView.ItemsSource = null;
                IPlistView.ItemsSource = _hosts;
            }));
        }

        private void PowerOn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WakeFunction(_hosts[IPlistView.SelectedIndex]);
                MessageBox.Show("Magic packet send!", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch { MessageBox.Show("Magic packet sending error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void SaveList_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
