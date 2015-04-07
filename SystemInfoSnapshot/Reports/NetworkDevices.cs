using System.Linq;
using System.Net.NetworkInformation;

namespace SystemInfoSnapshot.Reports
{
    public sealed class NetworkDevices : Report
    {
        public const string TemplateVar = "<!--[NETWORKDEVICES]-->";

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        protected override void Build()
        {
            var result = "<table data-sortable class=\"table table-striped table-bordered table-responsive table-hover sortable-theme-bootstrap\">" +
                            "<thead>" +
                                "<tr>" +
                                "<th>#</th>" +
                                "<th>Device ID</th>" +
                                "<th>Name</th>" +
                                "<th>Description</th>" +
                                "<th>Properties</th>" +
                                "<th>Statistics</th>" +
                                "<th class=\"text-center\">Status</th>" +
                                "</tr>" +
                            "</thead>" +
                            "<tbody>";
            var i = 0;
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                var properties = adapter.GetIPProperties();
                var statistics = adapter.GetIPStatistics();

                i++;
                result += "<tr>" +
                          "<td class=\"index\">" + i + "</td>" +
                          "<td class=\"deviceid\">" + adapter.Id +
                                "<br>MAC Address: " + string.Join(":", adapter.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2"))) +
                                "<br>Type: " + adapter.NetworkInterfaceType + "</td>" +
                          "<td class=\"name\">" + adapter.Name + "</td>" +
                          "<td class=\"description\">" + adapter.Description + "</td>" +
                          "<td class=\"properties\">";

                result += "<strong>Unicast Addresses:</strong>" +
                          "<ol>";
                result = properties.UnicastAddresses.Aggregate(result, (current, address) => current + string.Format("<li>{0}</li>", address.Address));
                result += "</ol>";

                result += "<strong>DHCP Server Addresses:</strong>" +
                          "<ol>";
                result = properties.DhcpServerAddresses.Aggregate(result, (current, address) => current + string.Format("<li>{0}</li>", address));
                result += "</ol>";

                result += "<strong>Gateway Addresses:</strong>" +
                          "<ol>";
                result = properties.GatewayAddresses.Aggregate(result, (current, address) => current + string.Format("<li>{0}</li>", address.Address));
                result += "</ol>";

                result += "<strong>DNS Addresses:</strong>" +
                          "<ol>";
                result = properties.DnsAddresses.Aggregate(result, (current, address) => current + string.Format("<li>{0}</li>", address));
                result += "</ol>";



                result += "</td>" +
                          "<td class=\"statistics\">" +
                          "<strong>Megabytes Received:</strong> " + (statistics.BytesReceived / 1024 / 1024) + "mb" +
                          "<br><strong>Megabytes Sent:</strong> " + (statistics.BytesSent / 1024 / 1024) + "mb" +
                          "</td>";



                result += "<td class=\"status text-center ";

                switch (adapter.OperationalStatus)
                {
                    case OperationalStatus.Up:
                        result += "text-success";
                        break;
                    case OperationalStatus.Down:
                        result += "text-danger";
                        break;
                    default:
                        result += "text-warning";
                        break;
                }

                result += "\">" + adapter.OperationalStatus + "</td>" +
                           "</tr>";


            }
            result += "</tbody></table>";
            Html = result;
        }
    }
}
