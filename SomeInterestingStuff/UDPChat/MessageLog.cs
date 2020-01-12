using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeInterestingStuff.UDPChat
{
    public static class MessageLog
    {
        private static List<Message> messageLog = new List<Message>();
        public static event EventHandler<LogUpdatedEventArgs> LogUpdated;

        /// <summary>
        /// Adding message to log. Set outgoing bool =true if this message is sending and =false if it is incoming.
        /// </summary>
        /// <param name="outgoing"></param>
        /// <param name="message"></param>
        public static void LogMessage(bool outgoing, Message message)
        {
            message.IsOutgoing = outgoing;
            messageLog.Add(message);
            RiseLogUpdateEvent(message);
        }
        public static void LogSystemMessage(string content)
        {
            Message message = new Message("System", content){ IsOutgoing = false };
            messageLog.Add(message);
            RiseLogUpdateEvent(message);
        }

        private static void RiseLogUpdateEvent(Message message)
        {
            LogUpdated?.Invoke(null, new LogUpdatedEventArgs
            {
                NewMessage = message
            });
        }
    }

    public class LogUpdatedEventArgs : EventArgs
    {
        public Message NewMessage { get; set; }
    }
}
