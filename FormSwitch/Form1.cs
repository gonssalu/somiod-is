using System;
using System.Net;
using System.Windows.Forms;
using RestSharp;
using uPLibrary.Networking.M2Mqtt;
using Application = FormSwitch.Models.Application;

namespace FormSwitch
{
    public partial class Form1 : Form
    {
        private static readonly string BrokerIp = "127.0.0.1";
        private static readonly string ApiBaseUri = @"http://localhost:44396/";

        private static readonly HttpStatusCode EntityAlreadyExists = (HttpStatusCode) 422;

        private MqttClient _mClient;
        private readonly RestClient _restClient = new RestClient(ApiBaseUri);

        public Form1()
        {
            InitializeComponent();
        }

        private bool ConnectToBroker()
        {
            _mClient = new MqttClient(BrokerIp);
            // _mClient.Connect("FormSwitch");
            _mClient.Connect(Guid.NewGuid().ToString());

            if (!_mClient.IsConnected) {
                MessageBox.Show("Could not connect to the message broker", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // MessageBox.Show("Connected to the message broker", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        private void CreateApplication()
        {
            var app = new Application
            {
                Name = "lighting"
            };

            var request = new RestRequest("api/somiod", Method.Post);
            // request.AddXmlBody(app);
            request.AddObject(app);

            var response = _restClient.Execute(request);

            if (response.StatusCode == EntityAlreadyExists) {
                MessageBox.Show("Application already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.StatusCode != HttpStatusCode.OK) {
                MessageBox.Show("Could not create application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Application created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!ConnectToBroker())
                Close();
            CreateApplication();
        }
    }
}
