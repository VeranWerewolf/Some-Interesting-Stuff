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
using System.Windows.Forms;


namespace SomeInterestingStuff.KeyCodes
{
    /// <summary>
    /// Interaction logic for KeyCodesWindow.xaml
    /// </summary>
    public partial class KeyCodesWindow : Window
    {
        public KeyCodesWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void KeyCodeWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Keys formKey = (Keys)KeyInterop.VirtualKeyFromKey(e.Key);
            ASCII_text.Content = e.Key.ToString(); //e.KeyCode.ToString();
            Decimal_text.Content = Convert.ToInt16(e.Key).ToString(); //e.KeyValue.ToString();
            HEX_text.Content = "0x" + Convert.ToByte(e.Key).ToString("x2");//"0x" + e.KeyValue.ToString("X2");
            Modifier_text.Content = e.SystemKey.ToString(); // //e.Modifiers.ToString();
            ModifiernKey_text.Content = e.KeyboardDevice.Modifiers.ToString();//e.KeyData.ToString();

        }
    }
}
