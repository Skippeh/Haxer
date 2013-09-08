using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;
using Newtonsoft.Json;
using ServerCSharp.Packets;

namespace ServerCSharp
{
    public class Server
    {
        public GameWorld World { get; private set; }

        private WebSocketServer websocketServer;
        private Dictionary<long, Client> clients;
        private Dictionary<string, Packet> packetHandlers;

        private long nextClientId = 0;

        public Server(string serverLocation)
        {
            websocketServer = new WebSocketServer(serverLocation);
            clients = new Dictionary<long, Client>();
            packetHandlers = new Dictionary<string, Packet>();

            World = GameWorld.CreateOrLoad("world");

            AddPackets();
        }

        public void Start()
        {
            websocketServer.Start(OnClientConnected);
        }

        public void Stop()
        {
            websocketServer.Dispose();
            World.Save();
        }

        private void OnClientConnected(IWebSocketConnection connection)
        {
            Console.WriteLine("Client from " + connection.IPString() + " connected.");

            var id = nextClientId++;
            clients.Add(id, new Client(this, id, connection));
        }

        private void AddPackets()
        {
            packetHandlers.Add("auth", new Packets.AuthPacket(this));
            packetHandlers.Add("command", new Packets.ConsoleCommand(this));
        }

        public void HandleMessage(Client client, string message)
        {
            Dictionary<string, object> data;
            try
            {
                data = JsonConvert.DeserializeObject<Dictionary<string, object>>(message);
            }
            catch (Exception ex)
            {
                client.Kick("Message is not properly formatted. (" + ex.Message + ")");
                return;
            }

            if (!data.ContainsKey("id"))
            {
                client.Kick("No id defined in message.");
                return;
            }

            string id = (string) data["id"];
            if (!packetHandlers.ContainsKey(id))
            {
                client.Kick("Unknown id defined in message. (" + id + ")");
                return;
            }

            if (id != "auth" && !client.Authenticated)
            {
                client.Kick("You are not authenticated, i can't allow you to do that dave.");
                return;
            }

            var packetHandler = packetHandlers[id];
            if (packetHandler.RequiredParameters != null && packetHandler.RequiredParameters.Any(param => !data.Keys.Contains(param)))
            {
                client.Kick("You did not specify all the required parameters for message id \"" + id + "\".");
                return;
            }

            Console.WriteLine(client.Connection.IPString() + " Handling message: " + id);
            packetHandler.Handle(client, new Message(data));
        }

        public Client[] GetClients()
        {
            return GetClients(null);
        }

        public Client[] GetClients(Client except)
        {
            return clients.Values.Where(client => client.Authenticated && client != except).ToArray();
        }

        public void SendToAll(string id, Message message)
        {
            foreach(var client in clients.Values)
                client.Send(id, message);
        }

        public void SendToAll(string id, Message message, Client except)
        {
            var clients = GetClients(except);

            foreach (var client in clients)
                client.Send(id, message);
        }
    }
}