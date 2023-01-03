using System;
using System.Net;
using System.Text;
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
        public Form1()
        {
            InitializeComponent();
        }
    }
}
