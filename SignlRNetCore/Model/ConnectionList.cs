using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignlRNetCore.Model
{
    public static class ConnectionList
    {
        public static List<ConnectionData> connections { get; set; } = new List<ConnectionData>();
        public static Dictionary<string, ConnectionData> ConnectionDictionary { get; set; } = new Dictionary<string, ConnectionData>();

        public static void AddUser(ConnectionData data)
        {
            connections.Add(data);
            ConnectionDictionary.Add(data.ConnectionId, data);
        }
    }
}
