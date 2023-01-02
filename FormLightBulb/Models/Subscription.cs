using System.Xml.Serialization;

namespace FormLightBulb.Models
{
    [XmlRoot("Subscription")]
    public class Subscription : Module
    {
        [XmlElement("EventType")]
        public string EventType { get; set; }

        [XmlElement("Endpoint")]
        public string Endpoint { get; set; }

        public Subscription(string name, string parent, string eventType, string endpoint) : base(name, parent)
        {
            EventType = eventType;
            Endpoint = endpoint;
        }
    }
}
