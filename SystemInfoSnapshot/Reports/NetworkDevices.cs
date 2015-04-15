using System.Linq;
using System.Net.NetworkInformation;
using System.Web.UI;

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
            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
            HtmlWriter.AddAttribute("data-sortable", "true");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "#");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Device ID");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Name / Description");
            //HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Description");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Properties");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Statistics");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, HtmlTextWriterAttribute.Class, "text-center", "Status");

            HtmlWriter.RenderEndTag(); // </tr>
            HtmlWriter.RenderEndTag(); // </thead>

            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

            var i = 0;
            foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                var properties = adapter.GetIPProperties();
                var statistics = adapter.GetIPv4Statistics();

                i++;
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "index", i.ToString());
                HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "deviceid", string.Format("{0}<br>" + "MAC Address: {1}<br>Type: {2}", 
                    adapter.Id, string.Join(":", adapter.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2"))), adapter.NetworkInterfaceType));
                HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "name", string.Format("<strong>Name:</strong> {0}<br><strong>Description:</strong> {1}", adapter.Name, adapter.Description));
                //HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "description", adapter.Description);

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "properties");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);


                HtmlWriter.RenderTag(HtmlTextWriterTag.Strong, "Unicast Addresses:");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Ol);
                foreach (var address in properties.UnicastAddresses)
                {
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Li, address.Address.ToString());
                }
                HtmlWriter.RenderEndTag();

                HtmlWriter.RenderTag(HtmlTextWriterTag.Strong, "DHCP Server Addresses:");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Ol);
                foreach (var address in properties.DhcpServerAddresses)
                {
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Li, address.ToString());
                }
                HtmlWriter.RenderEndTag();

                HtmlWriter.RenderTag(HtmlTextWriterTag.Strong, "Gateway Addresses:");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Ol);
                foreach (var address in properties.GatewayAddresses)
                {
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Li, address.Address.ToString());
                }
                HtmlWriter.RenderEndTag();

                HtmlWriter.RenderTag(HtmlTextWriterTag.Strong, "DNS Addresses:");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Ol);
                foreach (var address in properties.DnsAddresses)
                {
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Li, address.ToString());
                }
                HtmlWriter.RenderEndTag();


                HtmlWriter.RenderEndTag(); // </td>


                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                HtmlWriter.RenderTag(HtmlTextWriterTag.Strong, "Megabytes Received:");
                HtmlWriter.Write(" {0} MB<br>", (statistics.BytesReceived / 1024 / 1024));

                HtmlWriter.RenderTag(HtmlTextWriterTag.Strong, "Megabytes Sent:");
                HtmlWriter.Write(" {0} MB<br>", (statistics.BytesSent / 1024 / 1024));
                HtmlWriter.RenderEndTag(); // </td>



                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "status text-center");

                switch (adapter.OperationalStatus)
                {
                    case OperationalStatus.Up:
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-success");
                        break;
                    case OperationalStatus.Down:
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-danger");
                        break;
                    default:
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-warning");
                        break;
                }

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                HtmlWriter.Write(adapter.OperationalStatus);
                HtmlWriter.RenderEndTag(); // </td>

                HtmlWriter.RenderEndTag(); // </tr>
            }
            HtmlWriter.RenderEndTag(); // </tbody>
            HtmlWriter.RenderEndTag(); // </table>
        }
    }
}
