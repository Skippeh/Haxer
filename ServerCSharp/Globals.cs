using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace ServerCSharp
{
    public static class Globals
    {
        public static string MOTD = "MOTD";
        public static string DenyColor = "#DA2525";
        public static string AcceptColor = "#0F0";
        public static string SaltKey;

        private static void Initialize()
        {
            // Initialize any values that can't be set on creation.

            var rand = new Random();
            var builder = new StringBuilder();
            for (int i = 0; i < 64; ++i)
            {
                int randNumb = rand.Next(97, 123);
                string value = ((char) randNumb).ToString();
                builder.Append(rand.NextDouble() < 0.5 ? value : value.ToUpper());
            }
            SaltKey = builder.ToString();
        }

        public static void LoadValues()
        {
            Initialize();

            if (File.Exists("globals.json"))
            {
                var _this = Assembly.GetExecutingAssembly().GetType("ServerCSharp.Globals");
                var contents = File.ReadAllText("globals.json");
                var obj = JsonConvert.DeserializeObject<Dictionary<string, object>>(contents);

                foreach (var field in _this.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    if (obj.ContainsKey(field.Name))
                    {
                        field.SetValue(field, obj[field.Name]);
                    }
                }
            }
            else
            {
                Save();
            }
        }

        public static void Save()
        {
            var _this = Assembly.GetExecutingAssembly().GetType("ServerCSharp.Globals");
            var globals = new Dictionary<string, object>();

            foreach (var field in _this.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var value = field.GetValue(field);
                globals[field.Name] = value;
            }

            var json = JsonConvert.SerializeObject(globals, Formatting.Indented);

            try
            {
                var file = File.CreateText("globals.json");

                file.Write(json);
                file.Close();
            }
            catch (IOException ex)
            {
                Console.WriteLine("Failed to save globals! (" + ex.Message + ")");
                throw;
            }
        }
    }
}