using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCSharp.Packets
{
    public class ReadLine : Packet
    {
        public ReadLine(Server server) : base(server, "line") { }

        public override void Handle(Client client, Message message)
        {
            if (client.PendingReadLine != null)
            {
                var callback = client.PendingReadLine;
                client.PendingReadLine = null;
                callback(message["line"] as string);
            }
        }
    }
}