using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SOMIOD.Helpers
{
    public static class XmlHelper
    {
        public static XmlDocument Serialize(object obj)
        {
            var xmlDoc = new XmlDocument();
            var serializer = new XmlSerializer(obj.GetType());

            using (var ms = new MemoryStream()) {
                serializer.Serialize(ms, obj);
                ms.Position = 0;
                xmlDoc.Load(ms);
                return xmlDoc;
            }
        }
    }
}
