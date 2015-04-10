using System;
using System.Collections.Generic;
using OpenHardwareMonitor.Hardware;

namespace SystemInfoSnapshot.Reports
{
    public sealed class SystemInfo : Report
    {
        public const string TemplateVar = "<!--[SYSTEMINFO]-->";

        public SystemInfo()
        {
            CanAsync = false;
        }

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        protected override void Build()
        {
            var icon = SystemHelper.IsWindows() ? "fa fa-windows fa-3x" : "fa fa-linux fa-3x";
            var result = "<div class=\"row\">" +
                            "<div class=\"col-sm-6 col-md-4 col-lg-3\">" +
                                "<div class=\"well text-center\">" +
                                    "<h1><i class=\""+icon+"\"></i></h1>" +
                                    "<!--<h2>System</h2>-->" +
                                    "<h3>" + Environment.OSVersion + "</h3>" +
                                    (Environment.Is64BitOperatingSystem ? "64" : "32") + " bits operative system<br>" +
                                    "Processor count: " + Environment.ProcessorCount + "<br>" +
                                    "System directory: " + Environment.SystemDirectory +
                                "</div>" +
                            "</div>"+
                            "<div class=\"col-sm-6 col-md-4 col-lg-3\">" +
                                "<div class=\"well text-center\">" +
                                    "<h1><i class=\"fa fa-user fa-3x\"></i></h1>" +
                                    "<!--<h2>User</h2>-->" +
                                    "<h3>" + Environment.MachineName + "</h3>" +
                                    "Domain Name: " + Environment.UserDomainName + "<br>" +
                                    "Username: " + Environment.UserName + "<br>" +
                                    "User folder: " + Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + 
                                "</div>" +
                            "</div>" +
                          "</div>";
            Html = result;
        }
    }
}
