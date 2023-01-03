using System;
using System.IO;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using FormLightBulb.Models;
using FormLightBulb.Properties;
using RestSharp;
using SOMIOD.Models;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static FormLightBulb.Properties.Settings;
using Application = FormLightBulb.Models.Application;

namespace FormLightBulb
{
    public partial class FormLightBulb : Form
    {
        #region Constants

        private static readonly string BrokerIp = Default.BrokerIp;
        private static readonly string ApiBaseUri = Default.ApiBaseUri;
        private static readonly HttpStatusCode CustomApiError = (HttpStatusCode) Default.CustomApiError;
        private static readonly string ApplicationName = Default.ApplicationName;
        private static readonly string ModuleName = Default.ModuleName;
        private static readonly string SubscriptionName = Default.SubscriptionName;
        private static readonly string EventType = Default.EventType;
        private static readonly string Endpoint = Default.Endpoint;
        private static readonly string[] Topic = { Default.Topic };

        #endregion

        private MqttClient _mClient;
        private readonly RestClient _restClient = new RestClient(ApiBaseUri);
        private bool _turnLightBulbOn;

        public FormLightBulb()
        {
            InitializeComponent();
        }

        #region Helpers

        private string DeserializeError(RestResponse response)
        {
            var error = JsonSerializer.Deserialize<Error>(response.Content ?? string.Empty);
            return error?.Message;
        }

        private bool CheckEntityAlreadyExists(RestResponse response)
        {
            if (response.StatusCode == CustomApiError)
                if (DeserializeError(response).Contains("already exists"))
                    return true;

            return false;
        }

        private void UpdateLightBulbState()
        {
            if (_turnLightBulbOn) {
                _turnLightBulbOn = true;
                pbLightbulb.Image = Resources.light_bulb_on;
                return;
            }

            _turnLightBulbOn = false;
            pbLightbulb.Image = Resources.light_bulb_off;
        }

        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs args)
        {
            string message = Encoding.UTF8.GetString(args.Message);
            using (TextReader reader = new StringReader(message)) {
                var not = (Notification) new XmlSerializer(typeof(Notification)).Deserialize(reader);
                if (not.EventType != "CREATE") return;

                _turnLightBulbOn = not.Content == "ON";
                UpdateLightBulbState();
            }

        }

        #endregion

        #region Message Broker

        private void ConnectToBroker()
        {
            _mClient = new MqttClient(BrokerIp);
            _mClient.Connect(Guid.NewGuid().ToString());

            if (!_mClient.IsConnected) {
                MessageBox.Show("Could not connect to the message broker", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void SubscribeToTopics()
        {
            _mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
            _mClient.Subscribe(Topic, qosLevels);
        }

        #endregion

        #region API Calls

        private void CreateApplication(string applicationName)
        {
            var app = new Application(applicationName);

            var request = new RestRequest("api/somiod", Method.Post);
            request.AddObject(app);

            var response = _restClient.Execute(request);

            if (CheckEntityAlreadyExists(response))
                return;

            if (response.StatusCode == 0) {
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("An error occurred while creating the application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CreateModule(string moduleName, string applicationName)
        {
            var mod = new Module(moduleName, applicationName);

            var request = new RestRequest($"api/somiod/{applicationName}", Method.Post);
            request.AddObject(mod);

            var response = _restClient.Execute(request);

            if (CheckEntityAlreadyExists(response))
                return;

            if (response.StatusCode == 0) {
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("An error occurred while creating the module", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CreateSubscription(string subscriptionName, string moduleName, string applicationName, string eventType, string endpoint)
        {
            var sub = new Subscription(subscriptionName, moduleName, eventType, endpoint);

            var request = new RestRequest($"api/somiod/{applicationName}/{moduleName}/subscriptions", Method.Post);
            request.AddObject(sub);

            var response = _restClient.Execute(request);

            if (CheckEntityAlreadyExists(response))
                return;

            if (response.StatusCode == 0) {
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("An error occurred while creating the application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        private void FormLightBulb_Shown(object sender, EventArgs e)
        {
            ConnectToBroker();
            SubscribeToTopics();
            CreateApplication(ApplicationName);
            CreateModule(ModuleName, ApplicationName);
            CreateSubscription(SubscriptionName, ModuleName, ApplicationName, EventType, Endpoint);
        }

        private void FormLightBulb_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_mClient.IsConnected) {
                _mClient.Unsubscribe(Topic);
                _mClient.Disconnect();
            }
        }
    }
}
