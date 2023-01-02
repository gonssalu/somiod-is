using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using FormSwitch.Models;
using RestSharp;
using uPLibrary.Networking.M2Mqtt;
using Application = FormSwitch.Models.Application;

namespace FormSwitch
{
    public partial class Form1 : Form
    {
        private static readonly string BrokerIp = "127.0.0.1";
        private static readonly string ApiBaseUri = @"http://localhost:44396/";

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
            // var request = new RestRequest("api/applications", Method.Post)

            var request = new RestRequest("api/somiod")
            {
                RequestFormat = DataFormat.Xml
            };

            // var response = _restClient.Execute<ApplicationList>(request);
            var response = _restClient.Execute<List<Application>>(request).Data;

            foreach (var application in response) {
                MessageBox.Show(application.Name);
            }

            // var application = response.Data;
            //
            // MessageBox.Show(response.Content);
            // MessageBox.Show(response.Data.Applications?.Count.ToString());
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!ConnectToBroker())
                Close();
            CreateApplication();
        }
    }
}
