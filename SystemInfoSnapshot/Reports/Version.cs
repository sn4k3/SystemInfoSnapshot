namespace SystemInfoSnapshot.Reports
{
    public sealed class Version : Report
    {
        public const string TemplateVar = "<!--[VERSION]-->";

        public Version()
        {
            CanAsync = false;
        }

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        protected override void Build()
        {
            HtmlWriter.Write(ApplicationInfo.Version);
        }
    }
}
