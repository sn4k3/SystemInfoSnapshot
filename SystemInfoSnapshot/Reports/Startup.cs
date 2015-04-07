using System;
using System.Globalization;

namespace SystemInfoSnapshot.Reports
{
    public sealed class Startup : Report
    {
        public const string TemplateVar = "<!--[STARTUPAPPS]-->";

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
        }
    }
}
