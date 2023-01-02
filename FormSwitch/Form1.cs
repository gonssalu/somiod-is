using System;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;
using FormSwitch.Models;
using RestSharp;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Application = FormSwitch.Models.Application;

namespace FormSwitch
{
    public partial class Form1 : Form
    {
        #region Constants

        private static readonly string BrokerIp = "127.0.0.1";
        private static readonly string ApiBaseUri = @"http://localhost:44396/";

        private static readonly HttpStatusCode CustomApiError = (HttpStatusCode) 422;

        private static readonly string ApplicationName = "lighting";
        private static readonly string ModuleName = "light_bulb";
        private static readonly string SubscriptionName = "sub1";
        private static readonly string EventType = "CREATE";
        private static readonly string Endpoint = "mqtt://127.0.0.1:1883";
        private static readonly string[] Topic = { "light_bulb" };

        #endregion

        private MqttClient _mClient;
        private readonly RestClient _restClient = new RestClient(ApiBaseUri);

        public Form1()
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

        #endregion

        #region Message Broker

        private void ConnectToBroker()
        {
            _mClient = new MqttClient(BrokerIp);
            // _mClient.Connect("FormSwitch");
            _mClient.Connect(Guid.NewGuid().ToString());

            if (!_mClient.IsConnected) {
                MessageBox.Show("Could not connect to the message broker", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }

            // MessageBox.Show("Connected to the message broker", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SubscribeToTopics()
        {
            // Msg arrived
            _mClient.MqttMsgPublishReceived += (sender, args) =>
                {
                    string message = System.Text.Encoding.UTF8.GetString(args.Message);
                    MessageBox.Show(message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

            // Subscribe to topic
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
            _mClient.Subscribe(Topic, qosLevels);

            MessageBox.Show("Subscribed to topics", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (response.StatusCode != HttpStatusCode.OK) {
                MessageBox.Show("Could not create application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // TODO: Remove this
            MessageBox.Show("Application created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CreateModule(string moduleName, string applicationName)
        {
            var mod = new Module(moduleName, applicationName);

            var request = new RestRequest($"api/somiod/{applicationName}", Method.Post);
            request.AddObject(mod);

            var response = _restClient.Execute(request);

            if (CheckEntityAlreadyExists(response))
                return;

            if (response.StatusCode != HttpStatusCode.OK) {
                MessageBox.Show("Could not create module", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // TODO: Remove this
            MessageBox.Show("Module created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CreateSubscription(string subscriptionName, string moduleName, string applicationName, string eventType, string endpoint)
        {
            var sub = new Subscription(subscriptionName, moduleName, eventType, endpoint);

            var request = new RestRequest($"api/somiod/{applicationName}/{moduleName}/subscriptions", Method.Post);
            request.AddObject(sub);

            var response = _restClient.Execute(request);

            if (CheckEntityAlreadyExists(response))
                return;

            if (response.StatusCode != HttpStatusCode.OK) {
                MessageBox.Show("Could not create subscription", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // TODO: Remove this
            MessageBox.Show("Subscription created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        private void Form1_Shown(object sender, EventArgs e)
        {
            ConnectToBroker();
            SubscribeToTopics();
            CreateApplication(ApplicationName);
            CreateModule(ModuleName, ApplicationName);
            CreateSubscription(SubscriptionName, ModuleName, ApplicationName, EventType, Endpoint);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_mClient.IsConnected) {
                _mClient.Unsubscribe(Topic);
                _mClient.Disconnect();
            }
        }
    }
}
