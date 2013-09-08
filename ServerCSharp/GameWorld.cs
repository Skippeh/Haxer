using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServerCSharp
{
    public class GameWorld
    {
        public Dictionary<string, GameServer> Servers { get; private set; }
        public int Seed { get; private set; }

        private string fileName;

        private GameWorld(int? seed = null)
        {
            Seed = seed ?? new Random().Next();
            Servers = new Dictionary<string, GameServer>();
        }

        #region Init stuff
        public static GameWorld Load(string file)
        {
            if (!File.Exists(file))
                throw new ArgumentException("Specified file does not exist.", "file");

            Console.WriteLine("Loading world...");

            var world = new GameWorld();

            BinaryReader reader;
            try
            {
                reader = new BinaryReader(new FileStream(file, FileMode.Open, FileAccess.Read));
            }
            catch (IOException ex)
            {
                throw new ArgumentException("Specified save could not be loaded. (" + ex.Message + ")", "file", ex);
            }

            try
            {
                world.fileName = file;

                world.Seed = reader.ReadInt32();
                var serverCount = reader.ReadInt32();
                for (int i = 0; i < serverCount; ++i)
                {
                    var key = reader.ReadString();
                    var server = GameServer.Read(reader);
                    world.Servers.Add(key, server);
                }
            }
            catch (IOException ex)
            {
                throw new ArgumentException("Specified save could not be loaded. (" + ex.Message + ")", "file", ex);
            }

            reader.Close();

            Console.WriteLine("World loaded!");
            return world;
        }

        public void Save(string file)
        {
            if (File.Exists(file))
                Console.WriteLine("Overwriting save: " + file);

            BinaryWriter writer;
            try
            {
                writer = new BinaryWriter(new FileStream(file, FileMode.Create, FileAccess.Write));
            }
            catch (IOException ex)
            {
                throw new ArgumentException("Could not save to specified file. (" + ex.Message + ")", "file", ex);
            }

            writer.Write(Seed);
            writer.Write(Servers.Count);

            foreach (var keyVal in Servers)
            {
                var key = keyVal.Key;
                var server = keyVal.Value;

                writer.Write(key);
                server.Write(writer);
            }

            writer.Close();
        }

        public void Save()
        {
            Save(fileName);
        }

        public static GameWorld Create(int? seed)
        {
            Console.WriteLine("Creating world...");
            var world = new GameWorld(seed);

            var rand = new Random(world.Seed);
            world.GenerateWorld(rand);

            Console.WriteLine("World created!");
            return world;
        }

        public static GameWorld CreateOrLoad(string file, int? seed = null)
        {
            if (!File.Exists(file))
            {
                var world = Create(seed);
                world.fileName = file;

                return world;
            }

            return Load(file);
        }

        private void GenerateWorld(Random rand)
        {
            GenerateServers(rand);
        }

        private void GenerateServers(Random rand)
        {
            Console.WriteLine("Generating servers...");

            var serverCount = new Random().Next(0, 10000);
            for (int i = 0; i < serverCount; ++i )
            {
                string ip;
                while (true)
                {
                    ip = Helper.GenerateIPV4(rand);
                    if (!Servers.ContainsKey(ip))
                        break;
                }

                Servers.Add(ip, new GameServer(ip));
            }

            Console.WriteLine("Servers generated!");
        }
        #endregion
    }
}