using System;
using System.Xml.Serialization;

namespace SOMIOD.Models
{
    [XmlRoot(ElementName = "Subscription")]
    public class Subscription : Module
    {   
        [XmlElement(ElementName = "EventType")]
        public string EventType { get; set; }

        [XmlElement(ElementName = "Endpoint")]
        public string Endpoint { get; set; }
        
        public Subscription() { }
        
        public Subscription(int id, string name, DateTime creationDate, int parent, string eventType, string endpoint) :
            base(id, name, creationDate, parent)
        {
            EventType = eventType;
            Endpoint = endpoint;
        }
    }
}
