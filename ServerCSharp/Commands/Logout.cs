using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCSharp.Commands
{
    public class Logout : Command
    {
        public Logout(Server server) : base(server) { }

        public override void Handle(Client client, string command)
        {
            if (!client.IsLoggedIn())
            {
                client.WriteLine("You are not logged in.", Globals.DenyColor);
                return;
            }

            client.Logout();
            client.WriteLine("You are now logged out.", Globals.AcceptColor);
        }
    }
}