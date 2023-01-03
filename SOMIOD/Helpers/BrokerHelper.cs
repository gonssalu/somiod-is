using SOMIOD.Models;
using System;
using System.Reflection;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using uPLibrary.Networking.M2Mqtt.Messages;

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

                System.Diagnostics.Debug.WriteLine("\n\nConnected to message broker endpoint '" + endPoint + "'\n\n");

                _mClient.Publish(topic, Encoding.UTF8.GetBytes(XmlHelper.Serialize(notification).OuterXml));
                //_mClient.Publish(topic, Encoding.UTF8.GetBytes("ON"));

                if (_mClient.IsConnected)
                {
                    Thread.Sleep(1000);
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