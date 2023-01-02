using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FormSwitch.Models
{
    [XmlRoot("ArrayOfApplication")]
    public abstract class ApplicationList
    {
        [XmlElement("Application")]
        public List<Application> Applications { get; set; }
    }

    [XmlRoot("Application")]
    public class Application
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("CreationDate")]
        public DateTime CreationDate { get; set; }

        // public Application(int id, string name, DateTime creationDate)
        // {
        //     Id = id;
        //     Name = name;
        //     CreationDate = creationDate;
        // }

        public Application(string name)
        {
            Name = name;
        }
    }
}
