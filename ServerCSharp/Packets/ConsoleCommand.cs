using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerCSharp.Commands;

namespace ServerCSharp.Packets
{
    public class ConsoleCommand : Packet
    {
        private Dictionary<string, Command> commands; 

        public ConsoleCommand(Server server) : base(server,  "cmd")
        {
            commands = new Dictionary<string, Command>();

            AddCommands();
        }

        private void AddCommands()
        {
            commands.Add("login", new Commands.Login(Server));
            commands.Add("logout", new Commands.Logout(Server));
            commands.Add("echo", new Commands.Echo(Server));
            commands.Add("clear", new Commands.Clear(Server));
        }

        public override void Handle(Client client, Message message)
        {
            var command = message["cmd"] as string;
            var args = command.Split(' ');
            var cmdId = args.Take(1).ElementAt(0);

            if (client.PendingReadKey != null || client.PendingReadLine !=  null)
            {
                client.Kick("You did not answer a read request.");
            }

            if (!commands.ContainsKey(cmdId.ToLower()))
            {
                client.WriteLine("Unknown command.", Globals.DenyColor);
                return;
            }

            bool firstIndex = true;
            var arguments = new List<string>();
            foreach (var arg in args)
            {
                if (firstIndex)
                {
                    firstIndex = false;
                    continue;
                }

                arguments.Add(arg);
            }

            commands[cmdId].Arguments = arguments.ToArray();
            commands[cmdId].Handle(client, command);
        }
    }
}