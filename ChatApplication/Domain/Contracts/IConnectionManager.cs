using System.Collections.Generic;

namespace ChatApplication.Domain.Contracts
{
    public interface IConnectionManager
    {
        IEnumerable<string> OnlineUsers { get; }
        Dictionary<string, HashSet<string>> userMap { get; }

        void AddConnection(string userName, string connectionId);
        HashSet<string> GetConnections(string userName);
        void RemoveConnection(string connectionId);
    }
}
