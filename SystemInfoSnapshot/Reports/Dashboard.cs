using System;

namespace SystemInfoSnapshot.Reports
{
    public sealed class Dashboard : Report
    {
        public const string TemplateVar = "<!--[DASHBOARD]-->";

        public Dashboard()
        {
            CanAsync = false;
        }

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        protected override void Build()
        {
            var result = "<div class=\"row\">" +
                            "<div class=\"col-sm-6\">" +
                                "<div class=\"well text-center\">" +
                                    "<h1><i class=\"fa fa-exclamation-triangle fa-3x text-warning\"></i></h1>" +
                                    "<h2>Warnings</h2>" +
                                    "<h3 id=\"dashboard-warnings\" class=\"text-warning\">0</h3>" +
                                "</div>" +
                            "</div>" +
                             "<div class=\"col-sm-6\">" +
                                "<div class=\"well text-center\">" +
                                    "<h1><i class=\"fa fa-times fa-3x text-danger\"></i></h1>" +
                                    "<h2>Errors</h2>" +
                                    "<h3 id=\"dashboard-errors\" class=\"text-danger\">0</h3>" +
                                "</div>" +
                            "</div>" +
                          "</div>";
            Html = result;
        }
    }
}
