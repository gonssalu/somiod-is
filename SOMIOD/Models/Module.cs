using System;
using System.Xml.Serialization;

namespace SOMIOD.Models
{
    [XmlRoot(ElementName = "Module")]
    public class Module : Application
    {
        [XmlElement(ElementName = "Parent")]
        public int Parent { get; set; }

        public Module() { }

        public Module(int id, string name, DateTime creationDate, int parent) : base(id, name, creationDate)
        {
            Parent = parent;
        }
    }
}
