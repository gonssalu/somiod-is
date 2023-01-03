using System;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;
using FormSwitch.Models;
using RestSharp;
using static FormSwitch.Properties.Settings;

namespace FormSwitch
{
    public partial class FormSwitch : Form
    {
        #region Constants

        private static readonly string ApiBaseUri = Default.ApiBaseUri;
        private static readonly string ApplicationName = Default.ApplicationName;
        private static readonly string ModuleName = Default.ModuleName;
        private static readonly string ModuleToSendData = Default.ModuleToSendData;
        private static readonly HttpStatusCode CustomApiError = (HttpStatusCode) Default.CustomApiError;

        #endregion

        private readonly RestClient _restClient = new RestClient(ApiBaseUri);

        public FormSwitch()
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

        #region API Calls

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

        private void CreateData(string appName, string moduleToSendData, string content)
        {
            var mod = new Data(content);

            var request = new RestRequest($"api/somiod/{appName}/{moduleToSendData}/data", Method.Post);
            request.AddObject(mod);

            var response = _restClient.Execute(request);

            if (response.StatusCode == 0) {
                MessageBox.Show("Could not connect to the API", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (response.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("An error occurred while creating data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        private void btnOn_Click(object sender, EventArgs e)
        {
            CreateData(ApplicationName, ModuleToSendData, "ON");
        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            CreateData(ApplicationName, ModuleToSendData, "OFF");
        }

        private void FormSwitch_Shown(object sender, EventArgs e)
        {
            CreateModule(ModuleName, ApplicationName);
        }
    }
}
