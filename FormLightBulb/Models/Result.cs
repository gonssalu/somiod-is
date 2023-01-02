using System.Xml.Serialization;

namespace FormLightBulb.Models
{
    [XmlRoot("ArrayOfApplication")]
    public class Result
    {
        [XmlElement("Application")]
        public Application Application { get; set; }
    }
}
