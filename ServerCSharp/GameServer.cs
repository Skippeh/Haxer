using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerCSharp
{
    public class GameServer
    {
        public string IP { get; private set; }
        public List<Client> ConnectedClients { get; private set; }  

        public GameServer(string ip)
        {
            IP = ip;
            ConnectedClients = new List<Client>();

            IPAddress address;
            if (!IPAddress.TryParse(IP, out address))
                throw new ArgumentException("Specified IP is not a valid IP address.");
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(IP);
        }

        public static GameServer Read(BinaryReader reader)
        {
            return new GameServer(reader.ReadString());
        }

        public void DisconnectClient(Client client)
        {
            if (ConnectedClients.Contains(client))
            {
                ConnectedClients.Remove(client);
            }
        }
    }
}