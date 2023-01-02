using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;


namespace FormSwitch
{
    public partial class Form1 : Form
    {
        private MqttClient _mClient;

        public Form1()
        {
            InitializeComponent();
        }
    }
}
