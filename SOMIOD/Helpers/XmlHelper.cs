using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;
using SOMIOD.Exceptions;

namespace SOMIOD.Helpers
{
    public class XmlHelper
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
