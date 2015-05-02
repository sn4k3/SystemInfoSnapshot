using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using SystemInfoSnapshot.Core.Web;

namespace SystemInfoSnapshot.GUI.Forms
{
    public partial class FrmServerOptions : Form
    {
        #region Properties
        /// <summary>
        /// Gets the selected <see cref="IPEndPoint"/>
        /// </summary>
        public IPEndPoint IPEndPoint { get; private set; }

        /// <summary>
        /// Gets the selected server update interval
        /// </summary>
        public uint UpdateInterval { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public FrmServerOptions()
        {
            InitializeComponent();
            IPEndPoint = new IPEndPoint(IPAddress.Any, WebServer.DefaultPort);
            UpdateInterval = 10000;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            cbIPAddress.Items.Add("0.0.0.0");
            cbIPAddress.Items.Add("127.0.0.1");
            foreach (IPAddress ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
            {
                cbIPAddress.Items.Add(ip);
            }
            cbIPAddress.SelectedIndex = 0;

            tbPort.Text = WebServer.DefaultPort.ToString();
            tbUpdateInterval.Text = WebServer.DefaultUpdateInterval.ToString();
        }
        #endregion

        #region Events
        private void ButtonClick(object sender, System.EventArgs e)
        {
            if (sender == btnStartServer)
            {
                // IP cofig
                ushort port;
                if (!ushort.TryParse(tbPort.Text, out port) || port == 0)
                {
                    MessageBox.Show(
                        @"The selected port is invalid and can't be used, please choose another port and try again.",
                        @"Invalid port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                IPAddress ip = IPAddress.Any;
                if (cbIPAddress.SelectedItem.Equals("*") || cbIPAddress.SelectedItem.Equals("0:0:0:0"))
                {
                    // pass
                }
                if (cbIPAddress.SelectedItem.Equals("127.0.0.1") || cbIPAddress.SelectedItem.Equals("localhost"))
                {
                    ip = IPAddress.Loopback;
                }
                else if (!IPAddress.TryParse(cbIPAddress.SelectedItem.ToString(), out ip))
                {
                    MessageBox.Show(
                        @"The ip address is invalid and can't be used, please choose another ip and try again.",
                        @"Invalid ip address", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                IPEndPoint = new IPEndPoint(ip, port);

                // Update interval config
                uint updateInterval;
                if (uint.TryParse(tbUpdateInterval.Text, out updateInterval))
                {
                    if (updateInterval < 1)
                        updateInterval = WebServer.DefaultUpdateInterval;
                }
                else
                {
                    updateInterval = WebServer.DefaultUpdateInterval;
                }

                UpdateInterval = updateInterval;

                try
                {
                    if (!WebServer.Instance.Start(IPEndPoint))
                    {
                        MessageBox.Show("Failed to start the server, please try a different configuration.", "Failed to start the server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to start the server, please try a different configuration.", "Failed to start the server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                DialogResult = DialogResult.OK;
                Close();
                return;
            }
            if (sender == btnClose)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }
        }
        #endregion
    }
}
