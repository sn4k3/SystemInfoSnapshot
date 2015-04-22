using System;
using System.Web.UI;
using SystemInfoSnapshot.Core.InstalledProgram;
using SystemInfoSnapshot.Core.Malware;
using SystemInfoSnapshot.Extensions;

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
            var i = 0;
            var programManager = new InstalledProgramManager();
            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "#");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Program");
            if (!SystemHelper.IsMacOSX)
            {
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Version");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Publisher");
                HtmlWriter.RenderTag(HtmlTextWriterTag.Th, SystemHelper.IsWindows ? "Install Date" : "Description");
            }

            HtmlWriter.RenderEndTag(); // </tr>
            HtmlWriter.RenderEndTag(); // </thead>

            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

            foreach (var program in programManager)
            {
                i++;
                if (MalwareManager.Instance.Contains(program.Name))
                {
                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "danger");
                }
                else if (program.Name.Contains("toolbar", StringComparison.OrdinalIgnoreCase))
                {
                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "warning");
                }

                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "index", i.ToString());
                HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "program", program.Name);

                if (!SystemHelper.IsMacOSX)
                {
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "version", program.Version);
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "publisher",
                        program.Publisher);

                    if (SystemHelper.IsWindows)
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "installdate",
                            program.InstallDate);
                    else
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "description",
                            program.Description);

                }

                HtmlWriter.RenderEndTag();

            }

            HtmlWriter.RenderEndTag(); // </tbody>
            HtmlWriter.RenderEndTag(); // </table>
        }
    }
}
