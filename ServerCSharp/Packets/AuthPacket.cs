using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCSharp.Packets
{
    public class AuthPacket : Packet
    {
        public AuthPacket(Server server) : base(server) { }

        public override void Handle(Client client, Message message)
        {
            if (client.Authenticated)
            {
                client.Send("auth", new Message("success", false,
                                                "reason", "You are already authenticated."));
                client.Kick("Auth failure");
                return;
            }

            client.Authenticated = true;
            client.Send("auth", new Message("success", true,
                                            "clientID", client.ID,
                                            "motd", Globals.MOTD));
        }
    }
}