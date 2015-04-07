using System;
using System.Globalization;

namespace SystemInfoSnapshot.Reports
{
    public sealed class Programs : Report
    {
        public const string TemplateVar = "<!--[PROGRAMS]-->";

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
                                "<th>Program</th>" +
                                "<th>Version</th>" +
                                "<th>Publisher</th>" +
                                "<th>Install Date</th>" +
                                "</tr>" +
                            "</thead>" +
                            "<tbody>";
            var i = 0;
            var programs = InstalledProgram.GetInstalledPrograms();
            foreach (var program in programs)
            {
                i++;
                result += "<tr>" +
                            "<td class=\"index\">" + i + "</td>" +
                            "<td class=\"program\">" + program.Name + "</td>" +
                            "<td class=\"version\">" + program.Version + "</td>" +
                            "<td class=\"publisher\">" + program.Publisher + "</td>" +
                            "<td class=\"installdate\">" + program.InstallDate + "</td>" +
                           "</tr>";


            }
            result += "</tbody></table>";

            Html = result;
        }
    }
}
