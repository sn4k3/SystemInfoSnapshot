using System;
using System.Diagnostics;
using System.Web.UI;
using SystemInfoSnapshot.Core;
using SystemInfoSnapshot.Core.Process;

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


            ProcessManager.ClearProcessInfoCache();
            Managers.ProcessManager.Update();
            
            var i = 0;
            foreach (var process in Managers.ProcessManager)
            {
                i++;

                try
                {
                    if (string.IsNullOrEmpty(process.ProcessName) && string.IsNullOrEmpty(process.ExecutablePath))
                        continue;


                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "index", i.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "pid", process.Id.ToString());
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "name", process.ProcessName);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "file", process.ExecutablePath);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "uptime", process.UpTimeString);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "threads", process.Threads.Count.ToString());

                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "memory");
                    HtmlWriter.AddAttribute("data-order", process.WorkingSet64.ToString());
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                    HtmlWriter.Write((process.WorkingSet64 / 1024.0 / 1024.0).ToString("#.##"));
                    HtmlWriter.RenderEndTag();

                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "peakmemory");
                    HtmlWriter.AddAttribute("data-order", process.PeakWorkingSet64.ToString());
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
        }
    }
}
