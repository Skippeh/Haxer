using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCSharp.Commands
{
    public abstract class Command
    {
        protected Server Server { get; private set; }
        public string[] Arguments { get; set; }

        protected Command(Server server)
        {
            Server = server;
        }

        public abstract void Handle(Client client, string command);

        protected string JoinArg(int start = 0, int count = -1)
        {
            if (count == -1)
                count = Arguments.Length - start;

            return string.Join(" ", Arguments.Skip(start).Take(count));
        }
    }
}