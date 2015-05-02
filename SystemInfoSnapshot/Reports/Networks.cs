using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web.UI;
using SystemInfoSnapshot.Extensions;

namespace SystemInfoSnapshot.Reports
{
    public sealed class Networks : Report
    {
        public const string TemplateVar = "<!--[NETWORKS]-->";

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        protected override void Build()
        {
            var networks = NetworkInterface.GetAllNetworkInterfaces().ToList().OrderBy(@interface => @interface.OperationalStatus);
            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

            HtmlWriter.RenderTag(HtmlTextWriterTag.Caption, HtmlTextWriterAttribute.Class, "text-center", "<h2>Network Interfaces</h2>");

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
            foreach (var adapter in networks)
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



            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();


            /*
             * Global Statistics
             */

            try
            {
                var ipv4GlobalStatistics = ipGlobalProperties.GetIPv4GlobalStatistics();
                var ipv6GlobalStatistics = ipGlobalProperties.GetIPv6GlobalStatistics();

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Caption, HtmlTextWriterAttribute.Class, "text-center", "<h2>Global Statistics</h2>");

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Statistic Name");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "IPv4 Value");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "IPv6 Value");

                HtmlWriter.RenderEndTag(); // </tr>
                HtmlWriter.RenderEndTag(); // </thead>

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);


                foreach (var prop in ipv4GlobalStatistics.GetType().GetProperties())
                {
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "name", prop.Name.SpaceCamelCase());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "ipv4value", prop.GetValue(ipv4GlobalStatistics).ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "ipv6value", prop.GetValue(ipv6GlobalStatistics).ToString());
                    HtmlWriter.RenderEndTag();
                }

                HtmlWriter.RenderEndTag(); // </tbody>
                HtmlWriter.RenderEndTag(); // </table>
            }
            catch
            {
                // ignored
            }
            //HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "row");
            //HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
            //HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "col-sm-6");
            //HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            


            /*
             *  ICMP Statistics
             */

            try
            {
                var icmpv4Statistics = ipGlobalProperties.GetIcmpV4Statistics();
                var icmpv4StatisticsProps = ipGlobalProperties.GetIcmpV4Statistics().GetType().GetProperties();
                var icmpv6Statistics = ipGlobalProperties.GetIcmpV6Statistics();
                var icmpv6StatisticsProps = ipGlobalProperties.GetIcmpV6Statistics().GetType().GetProperties();

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Caption, HtmlTextWriterAttribute.Class, "text-center", "<h2>ICMP Statistics</h2>");

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "ICMPv4 Statistic Name");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "ICMPv4 Value");

                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "ICMPv6 Statistic Name");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "ICMPv6 Value");

                HtmlWriter.RenderEndTag(); // </tr>
                HtmlWriter.RenderEndTag(); // </thead>

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);


                for (var x = 0; x < Math.Max(icmpv4StatisticsProps.Length, icmpv6StatisticsProps.Length); x++)
                {
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                    if (x < icmpv4StatisticsProps.Length)
                    {
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "icmpv4name", icmpv4StatisticsProps[x].Name.SpaceCamelCase());
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "icmpv4value", icmpv4StatisticsProps[x].GetValue(icmpv4Statistics).ToString());
                    }
                    else
                    {
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "icmpv4name", string.Empty);
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "icmpv4value", string.Empty);
                    }

                    if (x < icmpv6StatisticsProps.Length)
                    {
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "icmpv6name", icmpv6StatisticsProps[x].Name.SpaceCamelCase());
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "icmpv6value", icmpv6StatisticsProps[x].GetValue(icmpv6Statistics).ToString());
                    }
                    else
                    {
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "icmpv6name", string.Empty);
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "icmpv6value", string.Empty);
                    }
                    HtmlWriter.RenderEndTag();
                }
                HtmlWriter.RenderEndTag(); // </tbody>
                HtmlWriter.RenderEndTag(); // </table>
            }
            catch
            {
                // ignored
            }

            
            try
            {
                /*
                *  TCP Statistics
                */

                var ipv4Statistics = ipGlobalProperties.GetTcpIPv4Statistics();
                var ipv6Statistics = ipGlobalProperties.GetTcpIPv6Statistics();

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Caption, HtmlTextWriterAttribute.Class, "text-center", "<h2>TCP Statistics</h2>");

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Statistic Name");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "TCPv4 Value");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "TCPv6 Value");

                HtmlWriter.RenderEndTag(); // </tr>
                HtmlWriter.RenderEndTag(); // </thead>

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

                foreach (var prop in ipv4Statistics.GetType().GetProperties())
                {
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "name", prop.Name.SpaceCamelCase());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "tcp4value", prop.GetValue(ipv4Statistics).ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "tcpv6value", prop.GetValue(ipv6Statistics).ToString());
                    HtmlWriter.RenderEndTag();
                }

                HtmlWriter.RenderEndTag(); // </tbody>
                HtmlWriter.RenderEndTag(); // </table>
            }
            catch
            {
                // ignored
            }


            try
            {
                /*
                *  UDP Statistics
                */

                var udp4Statistics = ipGlobalProperties.GetUdpIPv4Statistics();
                var udp6Statistics = ipGlobalProperties.GetUdpIPv6Statistics();

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Caption, HtmlTextWriterAttribute.Class, "text-center", "<h2>UDP Statistics</h2>");

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Statistic Name");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "UDPv4 Value");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "UDPv6 Value");

                HtmlWriter.RenderEndTag(); // </tr>
                HtmlWriter.RenderEndTag(); // </thead>

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

                foreach (var prop in udp4Statistics.GetType().GetProperties())
                {
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "name", prop.Name.SpaceCamelCase());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "udp4value", prop.GetValue(udp4Statistics).ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "udpv6value", prop.GetValue(udp6Statistics).ToString());
                    HtmlWriter.RenderEndTag();
                }

                HtmlWriter.RenderEndTag(); // </tbody>
                HtmlWriter.RenderEndTag(); // </table>
            }
            catch
            {
                // ignored
            }


            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

            HtmlWriter.RenderTag(HtmlTextWriterTag.Caption, HtmlTextWriterAttribute.Class, "text-center", "<h2>TCP and UDP Connections</h2>");

            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Protocol");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Local Address");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Local Port");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Remote Address");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Remote Port");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "State");


            HtmlWriter.RenderEndTag(); // </tr>
            HtmlWriter.RenderEndTag(); // </thead>
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

            try
            {
                /**
                 * Active TCP Connections
                 */
                var activeTcpConnections = ipGlobalProperties.GetActiveTcpConnections();

                foreach (var con in activeTcpConnections)
                {
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "protocol", con.LocalEndPoint.Address.ToString().Contains("::") ? "TCPv6" : "TCP");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "localip", con.LocalEndPoint.Address.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "localport", con.LocalEndPoint.Port.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "remoteip", con.RemoteEndPoint.Address.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "remoteport", con.RemoteEndPoint.Port.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "State", con.State.ToString());
                    HtmlWriter.RenderEndTag();
                }
            }
            catch
            {
                // ignored
            }


            try
            {
                /**
                 * Active TCP Listeners
                 */
                var activeTcpListeners = ipGlobalProperties.GetActiveTcpListeners();

                foreach (var con in activeTcpListeners)
                {
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "protocol", con.Address.ToString().Contains(":") ? "TCPv6" : "TCP");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "localip", con.Address.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "localport", con.Port.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "remoteip", con.Address.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "remoteport", "0");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "State", "Listening");
                    HtmlWriter.RenderEndTag();
                }
            }
            catch
            {
                // ignored
            }

            try
            {
                /**
                 * Active UDP Listeners
                 */
                var activeUdpListeners = ipGlobalProperties.GetActiveUdpListeners();

                foreach (var con in activeUdpListeners)
                {
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "protocol", con.Address.ToString().Contains(":") ? "UDPv6" : "UDP");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "localip", con.Address.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "localport", con.Port.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "remoteip", "*");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "remoteport", "*");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "State", "Listening");
                    HtmlWriter.RenderEndTag();
                }
            }
            catch
            {
                // ignored
            }

            HtmlWriter.RenderEndTag(); // </tbody>
            HtmlWriter.RenderEndTag(); // </table>
        }
    }
}
