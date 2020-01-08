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
            if (!IsValidIP(ip))
            {
                throw new ArgumentException("IP validation failed!");
            }
            else if (!IsValidMAC(mac))
            {
                throw new ArgumentException("MAC validation failed!");
            }
            else if (!IsValidPort(Port))
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

            if (!IsValidIP(ip))
            {
                throw new ArgumentException("IP validation failed!");
            }
            else if (!IsValidMAC(mac))
            {
                throw new ArgumentException("MAC validation failed!");
            }
            else if (!IsValidPort(Port))
            {
                throw new ArgumentException("Port validation failed!");
            }

            this.ipAdress = ip;
            this.port = Port;
            this.computerName = name;
            this.macAddress = mac;
        }

        public bool IsValidPort(int Port)
        {
            return (Port >= 0 && Port <= 65535);
        }
        public bool IsValidMAC (string Address)
        {
            Address = Address.Replace(" ", "").Replace(":", "").Replace("-", "");

            // Match pattern for MAC address
            // Matches:12-23-34-45-56-67, 12:23:34:45:56:67, 122334455667
            // But not:12:34-4556-67
            string Pattern = @"^(?:[0-9a-fA-F]{2}:){5}[0-9a-fA-F]{2}|(?:[0-9a-fA-F]{2}-){5}[0-9a-fA-F]{2}|(?:[0-9a-fA-F]{2}){5}[0-9a-fA-F]{2}$";
            //Regular Expression object
            System.Text.RegularExpressions.Regex check =
                new System.Text.RegularExpressions.Regex(Pattern);
            
            //check to make sure an ip address was provided    
            if (string.IsNullOrEmpty(Address))
                //returns false if IP is not provided    
                return false;
            else
                //Matching the pattern    
                return check.IsMatch(Address, 0);
        }
        public bool IsValidIP(string Address)
        {
            //Match pattern for IP address    
            string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            //Regular Expression object    
            System.Text.RegularExpressions.Regex check =
                new System.Text.RegularExpressions.Regex(Pattern);

            //check to make sure an ip address was provided    
            if (string.IsNullOrEmpty(Address))
                //returns false if IP is not provided    
                return false;
            else
                //Matching the pattern    
                return check.IsMatch(Address, 0);
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
