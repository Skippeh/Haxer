using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCSharp.Packets
{
    public class ReadKey : Packet
    {
        public ReadKey(Server server) : base(server, "character", "shift", "ctrl", "alt") { }

        public override void Handle(Client client, Message message)
        {
            if (client.PendingReadKey != null)
            {
                var shift = (bool)message["shift"];
                var ctrl = (bool)message["ctrl"];
                var alt = (bool)message["alt"];
                var character = (char)message["character"];

                var callback = client.PendingReadKey;
                client.PendingReadKey = null;
                callback(new KeyInfo(shift, ctrl, alt, character));
            }
        }
    }
}