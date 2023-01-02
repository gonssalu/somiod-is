using SOMIOD.Models;
using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace SOMIOD.Helpers
{
    public class BrokerHelper
    {
        private static string _GUID = Guid.NewGuid().ToString();
        public static bool IsValidEndpoint(string name)
        {
            return Uri.CheckHostName(name) != UriHostNameType.Unknown;
        }

        public static void FireNotification(string endPoint, string topic, Notification notification)
        {
            try
            {
                var _mClient = new MqttClient(endPoint);
                _mClient.Connect(_GUID);

                if (!_mClient.IsConnected)
                    throw new BrokerException("Couldn't connect to message broker endpoint '" + endPoint + "'");

                _mClient.Publish(topic, Encoding.UTF8.GetBytes(XmlHelper.Serialize(notification).OuterXml));

                if (_mClient.IsConnected)
                {
                    _mClient.Disconnect();
                }
            }
            catch(Exception e)
            {
                if (e is BrokerException) throw e;
                else if(e is MqttConnectionException)
                    throw new BrokerException("Couldn't connect to message broker endpoint '" + endPoint + "'");
                else
                    throw new BrokerException(e.Message);
            }
            
        }
    }
}