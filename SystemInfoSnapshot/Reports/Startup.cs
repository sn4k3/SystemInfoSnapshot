using System;
using System.Collections.Generic;

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

            var result = "<ul class=\"nav nav-tabs\" role=\"tablist\">";
            foreach (var autorunDict in autorunsDict)
            {
                result += string.Format("<li role=\"presentation\" class=\"{0}\"><a href=\"#autorun_{2}\" aria-controls=\"{2}\" role=\"tab\" data-toggle=\"tab\"><i class=\"{4}\"></i> {1} ({3})</a></li>", 
                    autorunDict.Key.Equals("Logon") ? "active" : string.Empty, 
                    autorunDict.Key, autorunDict.Key.Replace(" ", ""), 
                    autorunDict.Value.Count, 
                    icons.ContainsKey(autorunDict.Key) ? icons[autorunDict.Key] : string.Empty);
            }
            result += "</ul>";


            result += "<div class=\"tab-content\">";
            foreach (var autorunDict in autorunsDict)
            {
                result += string.Format("<div role=\"tabpanel\" class=\"tab-pane{0}\" id=\"autorun_{1}\">", (autorunDict.Key.Equals("Logon") ? " active" : string.Empty), autorunDict.Key.Replace(" ", ""));
                if (autorunDict.Value.Count == 0)
                {
                    result += "<p><strong>No autorun entries under this category</strong></p>";
                    continue;
                }
                result += "<table data-sortable class=\"table table-striped table-bordered table-responsive table-hover sortable-theme-bootstrap\">" +
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
                         "<tbody>";
                var i = 0;
                foreach (var autorunEntry in autorunDict.Value)
                {
                    i++;
                    var extraClass = string.Empty;
                    if (!autorunEntry.IsValidFile)
                    {
                        extraClass = "warning";
                    }
                    result += "<tr class=\""+extraClass+"\">" +
                                "<td class=\"index\">" + i + "</td>" +
                                "<td class=\"text-center enabled\" data-value=\"" + Convert.ToByte(autorunEntry.Enabled) + "\">" + (autorunEntry.Enabled ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>" : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                                "<td class=\"entry\">" + autorunEntry.Entry + "</td>" +
                                "<td class=\"description\">" + autorunEntry.Description + "</td>" +
                                "<td class=\"publisher\">" + autorunEntry.Publisher + "</td>" +
                                "<td class=\"path\">" + autorunEntry.ImagePath + "</td>" +
                               "</tr>";
                }
                result += "</tbody></table>";
                result += "</div>";
            }
            result += "</div>";

            Html = result;
        }
    }
}
