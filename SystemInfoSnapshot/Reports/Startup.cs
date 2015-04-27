using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using SystemInfoSnapshot.Core;

namespace SystemInfoSnapshot.Reports
{
    public sealed class Startup : Report
    {
        public const string TemplateVar = "<!--[AUTORUNS]-->";


        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        // OLD BUILD
        /*protected override void Build()
        {
            var result = "<table data-sortable class=\"table table-striped table-bordered table-responsive table-hover sortable-theme-bootstrap\">" +
                            "<thead>" +
                                "<tr>" +
                                    "<th>#</th>" +
                                    "<th>Key</th>" +
                                    "<th>Program</th>" +
                                    "<th>File</th>" +
                                "</tr>" +
                            "</thead>" +
                         "<tbody>";
            var i = 0;
            var autoruns = Autorun.GetAutoruns();
            foreach (var autorun in autoruns)
            {
                i++;
                result += "<tr>" +
                            "<td class=\"index\">" + i + "</td>" +
                            "<td class=\"key\">" + autorun.Key + "</td>" +
                            "<td class=\"program\">" + autorun.Program + "</td>" +
                            "<td class=\"file\">" + autorun.Path + "</td>" +
                           "</tr>";
            }


            result += "</tbody></table>";
            Html = result;
        }*/

        protected override void Build()
        {
            if (SystemHelper.IsWindows)
            {
                var icons = new Dictionary<string, string>
                {
                    {"Logon", "fa fa-sign-in"},
                    {"Explorer", "fa fa-folder"},
                    {"Internet Explorer", "fa fa-globe"},
                    {"Tasks", "fa fa-clock-o"},
                    {"Services", "fa fa-cogs"},
                    {"Drivers", "fa fa-desktop"},
                    {"Codecs", "fa fa-play-circle"},
                    {"Boot Execute", ""},
                    {"Hijacks", ""},
                    {"Known DLLs", "fa fa-file-o"},
                    {"Winlogon", ""},
                    {"Print Monitors", "fa fa-print"},
                    {"LSA Providers", "fa fa-shield"},
                    {"Network Providers", "fa fa-wifi"},
                    {"WDM", ""},
                    {"WMI", ""},
                    {"Sidebar Gadgets", ""},
                };
                Managers.AutorunManager.Update();
                var autorunsDict = Managers.AutorunManager.GetAsDictionary();

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "nav nav-tabs");
                HtmlWriter.AddAttribute("role", "tablist");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Ul);
                //result = "<ul class=\"nav nav-tabs\" role=\"tablist\">";
                foreach (var autorunDict in autorunsDict)
                {
                    if (autorunDict.Key.Equals("Logon"))
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "active");
                    HtmlWriter.AddAttribute("role", "presentation");
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Li);

                    var id = autorunDict.Key.Replace(" ", string.Empty);
                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("#autorun_{0}", id));
                    HtmlWriter.AddAttribute("aria-controls", id);
                    HtmlWriter.AddAttribute("role", "tab");
                    HtmlWriter.AddAttribute("data-toggle", "tab");
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.A);


                    if(icons.ContainsKey(autorunDict.Key) )
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, icons[autorunDict.Key]);
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.I);
                    HtmlWriter.RenderEndTag(); // </i>

                    HtmlWriter.Write(" {0} ({1})", autorunDict.Key, autorunDict.Value.Count);

                    HtmlWriter.RenderEndTag(); // </a>
                    HtmlWriter.RenderEndTag(); // </li>
                }
                HtmlWriter.RenderEndTag(); // </ul>

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "tab-content");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                foreach (var autorunDict in autorunsDict)
                {
                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("autorun_{0}", autorunDict.Key.Replace(" ", string.Empty)));
                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "tab-pane");
                    if (autorunDict.Key.Equals("Logon"))
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "active");
                    HtmlWriter.AddAttribute("role", "tabpanel");
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                    if (autorunDict.Value.Count == 0)
                    {
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.P);
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Strong);
                        HtmlWriter.Write("No autorun entries under this category.");
                        HtmlWriter.RenderEndTag(); // </strong>
                        HtmlWriter.RenderEndTag(); // </p>
                        continue;
                    }

                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "#");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, HtmlTextWriterAttribute.Width, "100", "Enabled");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Entry");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Description");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Publisher");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Image Path");
                    
                    HtmlWriter.RenderEndTag(); // </tr>
                    HtmlWriter.RenderEndTag(); // </thead>

                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

                    var i = 0;
                    foreach (var autorunEntry in autorunDict.Value.OrderBy(item => !item.Enabled).ThenBy(item => item.Entry))
                    {
                        i++;
                        if (!autorunEntry.IsValidFile)
                        {
                            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "warning");
                        }
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "index", i.ToString());

                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-center enabled");
                        HtmlWriter.AddAttribute("data-order", Convert.ToByte(autorunEntry.Enabled).ToString());
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                        HtmlWriter.Write(autorunEntry.Enabled
                                      ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>"
                                      : "<span class=\"glyphicon glyphicon-remove text-error\"></span>");
                        HtmlWriter.RenderEndTag();

                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "entry", autorunEntry.Entry);
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "description", autorunEntry.Description);
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "publisher", autorunEntry.Publisher);
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "imagepath", autorunEntry.ImagePath);

                        HtmlWriter.RenderEndTag(); // </tr>
                    }
                    HtmlWriter.RenderEndTag(); // </tbody>
                    HtmlWriter.RenderEndTag(); // </table>
                    HtmlWriter.RenderEndTag(); // </div>
                }
                HtmlWriter.RenderEndTag(); // </div>
            }
            else
            {
                WriteNotSupportedMsg();
            }
        }
    }
}
