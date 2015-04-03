using System;
using System.IO;
using System.Windows.Forms;
using SystemInfoSnapshot.Properties;

namespace SystemInfoSnapshot
{
    /// <summary>
    /// HTML Template
    /// </summary>
    public sealed class HTMLTemplate : IDisposable
    {
        #region Template placeholder Constants
        public const string VersionVar      = "<!--VERSION-->";
        public const string TitleVar        = "<!--[TITLE]-->";
        public const string SystemInfoVar   = "<!--[SYSTEMINFO]-->";
        public const string ProcessesVar    = "<!--[PROCESSES]-->";
        public const string ServicesVar     = "<!--[SERVICES]-->";
        public const string StartupVar      = "<!--[STARTUPAPPS]-->";
        public const string ProgramsVar     = "<!--[PROGRAMS]-->";
        #endregion

        #region Properties
        /// <summary>
        /// Gets the whole generated html
        /// </summary>
        public string TemplateHTML { get; private set; }

        /// <summary>
        /// Gets or sets the filename for the html file. .html will be appended.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets the last path for the generated file.
        /// </summary>
        public string LastSaveFilePath { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public HTMLTemplate()
        {
            TemplateHTML = Resources.template;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Write page title into template.
        /// </summary>
        /// <param name="html">Html to write.</param>
        public void WriteTitle(string html)
        {
            WriteFromVar(TitleVar, html);
        }

        /// <summary>
        /// Write system info into template.
        /// </summary>
        /// <param name="html">Html to write.</param>
        public void WriteSystemInfo(string html)
        {
            WriteFromVar(SystemInfoVar, html);
        }
        
        /// <summary>
        /// Write processes info into template.
        /// </summary>
        /// <param name="html">Html to write.</param>
        public void WriteProcesses(string html)
        {
            WriteFromVar(ProcessesVar, html);
        }
        
        /// <summary>
        /// Write services info into template.
        /// </summary>
        /// <param name="html">Html to write.</param>
        public void WriteServices(string html)
        {
            WriteFromVar(ServicesVar, html);
        }

        /// <summary>
        /// Write startup info into template.
        /// </summary>
        /// <param name="html">Html to write.</param>
        public void WriteStartup(string html)
        {
            WriteFromVar(StartupVar, html);
        }

        /// <summary>
        /// Write installed programs into template.
        /// </summary>
        /// <param name="html">Html to write.</param>
        public void WritePrograms(string html)
        {
            WriteFromVar(ProgramsVar, html);
        }

        /// <summary>
        /// Write in a placeholder variable into template.
        /// </summary>
        /// <param name="templateVar">Placeholder variable.</param>
        /// <param name="html">Html to write.</param>
        public void WriteFromVar(string templateVar, string html)
        {
            TemplateHTML = TemplateHTML.Replace(templateVar, html);
        }

        /// <summary>
        /// Write all generated HTML to a file and close the stream
        /// </summary>
        public void WriteToFile()
        {
            var filename = Filename ?? "SystemInfoSnapshot";
            filename += string.Format("_{0}.html", DateTime.Now).Replace(':', '-').Replace('/', '-').Replace(' ', '_');
#if !DEBUG
            filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), filename);
#endif
            TemplateHTML = TemplateHTML.Replace(VersionVar, ApplicationInfo.Version.ToString());
            using (var HtmlWriter = new StreamWriter(filename))
            {
                HtmlWriter.Write(TemplateHTML);
                HtmlWriter.Close();
            }

            LastSaveFilePath = filename;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Dispose and clean resources.
        /// </summary>
        public void Dispose()
        {
        }
        #endregion

    }
}
