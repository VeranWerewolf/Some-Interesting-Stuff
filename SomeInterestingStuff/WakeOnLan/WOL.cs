using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeInterestingStuff.WakeOnLan
{
    public static class WOL
    {
        public static byte[] MagicPacket(string preparedMAC)
        {
            //set sending bytes
            int counter = 0;
            //buffer to be send
            byte[] bytes = new byte[102]; //Magic packet is 102 bytes length, so 102 is enough :-)

            //first 6 bytes should be 0xFF
            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;
            //now repeat MAC 16 times
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

        public static void WakeFunction(TableHost HostToWake)
        {
            WOLClass client = new WOLClass();
            client.Connect(
                HostToWake.netipAdress,  //255.255.255.255 broadcast
                HostToWake.port); // port=9 is default

            //client.SetClientToBrodcastMode();

            //now send wake up packet
            byte[] packet = MagicPacket(HostToWake.preparedMac);
            int reterned_value = client.Send(packet, packet.Length);
            client.Close();
        }
    }
}
