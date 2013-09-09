using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCSharp.Commands
{
    public class Clear : Command
    {
        public Clear(Server server) : base(server) { }

        public override void Handle(Client client, string command)
        {
            client.Send("clear");
        }
    }
}