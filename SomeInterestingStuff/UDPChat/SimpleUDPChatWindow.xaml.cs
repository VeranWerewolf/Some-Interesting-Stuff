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
    /// Interaction logic for SimpleUDPChatWindow.xaml
    /// </summary>
    public partial class SimpleUDPChatWindow : Window
    {
        private UDPChat chat;

        public SimpleUDPChatWindow()
        {
            this.Visibility = Visibility.Hidden;
            InitializeComponent();
            // Rise setting window
            ChatSettingsWindow settingsWindow = new ChatSettingsWindow();
            settingsWindow.SettingsProvided += c_SettingsLoaded;
            settingsWindow.Show();
        }

        private void c_SettingsLoaded(object sender, SettingsProvidedEventArgs e)
        {
            SetSettings(e);
            this.Visibility = Visibility.Visible;
            this.Show();
            MessageLog.LogSystemMessage("Welcome to UDP Chat!");
        }

        private void SetSettings(SettingsProvidedEventArgs e)
        {
            try
            {
                chat = new UDPChat(e.Address, e.RPort, e.LPort, e.Name);
                MessageLog.LogUpdated += c_LogUpdated;
            }
            catch (Exception ex) { MessageLog.LogSystemMessage("Error!" + ex.Message); }
        }


        private void c_LogUpdated(object sender, LogUpdatedEventArgs e)
        {
            UpdateChatBlock(e.NewMessage);
        }
        public void UpdateChatBlock(Message message)
        {
            ChatBlock.Text += new StringBuilder().Append(Environment.NewLine)
                .Append(Environment.NewLine).Append(Message.ToString(message)).ToString();
            if (Scroller.VerticalOffset == Scroller.ScrollableHeight)
            {
                Scroller.ScrollToEnd();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                chat.SendMessage(SendTextBox.Text);

                Dispatcher.Invoke(new Action(() =>
                {
                    SendTextBox.Text = "";
                }));
            }
            catch (Exception ex) { MessageLog.LogSystemMessage("Error!" + ex.Message); }
        }
    }
}
