using SOMIOD.Exceptions;
using System;
using System.Xml.Serialization;

namespace SOMIOD.Models
{
    [XmlRoot(ElementName = "Application")]
    public class Application
    {
        [XmlElement(ElementName = "Id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "CreationDate")]
        public string CreationDate { get; set; }


        public Application() { }

        public Application(int id, string name, DateTime creationDate)
        {
            Id = id;
            Name = name;
            CreationDate = creationDate.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
