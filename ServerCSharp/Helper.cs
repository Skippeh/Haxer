using System;

namespace ServerCSharp
{
    internal static class Helper
    {
        public static string GenerateIPV4(Random rand)
        {
            var b0 = (byte)rand.Next(0, 256);
            var b1 = (byte)rand.Next(0, 256);
            var b2 = (byte)rand.Next(0, 256);
            var b3 = (byte)rand.Next(0, 256);

            return b0 + "." + b1 + "." + b2 + "." + b3;
        }
    }
}