using System;
using System.Text;
using System.Threading;
using SOMIOD.Exceptions;
using SOMIOD.Models;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace SOMIOD.Helpers
{
    public static class BrokerHelper
    {
        private static readonly string Guid = System.Guid.NewGuid().ToString();

        public static void FireNotification(string endPoint, string topic, Notification notification)
        {
            try {
                var mClient = new MqttClient(endPoint);
                mClient.Connect(Guid);

                if (!mClient.IsConnected)
                    throw new BrokerException("Couldn't connect to message broker endpoint '" + endPoint + "'");

                mClient.Publish(topic, Encoding.UTF8.GetBytes(XmlHelper.Serialize(notification).OuterXml));

                if (mClient.IsConnected) {
                    Thread.Sleep(1000);
                    mClient.Disconnect();
                }
            }
            catch (Exception e) {
                if (e is BrokerException) throw e;
                else if (e is MqttConnectionException)
                    throw new BrokerException("Couldn't connect to message broker endpoint '" + endPoint + "'");
                else
                    throw new BrokerException(e.Message);
            }

        }
    }
}
