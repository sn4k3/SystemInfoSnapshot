using System;
using System.ServiceProcess;
using System.Web.UI;

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
            if (SystemHelper.IsWindows)
            {
                var services = ServiceController.GetServices();
                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "#");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Display Name");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Service Name");
                //HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Service Type");

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-center");
                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Width, "180");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Th);
                HtmlWriter.Write("Can Pause/Continue");
                HtmlWriter.RenderEndTag();

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-center");
                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Width, "180");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Th);
                HtmlWriter.Write("Can Shutdown");
                HtmlWriter.RenderEndTag();

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-center");
                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Width, "180");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Th);
                HtmlWriter.Write("Can Stop");
                HtmlWriter.RenderEndTag();

                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Status");

                HtmlWriter.RenderEndTag(); // </tr>
                HtmlWriter.RenderEndTag(); // </thead>

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

                var i = 0;
                // try to find service name
                foreach (var service in services)
                {
                    i++;
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "index", i.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "displayname",
                        service.DisplayName);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "displayname",
                        service.ServiceName);
                    //HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "servicetype", service.ServiceType.ToString());

                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-center canpause");
                    HtmlWriter.AddAttribute("data-order" +
                                            "", Convert.ToByte(service.CanPauseAndContinue).ToString());
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    HtmlWriter.Write(service.CanPauseAndContinue
                        ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>"
                        : "<span class=\"glyphicon glyphicon-remove text-error\"></span>");
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-center canshutdown");
                    HtmlWriter.AddAttribute("data-order", Convert.ToByte(service.CanShutdown).ToString());
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    HtmlWriter.Write(service.CanShutdown
                        ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>"
                        : "<span class=\"glyphicon glyphicon-remove text-error\"></span>");
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-center canstop");
                    HtmlWriter.AddAttribute("data-order", Convert.ToByte(service.CanStop).ToString());
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    HtmlWriter.Write(service.CanStop
                        ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>"
                        : "<span class=\"glyphicon glyphicon-remove text-error\"></span>");
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "status");

                    /*result += "<tr>" +
                          "<td class=\"index\">" + i + "</td>" +
                          "<td class=\"displayname\">" + service.DisplayName + "</td>" +
                          "<td class=\"servicename\">" + service.ServiceName + "</td>" +
                          "<td>" + service.ServiceType + "</td>" +
                    //"<td>" + service.MachineName + "</td>" +
                          "<td class=\"text-center canpause\" data-order=\"" + Convert.ToByte(service.CanPauseAndContinue) + "\">" + (service.CanPauseAndContinue ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>" : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                          "<td class=\"text-center canshutdown\" data-order=\"" + Convert.ToByte(service.CanShutdown) + "\">" + (service.CanShutdown ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>" : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                          "<td class=\"text-center canstop\" data-order=\"" + Convert.ToByte(service.CanStop) + "\">" + (service.CanStop ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>" : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                          "<td class=\"";*/
                    switch (service.Status)
                    {
                        case ServiceControllerStatus.Paused:
                            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-warning");
                            break;
                        case ServiceControllerStatus.StartPending:
                            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-primary");
                            break;
                        case ServiceControllerStatus.Running:
                            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-success");
                            break;
                        case ServiceControllerStatus.Stopped:
                            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-danger");
                            break;
                    }
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    HtmlWriter.Write(service.Status);
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.RenderEndTag(); // </tr>

                    //result += " status\">" + service.Status + "</td>" +
                    //          "</tr>";

                }
                //result += "</tbody></table>";
                HtmlWriter.RenderEndTag(); // </tbody>
                HtmlWriter.RenderEndTag(); // </table>
            }
            else
            {
                WriteNotSupportedMsg();
            }
        }
    }
}
