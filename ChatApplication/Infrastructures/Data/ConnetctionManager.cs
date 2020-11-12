using System.Collections.Generic;
using System.Linq;
using ChatApplication.Domain.Attributes;
using ChatApplication.Domain.Contracts;

namespace ChatApplication.Infrastructures.Data
{

    [SingletonDependecy]
    public class ConnetctionManager : IConnectionManager
    {
        public ConnetctionManager()
        {
            userMap = new Dictionary<string, HashSet<string>>();
        }
        public Dictionary<string, HashSet<string>> userMap { get; }

        public IEnumerable<string> OnlineUsers { get => userMap.Keys; }

        public void AddConnection(string userName, string connectionId)
        {
            lock (userMap)
            {
                if (!userMap.ContainsKey(userName))
                {
                    userMap[userName] = new HashSet<string>();
                }
                userMap[userName].Add(connectionId);
            }
        }

        public HashSet<string> GetConnections(string userName)
        {
            var conn = new HashSet<string>();
            try
            {
                lock (userMap)
                {
                    conn = userMap[userName];
                }
            }
            catch
            {
                conn = null;
            }
            return conn;
        }

        public void RemoveConnection(string connectionId)
        {
            lock (userMap)
            {
                foreach (string userName in userMap.Keys)
                {
                    if (userMap.ContainsKey(userName))
                    {
                        if (userMap[userName].Contains(connectionId))
                        {
                            userMap[userName].Remove(connectionId);
                            if (!userMap[userName].Any())
                                userMap.Remove(userName);
                            break;
                        }
                    }
                }
                //userMap = userMap.Except(userMap.Where(item => item.Value.Contains(connectionId))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
        }
    }
}