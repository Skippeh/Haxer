using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Haxer Server";

            Globals.LoadValues();

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.CursorTop -= 1;
            const string msg = "Use CTRL+C to exit the server, don't press the X unless you want to loose any unsaved changes!";
            Console.CursorLeft = (Console.WindowWidth / 2) - (msg.Length / 2);
            Console.WriteLine(msg);
            Console.ResetColor();

            var server = new Server("ws://192.168.1.200:45654");
            server.Start();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                server.Stop();
                Globals.Save();
            };

            Account.Create("Skippy", "test");

            while (true)
            {
                Thread.Sleep(500);
            }
        }
    }
}