using System;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Web.UI;

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
            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
            HtmlWriter.AddAttribute("data-sortable", "true");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "#");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "PID");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Name");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "File");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Up Time");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Threads");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Memory (MB)");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Peak Memory (MB)");

            HtmlWriter.RenderEndTag(); // </tr>
            HtmlWriter.RenderEndTag(); // </thead>

            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);


            ProcessHelper.ClearProcessInfoCache();

            var i = 0;
            foreach (var process in Process.GetProcesses())
            {
                i++;

                try
                {
                    var file = string.Empty;
                    var uptime = DateTime.Now.Subtract(process.StartTime).ToString(@"d\.hh\:mm\:ss");

                    try // Try if process is executed under 32bit and system is 64bits we need to handle permission exceptions
                    {
                        file = process.MainModule.FileName;
                    }
                    catch (Exception)
                    {
                        if (SystemHelper.IsWindows)
                        {
                            var processinfo = ProcessHelper.GetProcessInfo(process);
                            if (!ReferenceEquals(processinfo, null))
                            {
                                if (!ReferenceEquals(processinfo[ProcessHelper.ExecutablePath], null))
                                    file = processinfo[ProcessHelper.ExecutablePath].ToString();

                                /*if (!ReferenceEquals(processinfo[ProcessHelper.StartTime], null))
                            {
                                uptime =
                                    DateTime.Now.Subtract(
                                        ManagementDateTimeConverter.ToDateTime(
                                            processinfo[ProcessHelper.StartTime].ToString())).ToString(@"d\.hh\:mm\:ss");
                            }*/
                            }
                        }
                    }


                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "index", i.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "pid", process.Id.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "name", process.ProcessName);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "file", file);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "uptime", uptime);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "threads", process.Threads.Count.ToString());

                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "memory");
                    HtmlWriter.AddAttribute("data-value", process.WorkingSet64.ToString());
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    HtmlWriter.Write((process.WorkingSet64 / 1024.0 / 1024.0).ToString("#.##"));
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "peakmemory");
                    HtmlWriter.AddAttribute("data-value", process.PeakWorkingSet64.ToString());
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    HtmlWriter.Write((process.PeakWorkingSet64 / 1024.0 / 1024.0).ToString("#.##"));
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.RenderEndTag(); // </tr>
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            HtmlWriter.RenderEndTag(); // </tbody>
            HtmlWriter.RenderEndTag(); // </table>
            //result += "</tbody></table>";
        }
    }
}
