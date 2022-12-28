using System;

namespace SOMIOD.Models
{
    public class Subscription : Application
    {
        public Module Parent { get; set; }
        public string EventType { get; set; }
        public string Endpoint { get; set; }

        public Subscription(int id, string name, DateTime creationDate, Module parent, string eventType, string endpoint) :
            base(id, name, creationDate)
        {
            Parent = parent;
            EventType = eventType;
            Endpoint = endpoint;
        }
    }
}
