using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerCSharp.Commands
{
    public class Login : Command
    {
        public Login(Server server) : base(server) { }
        
        public override void Handle(Client client, string command)
        {
            //if (Arguments.Length == 0)
            //{
            //    client.SendWriteLine("Login to the specified account.\nUsage: login user password");
            //    return;
            //}
            //
            //if (Arguments.Length < 2)
            //{
            //    client.SendWriteLine("You need to specify both a username and a password.", Globals.DenyColor);
            //    return;
            //}
            //
            //if (client.IsLoggedIn())
            //{
            //    client.SendWriteLine("You are already logged in. Please logout first.", Globals.DenyColor);
            //    return;
            //}
            //
            //string user = Arguments[0];
            //string password = JoinArg(1);
            //
            //Account account;
            //if (!Account.Exists(user) || !Account.TryLogin(user, password, out account))
            //{
            //    client.SendWriteLine("Invalid username or password. Use the register command to register a new account.", Globals.DenyColor);
            //    return;
            //}
            //
            //client.Account = account;
            //client.SendWriteLine("Logged in as " + account.Username + ".", Globals.AcceptColor);

            client.SendWrite("Username: ");
            client.ReadLine(username =>
                            {
                                client.SendWrite("Password: ");
                                client.ReadLine(password =>
                                                {
                                                    Account account;
                                                    if (!Account.Exists(username) || !Account.TryLogin(username, password, out account))
                                                    {
                                                        client.SendWriteLine("Invalid username or password. Use the register command to register a new account.", Globals.DenyColor);
                                                        return;
                                                    }

                                                    client.Account = account;
                                                    client.SendWriteLine("Logged in as " + account.Username + ".", Globals.AcceptColor);
                                                }, true);
                            });
        }
    }
}