using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.UI;

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
                var autoruns = new Autoruns();
                autoruns.BuildEntries();
                var autorunsDict = autoruns.GetAsDictionary();

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
                    /*result +=
                        string.Format(
                            "<li role=\"presentation\" class=\"{0}\"><a href=\"#autorun_{2}\" aria-controls=\"{2}\" role=\"tab\" data-toggle=\"tab\"><i class=\"{4}\"></i> {1} ({3})</a></li>",
                            autorunDict.Key.Equals("Logon") ? "active" : string.Empty,
                            autorunDict.Key, autorunDict.Key.Replace(" ", ""),
                            autorunDict.Value.Count,
                            icons.ContainsKey(autorunDict.Key) ? icons[autorunDict.Key] : string.Empty);*/
                }
                HtmlWriter.RenderEndTag(); // </ul>
                //result += "</ul>";

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "tab-content");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                //result += "<div class=\"tab-content\">";
                foreach (var autorunDict in autorunsDict)
                {
                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("autorun_{0}", autorunDict.Key.Replace(" ", string.Empty)));
                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "tab-pane");
                    if (autorunDict.Key.Equals("Logon"))
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "active");
                    HtmlWriter.AddAttribute("role", "tabpanel");
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                    //result += string.Format("<div role=\"tabpanel\" class=\"tab-pane{0}\" id=\"autorun_{1}\">",
                    //    (autorunDict.Key.Equals("Logon") ? " active" : string.Empty), autorunDict.Key.Replace(" ", ""));
                    if (autorunDict.Value.Count == 0)
                    {
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.P);
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Strong);
                        HtmlWriter.Write("No autorun entries under this category.");
                        //result += "<p><strong>No autorun entries under this category</strong></p>";
                        HtmlWriter.RenderEndTag(); // </strong>
                        HtmlWriter.RenderEndTag(); // </p>
                        continue;
                    }

                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
                    HtmlWriter.AddAttribute("data-sortable", "true");
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "#");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, HtmlTextWriterAttribute.Width, "100", "Enabled");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Entry");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Description");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Publisher");
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Image Path");
                    /*HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Width, "100");
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Th);
                    HtmlWriter.Write("Enabled");
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Th);
                    HtmlWriter.Write("Entry");
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Th);
                    HtmlWriter.Write("Description");
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Th);
                    HtmlWriter.Write("Publisher");
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Th);
                    HtmlWriter.Write("Image Path");
                    HtmlWriter.RenderEndTag();*/

                    HtmlWriter.RenderEndTag(); // </tr>
                    HtmlWriter.RenderEndTag(); // </thead>

                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

                    /*result +=
                        "<table data-sortable class=\"table table-striped table-bordered table-responsive table-hover sortable-theme-bootstrap\">" +
                        "<thead>" +
                        "<tr>" +
                        "<th>#</th>" +
                        "<th width=\"100\">Enabled</th>" +
                        "<th>Entry</th>" +
                        "<th>Description</th>" +
                        "<th>Publisher</th>" +
                        "<th>Image Path</th>" +
                        "</tr>" +
                        "</thead>" +
                        "<tbody>";*/
                    var i = 0;
                    foreach (var autorunEntry in autorunDict.Value)
                    {
                        i++;
                        if (!autorunEntry.IsValidFile)
                        {
                            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "warning");
                        }
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "index", i.ToString());

                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-center enabled");
                        HtmlWriter.AddAttribute("data-value", Convert.ToByte(autorunEntry.Enabled).ToString());
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
                        /*result += "<tr class=\"" + extraClass + "\">" +
                                  "<td class=\"index\">" + i + "</td>" +
                                  "<td class=\"text-center enabled\" data-value=\"" +
                                  Convert.ToByte(autorunEntry.Enabled) + "\">" +
                                  (autorunEntry.Enabled
                                      ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>"
                                      : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                                  "<td class=\"entry\">" + autorunEntry.Entry + "</td>" +
                                  "<td class=\"description\">" + autorunEntry.Description + "</td>" +
                                  "<td class=\"publisher\">" + autorunEntry.Publisher + "</td>" +
                                  "<td class=\"path\">" + autorunEntry.ImagePath + "</td>" +
                                  "</tr>";*/
                    }
                    HtmlWriter.RenderEndTag(); // </tbody>
                    HtmlWriter.RenderEndTag(); // </table>
                    //result += "</tbody></table>";
                    HtmlWriter.RenderEndTag(); // </div>
                    //result += "</div>";
                }
                HtmlWriter.RenderEndTag(); // </div>
                //result += "</div>";
            }
            else
            {
                WriteNotSupportedMsg();
                //result = "<strong>This operating system is not suported yet!</strong>";
            }
        }
    }
}
