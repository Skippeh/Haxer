using System;
using System.Collections.Generic;
using Fleck;
using Newtonsoft.Json;

namespace ServerCSharp
{
    public class Client
    {
        public IWebSocketConnection Connection { get; private set; }

        public bool Authenticated { get; set; }
        public long ID { get; private set; }
        public Account Account { get; set; }

        private readonly Server server;

        public Client(Server server, long id, IWebSocketConnection connection)
        {
            this.server = server;
            ID = id;
            Connection = connection;
            Connection.OnClose = OnClose;
            Connection.OnMessage = OnMessage;
            Account = null;
        }

        public void Send(string id, Message data)
        {
            data["id"] = id;

            var json = data.GenerateJson();
            Connection.Send(json);
            Console.WriteLine(Connection.IPString() + " sent: " + json);
        }

        public void SendWrite(string text, string frontColor, string backColor = null)
        {
            Send("write", new Message("text", text,
                                      "endline", false,
                                      "fcolor", frontColor,
                                      "bcolor", backColor));
        }

        public void SendWriteLine(string text, string frontColor = null, string backColor = null)
        {
            Send("write", new Message("text", text,
                                      "endline", true,
                                      "fcolor", frontColor,
                                      "bcolor", backColor));
        }

        public void Kick(string reason)
        {
            Send("kick", new Message("reason", reason));
            Connection.Close();
        }

        public bool IsLoggedIn()
        {
            return Account != null;
        }

        private void OnClose()
        {
            Console.WriteLine(Connection.IPString() + " disconnected.");
        }

        private void OnMessage(string message)
        {
            Console.WriteLine(Connection.IPString() + " message: " + message);
            server.HandleMessage(this, message);
        }

        public void Logout()
        {
            foreach (var keyVal in server.World.Servers)
            {
                keyVal.Value.DisconnectClient(this);
            }

            Account = null;
        }
    }
}