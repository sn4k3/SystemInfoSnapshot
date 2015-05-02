using System;
using System.IO;
using System.Net;
using System.Threading;
using NHttp;

namespace SystemInfoSnapshot.Core.Web
{
    public class WebServer : IDisposable
    {
        #region Singleton
        /// <summary>
        /// A instance of this class
        /// </summary>
        private static WebServer _instance;

        /// <summary>
        /// Gets the singleton instance of this class
        /// </summary>
        public static WebServer Instance
        {
            get { return _instance ?? (_instance = new WebServer()); }
        }
        #endregion

        #region Constants
        /// <summary>
        /// Default port to use on web server
        /// </summary>
        public const ushort DefaultPort = 8080;

        /// <summary>
        /// Default update interval time in seconds
        /// </summary>
        public const uint DefaultUpdateInterval = 30;
        #endregion

        #region Properties
        public HttpServer HttpServer { get; private set; }
        #endregion

        #region Constructor
        private WebServer()
        {
            try
            {
                HttpServer = new HttpServer { EndPoint = new IPEndPoint(IPAddress.Any, DefaultPort) };
            }
            catch (Exception ex)
            {
                SystemHelper.DisplayMessage(ex.ToString());
            }
            
        }
        #endregion

        #region Methods
        /// <summary>
        /// Start the web server on a <see cref="IPEndPoint"/>
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool Start(IPEndPoint ipAddress = null)
        {
            if (HttpServer == null)
                return false;

            if (HttpServer.State != HttpServerState.Stopped)
                return false;

            if (!ReferenceEquals(ipAddress, null))
                HttpServer.EndPoint = ipAddress;
            HttpServer.RequestReceived += OnRequestReceived;

            HttpServer.Start();
            return true;
        }

        private void OnRequestReceived(object sender, HttpRequestEventArgs e)
        {
            if (e.Request.Path != null)
            {
                using (var writer = new StreamWriter(e.Response.OutputStream))
                {
                    if (!ReferenceEquals(Program.HtmlTemplate, null))
                    {
                        // Render the last avaliable report
                        writer.Write(Program.HtmlTemplate.TemplateHTML);
                    }
                    else
                    {
                        writer.Write("<h1>The report is not avaliable right now, please wait until it get generated and try again latter.</h1>");
                    }
                }
            }
            else
            {
                using (var writer = new StreamWriter(e.Response.OutputStream))
                {
                    writer.Write("<h1>Invalid request!</h1>");
                }
            }
        }

        public void BlockAndWait()
        {
            while (HttpServer.State != HttpServerState.Stopped)
            {
                Thread.Sleep(1000);
            }    
        }

        #endregion

        #region Overrides
        public void Dispose()
        {
            HttpServer.Dispose();
        }
        #endregion
    }
}
