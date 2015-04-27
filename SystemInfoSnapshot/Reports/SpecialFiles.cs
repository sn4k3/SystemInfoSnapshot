using System.Web.UI;
using SystemInfoSnapshot.Core;

namespace SystemInfoSnapshot.Reports
{
    public sealed class SpecialFiles : Report
    {
        public const string TemplateVar = "<!--[SPECIALFILES]-->";

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        protected override void Build()
        {
            var i = 0;

            foreach (var specialFile in Managers.SpecialFileManager)
            {
                i++;
                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "panel panel-success");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "panel-heading");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

                
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.H2);
                HtmlWriter.RenderTag(HtmlTextWriterTag.Div, HtmlTextWriterAttribute.Class, "pull-right", string.Format("<span class=\"badge\" style=\"font-size:34px;\">{0}</span>", i));
                HtmlWriter.Write("<i class=\"fa fa-file-o\"></i> {0}", specialFile.Filename);
                HtmlWriter.RenderEndTag(); // </h2>
                HtmlWriter.RenderTag(HtmlTextWriterTag.Span, specialFile.FullPath);
                HtmlWriter.RenderEndTag(); // </div>

                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "panel-body");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                HtmlWriter.RenderTag(HtmlTextWriterTag.Pre, specialFile.Content.ToString());
                HtmlWriter.RenderEndTag(); // </div>
                
                HtmlWriter.RenderEndTag(); // </div>
            }

            /*HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "#");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Program");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Version");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Publisher");
            HtmlWriter.RenderTag(HtmlTextWriterTag.Th, SystemHelper.IsWindows ? "Install Date" : "Description");

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
                HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "version", program.Version);
                HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "publisher", program.Publisher);

                if(SystemHelper.IsWindows)
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "installdate", program.InstallDate);
                else
                    HtmlWriter.RenderTag(HtmlTextWriterTag.Td, HtmlTextWriterAttribute.Class, "description", program.Description);

                HtmlWriter.RenderEndTag();

            }

            HtmlWriter.RenderEndTag(); // </tbody>
            HtmlWriter.RenderEndTag(); // </table>*/
        }
    }
}
