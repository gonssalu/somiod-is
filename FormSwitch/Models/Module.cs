using System;
using System.Xml.Serialization;

namespace FormSwitch.Models
{
    [XmlRoot("Module")]
    public class Module : Application
    {
        [XmlElement("Parent")]
        public string Parent { get; set; }

        public Module(int id, string name, DateTime creationDate, string parent) : base(id, name, creationDate)
        {
            Parent = parent;
        }

        public Module(string name, string parent) : base(name)
        {
            Parent = parent;
        }
    }
}
