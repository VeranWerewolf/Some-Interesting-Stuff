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
                    MagicLabel.Text = BitConverter.ToString(MagicPacket(host.preparedMac));
                }));
            }
            catch (Exception ex){ MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        static string BytesToStringConverted(byte[] bytes)
        {
            using (var stream = new System.IO.MemoryStream(bytes))
            {
                using (var streamReader = new System.IO.StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        private byte[] MagicPacket(string preparedMAC)
        {
            //set sending bytes
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[102];   // Magic packet is 102 bytes length, so 102 is enough :-)  

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

    }
}
