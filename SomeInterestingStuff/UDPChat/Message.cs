using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeInterestingStuff.UDPChat
{
    public class Message
    {
        public string SenderName { get; set; }
        public DateTime MessageDateTime { get; set; }
        public string MessageContent { get; set; }
        public bool IsOutgoing { get; set; }

        public Message(string senderName, string content)
        {
            MessageDateTime = DateTime.Now;
            SenderName = senderName;
            MessageContent = content;
        }

        public static byte[] ToBytes(Message message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        }

        public static Message ToMessage(byte[] bytes)
        {
            return JsonConvert.DeserializeObject<Message>(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
        }

        public static string ToString(Message message)
        {
            string direction;
            if (message.IsOutgoing) {direction = "=>  ";}
            else { direction = "<=  "; }

            return new StringBuilder().Append(direction).Append(message.MessageDateTime.ToString()).Append(Environment.NewLine)
                .Append(message.SenderName).Append(": ").Append(Environment.NewLine)
                .Append(message.MessageContent).ToString();
        }
    }
}
