using System;
using System.ServiceProcess;

namespace SystemInfoSnapshot.Reports
{
    public sealed class Services : Report
    {
        public const string TemplateVar = "<!--[SERVICES]-->";

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        protected override void Build()
        {
            var services = ServiceController.GetServices();

            var result = "<table data-sortable class=\"table table-striped table-bordered table-responsive table-hover sortable-theme-bootstrap\">" +
                            "<thead>" +
                                "<tr>" +
                                    "<th>#</th>" +
                                    "<th>Display Name</th>" +
                                    "<th>Service Name</th>" +
                                    "<th>Service Type</th>" +
                //"<th>Machine Name</th>" +
                                    "<th width=\"180\" class=\"text-center\">Can Pause/Continue</th>" +
                                    "<th width=\"180\" class=\"text-center\">Can Shutdown</th>" +
                                    "<th width=\"180\" class=\"text-center\">Can Stop</th>" +
                                    "<th>Status</th>" +
                                "</tr>" +
                            "</thead>" +
                         "<tbody>";

            var i = 0;
            // try to find service name
            foreach (var service in services)
            {
                i++;
                result += "<tr>" +
                          "<td class=\"index\">" + i + "</td>" +
                          "<td class=\"displayname\">" + service.DisplayName + "</td>" +
                          "<td class=\"servicename\">" + service.ServiceName + "</td>" +
                          "<td>" + service.ServiceType + "</td>" +
                    //"<td>" + service.MachineName + "</td>" +
                          "<td class=\"text-center canpause\" data-value=\"" + Convert.ToByte(service.CanPauseAndContinue) + "\">" + (service.CanPauseAndContinue ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>" : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                          "<td class=\"text-center canshutdown\" data-value=\"" + Convert.ToByte(service.CanShutdown) + "\">" + (service.CanShutdown ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>" : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                          "<td class=\"text-center canstop\" data-value=\"" + Convert.ToByte(service.CanStop) + "\">" + (service.CanStop ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>" : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                          "<td class=\"";
                switch (service.Status)
                {
                    case ServiceControllerStatus.Paused:
                        result += "text-warning";
                        break;
                    case ServiceControllerStatus.StartPending:
                        result += "text-primary";
                        break;
                    case ServiceControllerStatus.Running:
                        result += "text-success";
                        break;
                    case ServiceControllerStatus.Stopped:
                        result += "text-danger";
                        break;
                }

                result += " status\">" + service.Status + "</td>" +
                          "</tr>";

            }
            result += "</tbody></table>";
            Html = result;
        }
    }
}
