using System;
using System.Globalization;

namespace SystemInfoSnapshot.Reports
{
    public sealed class Title : Report
    {
        public const string TemplateVar = "<!--[TITLE]-->";

        public Title()
        {
            CanAsync = false;
        }

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        protected override void Build()
        {
            Html = string.Format("{0} - {1}", Environment.MachineName,
                DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }
    }
}
