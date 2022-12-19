using SignalRMessage.Models;
using System.Collections.Generic;

namespace SignalRMessage.Data
{
    public static class ClientSource
    {
        public static List<Client> Clients { get; } = new List<Client>();
    }
}
