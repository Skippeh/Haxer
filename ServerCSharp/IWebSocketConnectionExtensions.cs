using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fleck;

namespace ServerCSharp
{
    public static class IWebSocketConnectionExtensions
    {
        public static string IPString(this IWebSocketConnection connection)
        {
            return "(" + connection.ConnectionInfo.ClientIpAddress + ":" + connection.ConnectionInfo.ClientPort + ")";
        }
    }
}