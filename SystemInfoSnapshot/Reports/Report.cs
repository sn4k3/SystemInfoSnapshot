using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI;
using SystemInfoSnapshot.Components;

namespace SystemInfoSnapshot.Reports
{
    /// <summary>
    /// Represents a report
    /// </summary>
    public abstract class Report : IDisposable
    {
        #region Enums
        /// <summary>
        /// Represets all possible status for a report.
        /// </summary>
        public enum ReportStatus : byte
        {
            None = 0,
            Generating,
            Completed
        }
        #endregion

        #region Constants
        public const string TABLE_CLASS ="table table-striped table-bordered table-hover";
        public const string NOT_SUPPORTED = "This operating system is not suported yet!";
        #endregion

        #region Properties
        /// <summary>
        /// Html string
        /// </summary>
        //public string Html { get; protected set; }

        public string Html {
            get { return HtmlStringWriter.ToString(); } }

        public StringWriter HtmlStringWriter { get; private set; }

        public HtmlTextWriterEx HtmlWriter { get; private set; }

        /// <summary>
        /// Gets the report status
        /// </summary>
        public ReportStatus Status { get; private set; }

        /// <summary>
        /// Gets if this report worth from async
        /// </summary>
        public bool CanAsync { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        protected Report()
        {
            HtmlStringWriter = new StringWriter();
            HtmlWriter = new HtmlTextWriterEx(HtmlStringWriter);
            Status = ReportStatus.None;
            CanAsync = true;
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Gets the variable name to be replaced with the html result.
        /// </summary>
        /// <returns></returns>
        public abstract string GetTemplateVar();

        /// <summary>
        /// Build the report in Html.
        /// </summary>
        protected abstract void Build();


        #endregion

        #region Methods
        /// <summary>
        /// Generate the report.
        /// </summary>
        public void Generate()
        {
            if (Status == ReportStatus.Generating)
                return;

            HtmlStringWriter.GetStringBuilder().Clear();
            Status = ReportStatus.Generating;
            Build();
            Status = ReportStatus.Completed;
        }

        /// <summary>
        /// Get the html report and generate it if necessary.
        /// </summary>
        /// <param name="rebuild">True for rebuild a new report, otherwise false will return the Html.</param>
        /// <returns>Html value for the report.</returns>
        public string GetAndGenerate(bool rebuild = false)
        {
            if (rebuild)
            {
                Generate();
                return Html;
            }

            if (string.IsNullOrEmpty(Html))
            {
                Generate();
            }

            return Html;
        }

        public void WriteNotSupportedMsg()
        {
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.P);
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Strong);
            HtmlWriter.Write(NOT_SUPPORTED);
            HtmlWriter.RenderEndTag();
            HtmlWriter.RenderEndTag();
        }
        #endregion

        #region Static Methods

        /// <summary>
        /// Get all avaliable reports under <see cref="Reports"/> namespace.
        /// </summary>
        /// <returns>A list of avaliable reports.</returns>
        public static Report[] GetReports()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().
                    Where(t =>
                        (t.IsClass) &&
                        (!t.IsAbstract) &&
                        (String.Equals(t.Namespace, "SystemInfoSnapshot.Reports", StringComparison.Ordinal)) && 
                        (t.IsSubclassOf(typeof(Report)))
                ).ToArray();

            var reports = new Report[types.Length];
            for (var i = 0; i < types.Length; i++)
            {
                reports[i] = Activator.CreateInstance(types[i]) as Report;
            }

            return reports;
        }

        /// <summary>
        /// Generate all reports in a list.
        /// </summary>
        /// <param name="reports">List of reports to generate.</param>
        /// <param name="saveReport">True for save reports in the html file.</param>
        /// <param name="useSingleThread">True to generate the reports without parallel tasks</param>
        /// <returns><see cref="HtmlTemplate"/> with the reports already written in the template.</returns>
        public static HtmlTemplate GenerateReports(Report[] reports, bool saveReport = true, bool useSingleThread = false)
        {
            var htmlTemplate = new HtmlTemplate();
            //List<Report> asyncReports = new List<Report>();

            /*if (useSingleThread)
            {
                foreach (var report in reports)
                {
                    //if (report.CanAsync)
                    //{
                    //    asyncReports.Add(report);
                    //    continue;
                    //}
                    report.Generate();
                    htmlTemplate.WriteFromVar(report.GetTemplateVar(), report.Html);
                }
            }
            else
            {*/
#if DEBUG
            var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
#else
            var options = new ParallelOptions { MaxDegreeOfParallelism = useSingleThread ? 1 : -1 };
#endif
            Parallel.ForEach(reports, options, report =>
            {
                Debug.WriteLine(report.GetTemplateVar());
                report.Generate();

                lock (htmlTemplate)
                {
                    htmlTemplate.WriteFromVar(report.GetTemplateVar(), report.Html);
                }
            });

            /*foreach (var report in reports)
            {
                htmlTemplate.WriteFromVar(report.GetTemplateVar(), report.Html);
            }*/
            

            if (/*!ReferenceEquals(htmlTemplate, null) && */saveReport)
            {
                htmlTemplate.WriteToFile(); 
            }

            return htmlTemplate;
        }

        
        #endregion

        #region Overrides
        /// <summary>
        /// Gets a string representation of this class. <see cref="Html"/> is returned
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Html;
        }

        public void Dispose()
        {
            HtmlWriter.Dispose();
            HtmlStringWriter.Dispose();
        }

        #endregion
    }
}
