using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SOMIOD.Models
{
    [XmlRoot(ElementName = "Notification")]
    public class Notification
    {

        [XmlElement(ElementName = "EventType")]
        public string EventType { get; set; }


        [XmlElement(ElementName = "Content")]
        public string Content { get; set; }
        
        public Notification() { }

        public Notification(string eventType, string content)
        {
            EventType = eventType;
            Content = content;
        }
    }
}