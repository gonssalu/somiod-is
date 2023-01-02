using System;
using System.Net;
using System.Windows.Forms;
using FormSwitch.Models;
using RestSharp;
using uPLibrary.Networking.M2Mqtt;
using Application = FormSwitch.Models.Application;

namespace FormSwitch
{
    public partial class Form1 : Form
    {
        // Constants

        private static readonly string BrokerIp = "127.0.0.1";
        private static readonly string ApiBaseUri = @"http://localhost:44396/";

        private static readonly HttpStatusCode EntityAlreadyExists = (HttpStatusCode) 422;

        private static readonly string ApplicationName = "lighting";
        private static readonly string ModuleName = "light_bulb";

        // End of constants

        private MqttClient _mClient;
        private readonly RestClient _restClient = new RestClient(ApiBaseUri);

        public Form1()
        {
            InitializeComponent();
        }

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

        private void CreateApplication(string applicationName)
        {
            var app = new Application(applicationName);

            var request = new RestRequest("api/somiod", Method.Post);
            request.AddObject(app);

            var response = _restClient.Execute(request);

            if (response.StatusCode == EntityAlreadyExists)
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

            if (response.StatusCode == EntityAlreadyExists)
                return;

            if (response.StatusCode != HttpStatusCode.OK) {
                MessageBox.Show("Could not create module", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // TODO: Remove this
            MessageBox.Show("Module created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ConnectToBroker();
            CreateApplication(ApplicationName);
            CreateModule(ModuleName, ApplicationName);
        }
    }
}
