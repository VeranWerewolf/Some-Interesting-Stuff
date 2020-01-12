using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SomeInterestingStuff.UDPChat
{
    public class UDPChat
    {
        private static string remoteAddress; // хост для отправки данных
        private static int remotePort; // порт для отправки данных
        private static int localPort; // локальный порт для прослушивания входящих подключений
        private static string senderName; //имя отправителя
        private Thread receiveThread;

        public UDPChat(string remoteAddress, int remotePort, int localPort, string senderName)
        {

            UDPChat.localPort = localPort; // локальный порт для прослушивания
            UDPChat.remoteAddress = remoteAddress; // адрес, к которому мы подключаемся
            UDPChat.remotePort = remotePort; // порт, к которому мы подключаемся
            UDPChat.senderName = senderName; //имя отправителя
            //Log = new MessageLog();

            receiveThread = new Thread(ReceiveMessage);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        public void SendMessage(string newMessage)
        {
            Message message = new Message(senderName, newMessage);
            try
            {
                SendMessage(message);
            }
            catch (Exception)
            {
                throw new Exception("Error sending message");
            }
        }

        private void SendMessage(Message message)
        {
            // создаем UdpClient для отправки сообщений
            using (UdpClient sender = new UdpClient())
            {
                try
                {
                    //Message sending
                    MessageLog.LogMessage(true, message);
                    byte[] data = Message.ToBytes(message);
                    sender.Send(data, data.Length, remoteAddress, remotePort); // отправка

                }
                catch (Exception)
                {
                    throw new Exception("Error sending message");
                }
                finally
                {
                    sender.Dispose();
                }
            }
        }

        private static void ReceiveMessage()
        {
            // UdpClient для получения данных
            using (UdpClient receiver = new UdpClient(localPort))
            {
                IPEndPoint remoteIp = null; // адрес входящего подключения
                try
                {
                    while (true)
                    {
                        byte[] data = receiver.Receive(ref remoteIp); // получаем данные

                        //Resolve Message here
                        Message message = Message.ToMessage(data);
                        System.Windows.Application.Current.Dispatcher.BeginInvoke(
                            new ThreadStart(delegate { MessageLog.LogMessage(false, message); }));
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Error receiving message");
                }
                finally
                {
                    receiver.Dispose();
                }
            }
        }

        public void CloseReceiveThread()
        {
            receiveThread.Abort();
        }
    }
}
