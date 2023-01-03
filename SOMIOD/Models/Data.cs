using System;
using System.Xml.Serialization;

namespace SOMIOD.Models
{
    [XmlRoot(ElementName = "Data")]
    public class Data
    {
        [XmlElement(ElementName = "Id")]
        public int Id { get; set; }
        
        [XmlElement(ElementName = "Content")]
        public string Content { get; set; }
        
        [XmlElement(ElementName = "CreationDate")]
        public string CreationDate { get; set; }


        [XmlElement(ElementName = "Parent")]
        public int Parent { get; set; }

        public Data() { }

        public Data(int id, string content, DateTime creationDate, int parent)
        {
            {
                Id = id;
                Content = content;
                CreationDate = creationDate.ToString("yyyy-MM-dd HH:mm:ss");
                Parent = parent;
            }
        }
    }
}
