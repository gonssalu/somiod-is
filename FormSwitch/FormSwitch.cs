using System;
using System.Net;
using System.Windows.Forms;
using RestSharp;
using SOMIOD.Models;
using static FormSwitch.Properties.Settings;

namespace FormSwitch
{
    public partial class FormSwitch : Form
    {
        #region Constants

        private static readonly string ApiBaseUri = Default.ApiBaseUri;
        private static readonly string ApplicationName = Default.ApplicationName;
        private static readonly string ModuleName = Default.ModuleName;

        #endregion

        private readonly RestClient _restClient = new RestClient(ApiBaseUri);

        public FormSwitch()
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

            if (response.StatusCode == 0) {
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.StatusCode != HttpStatusCode.OK) {
                MessageBox.Show("An error occurred while creating data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(response.Content);
        }
    }
}
