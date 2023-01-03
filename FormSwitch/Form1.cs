using System;
using System.Net;
using System.Windows.Forms;
using RestSharp;
using SOMIOD.Models;
namespace FormSwitch
{
    public partial class Form1 : Form
    {
        private static readonly string ApiBaseUri = @"http://localhost:44396/";
        private readonly RestClient _restClient = new RestClient(ApiBaseUri);
        private static readonly string ApplicationName = "lighting";
        private static readonly string ModuleName = "light_bulb";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            CreateData(ApplicationName, ModuleName, "ON");
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            CreateData(ApplicationName, ModuleName, "OFF");
        }

        private void CreateData(string appName, string moduleName, string content)
        {
            var mod = new Data(content);

            var request = new RestRequest($"api/somiod/{appName}/{moduleName}/data", Method.Post);
            request.AddObject(mod);

            var response = _restClient.Execute(request);

            if (response.StatusCode == 0)
            {
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                MessageBox.Show("An error occurred while creating data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
