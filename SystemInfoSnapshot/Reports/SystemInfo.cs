using System;
using System.Web.UI;

namespace SystemInfoSnapshot.Reports
{
    public sealed class SystemInfo : Report
    {
        public const string TemplateVar = "<!--[SYSTEMINFO]-->";

        public SystemInfo()
        {
            CanAsync = false;
        }

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        protected override void Build()
        {
            var icon = SystemHelper.IsWindows ? "fa fa-windows fa-3x" : "fa fa-linux fa-3x";

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "row");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "col-sm-6 col-md-4 col-lg-3");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "well text-center");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.H1);
            HtmlWriter.RenderTag(HtmlTextWriterTag.I, HtmlTextWriterAttribute.Class, icon, null);
            HtmlWriter.RenderEndTag(); // </h1>

            HtmlWriter.RenderTag(HtmlTextWriterTag.H3, Environment.OSVersion.ToString());

            HtmlWriter.Write("{0} bits operative system<br>", (Environment.Is64BitOperatingSystem ? "64" : "32"));
            HtmlWriter.Write("Processor count: {0}<br>", Environment.ProcessorCount);
            HtmlWriter.Write("System directory: {0}", Environment.SystemDirectory);
            HtmlWriter.RenderEndTag(); // </div>
            HtmlWriter.RenderEndTag(); // </div>

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "col-sm-6 col-md-4 col-lg-3");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "well text-center");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.H1);
            HtmlWriter.RenderTag(HtmlTextWriterTag.I, HtmlTextWriterAttribute.Class, "fa fa-user fa-3x", null);
            HtmlWriter.RenderEndTag(); // </h1>

            HtmlWriter.RenderTag(HtmlTextWriterTag.H3, Environment.MachineName);

            HtmlWriter.Write("Domain Name: {0}<br>", Environment.UserDomainName);
            HtmlWriter.Write("Username: {0}<br>", Environment.UserName);
            HtmlWriter.Write("User folder: {0}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            HtmlWriter.RenderEndTag(); // </div>
            HtmlWriter.RenderEndTag(); // </div>

            HtmlWriter.RenderEndTag(); // </div>
        }
    }
}
