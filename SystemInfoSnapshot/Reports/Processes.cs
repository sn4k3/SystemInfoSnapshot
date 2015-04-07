using System;
using System.Diagnostics;
using System.Management;

namespace SystemInfoSnapshot.Reports
{
    public sealed class Processes : Report
    {
        public const string TemplateVar = "<!--[PROCESSES]-->";

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
                                    "<th>PID</th>" +
                //"<th>Title</th>" +
                                    "<th>Name</th>" +
                //"<th>Window Title</th>" +
                                    "<th>File</th>" +
                //"<th>Machine Name</th>" +
                                    "<th>Up Time</th>" +
                //"<th>CPU</th>" +
                                    "<th>Threads</th>" +
                                    "<th>Memory (MB)</th>" +
                                    "<th>Peak Memory (MB)</th>" +
                                "</tr>" +
                             "</thead>" +
                         "<tbody>";
            var i = 0;
            foreach (var process in Process.GetProcesses())
            {
                i++;

                try
                {
                    var processinfo = ProcessHelper.GetProcessInfo(process);
                    result += "<tr>" +
                                "<td class=\"index\">" + i + "</td>" +
                                "<td class=\"pid\">" + process.Id + "</td>" +
                        //"<td>" + proccess.Modules[0].ModuleName + "</td>" +
                                "<td class=\"name\">" + process.ProcessName + "</td>" +
                        //"<td>" + process.MainWindowTitle + "</td>" +
                                "<td class=\"file\">" + (processinfo[ProcessHelper.ExecutablePath] ?? string.Empty) + "</td>" +
                        //"<td>" + proccess.MachineName + "</td>" +
                                "<td class=\"uptime\">" + (processinfo[ProcessHelper.StartTime] != null ? DateTime.Now.Subtract(ManagementDateTimeConverter.ToDateTime(processinfo[ProcessHelper.StartTime].ToString())).ToString() : string.Empty) + "</td>" +
                        //"<td>" + process.UserProcessorTime + "</td>" +
                                "<td class=\"threads\">" + process.Threads.Count + "</td>" +
                                "<td class=\"memory\" data-value=\"" + process.WorkingSet64 + "\">" + (process.WorkingSet64 / 1024.0 / 1024.0).ToString("#.##") + "</td>" +
                                "<td class=\"peakmemory\" data-value=\"" + process.PeakWorkingSet64 + "\">" + (process.PeakWorkingSet64 / 1024.0 / 1024.0).ToString("#.##") + "</td>" +
                              "</tr>";
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            result += "</tbody></table>";
            Html = result;
        }
    }
}
