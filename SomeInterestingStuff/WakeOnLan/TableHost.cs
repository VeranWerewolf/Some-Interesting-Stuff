using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SomeInterestingStuff.WakeOnLan
{
    [DataContract]
    public class TableHost
    {
        public TableHost() { }
        /// <summary>
        /// Creating instance of WOL-Client. Throws ArgumentException if IP, port or MAC are not valid.
        /// </summary>
        public TableHost(string ip, int Port, string name, string mac)
        {
            if (!InputChecker.LanSettingsChecker.IsValidIP(ip))
            {
                throw new ArgumentException("IP validation failed!");
            }
            else if (!InputChecker.LanSettingsChecker.IsValidMAC(mac))
            {
                throw new ArgumentException("MAC validation failed!");
            }
            else if (!InputChecker.LanSettingsChecker.IsValidPort(Port))
            {
                throw new ArgumentException("Port validation failed!");
            }

            this.ipAdress = ip;
            this.port = Port;
            this.computerName = name;
            this.macAddress = mac;


        }

        /// <summary>
        /// Creating instance of WOL-Client. Throws ArgumentException if IP, port or MAC are not valid.
        /// </summary>
        public TableHost(string ipAndPort, string name, string mac)
        {
            int colonIndex = ipAndPort.IndexOf(':');
            string ip;
            int Port;

            if (colonIndex == -1)
            {
                ip = ipAndPort;
                Port = 9;
            }
            else
            {
                ip = ipAndPort.Substring(0, colonIndex);
                Port = int.Parse(ipAndPort.Substring(colonIndex + 1));
            }

            if (!InputChecker.LanSettingsChecker.IsValidIP(ip))
            {
                throw new ArgumentException("IP validation failed!");
            }
            else if (!InputChecker.LanSettingsChecker.IsValidMAC(mac))
            {
                throw new ArgumentException("MAC validation failed!");
            }
            else if (!InputChecker.LanSettingsChecker.IsValidPort(Port))
            {
                throw new ArgumentException("Port validation failed!");
            }

            this.ipAdress = ip;
            this.port = Port;
            this.computerName = name;
            this.macAddress = mac;
        }

        [DataMember]
        public string ipAdress { get; set; }
        public System.Net.IPAddress netipAdress { get { return System.Net.IPAddress.Parse(this.ipAdress); } }
        [DataMember]
        public int port { get; set; }
        [DataMember]
        public string computerName { get; set; }
        [DataMember]
        public string macAddress { get; set; }
        public string preparedMac { get { return macAddress.Replace(":", "").Replace("-", ""); } }
    }
}
