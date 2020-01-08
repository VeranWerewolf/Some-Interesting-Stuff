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

namespace SomeInterestingStuff.WakeOnLan
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MagicPacketWindow : Window
    {
        public MagicPacketWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MPButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TableHost host = new TableHost("1.1.1.1", 9, "NaN", MacTextBox.Text);
                Dispatcher.Invoke(new Action(() =>
                {
                    MagicLabel.Text = BitConverter.ToString(WOL.MagicPacket(host.preparedMac));
                }));
            }
            catch (Exception ex){ MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
