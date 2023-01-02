using System.Xml.Serialization;

namespace FormSwitch.Models
{
    [XmlRoot("ArrayOfApplication")]
    public class Result
    {
        [XmlElement("Application")]
        public Application Application { get; set; }
    }
}
