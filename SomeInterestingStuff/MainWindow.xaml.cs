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

namespace SomeInterestingStuff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void KeyCodesButton_Click(object sender, RoutedEventArgs e)
        {
            KeyCodes.KeyCodesWindow keyWindow = new KeyCodes.KeyCodesWindow();
            keyWindow.Show();
        }

        private void RPNButton_Click(object sender, RoutedEventArgs e)
        {
            ReversePolishNotation.RPNWindow rpnWindow = new ReversePolishNotation.RPNWindow();
            rpnWindow.Show();
        }

        private void WolButton_Click(object sender, RoutedEventArgs e)
        {
            WakeOnLan.WOLWindow wolWindow = new WakeOnLan.WOLWindow();
            wolWindow.Show();
        }
        private void MPButton_Click(object sender, RoutedEventArgs e)
        {
            WakeOnLan.MagicPacketWindow mpWindow = new WakeOnLan.MagicPacketWindow();
            mpWindow.Show();
        }
        
    }
}