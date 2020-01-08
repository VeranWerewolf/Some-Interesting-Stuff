using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly string ListFileName;
        private readonly string ListFilePath;

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

            ListFileName = "WOL-IP clients.json";
            ListFilePath = new StringBuilder().Append(System.IO.Path.GetDirectoryName(System.Reflection.Assembly
                .GetExecutingAssembly().Location))
                .Append("\\").Append(ListFileName).ToString();
            if (File.Exists(ListFilePath))
            {
                string json = File.ReadAllText(ListFilePath);
                _hosts = JsonConvert.DeserializeObject<List<TableHost>>(json);
                IPlistView.ItemsSource = null;
                IPlistView.ItemsSource = _hosts;
            }
            else
            {
                File.Create(ListFilePath).Close();
            }
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

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO Here would be a check if PC is online
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
                WOL.WakeFunction(HostToWake);
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
                WOL.WakeFunction(_hosts[IPlistView.SelectedIndex]);
                MessageBox.Show("Magic packet send!", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch { MessageBox.Show("Magic packet sending error!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        
        private void SaveList_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(ListFilePath, JsonConvert.SerializeObject(_hosts));
        }
    }
}
