using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Models
{
    public class Subscription : Application
    {
        public Module Parent { get; set; }
        public String EventType { get; set; }
        public String Endpoint { get; set; }

        public Subscription(int id, String name, DateTime creationDate, Module parent, String eventType, String endpoint) : base(id, name, creationDate)
        {
            Parent = parent;
            EventType = eventType;
            Endpoint = Endpoint;
        }
    }
}