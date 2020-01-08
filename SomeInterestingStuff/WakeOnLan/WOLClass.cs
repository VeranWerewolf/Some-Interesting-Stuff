using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace SomeInterestingStuff.WakeOnLan
{
    public class WOLClass : UdpClient
    {
        public WOLClass()
            : base()
        { }

        //Установим broadcast для отправки сообщений
        public void SetClientToBrodcastMode()
        {
            if (this.Active)
                this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 0);
        }
    }
}
