/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */
using System;
using System.IO;
using SystemInfoSnapshot.Properties;

namespace SystemInfoSnapshot
{
    /// <summary>
    /// HTML Template
    /// </summary>
    public sealed class HtmlTemplate
    {
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
        public HtmlTemplate()
        {
            TemplateHTML = Resources.template;
        }
        #endregion

        #region Methods
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
            var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            
            filename = Directory.Exists(path) ? Path.Combine(path, filename) : filename;
#endif
            using (var htmlWriter = new StreamWriter(filename))
            {
                htmlWriter.Write(TemplateHTML);
                htmlWriter.Close();
            }

            LastSaveFilePath = filename;
        }

        public static string NormalizeBigString(string text)
        {
            const uint maxLength = 50;
            if (text.Length < maxLength)
            {
                return text;
            }
            if (text.Split(' ').Length > 1)
                return text;


            while (text.Length > maxLength)
            {
                int mid = text.Length/2;
                text = text.Remove(mid);
            }

            text = text.Insert(text.Length / 2, "(...)");

            return text;
        }

        /// <summary>
        /// Show and select generated html file in explorer.
        /// </summary>
        public void ShowInExplorer()
        {
            if (string.IsNullOrEmpty(Program.HtmlTemplate.LastSaveFilePath)) return;
            ProcessHelper.ShowInExplorer(Program.HtmlTemplate.LastSaveFilePath);
        }

        /// <summary>
        /// Show and select generated html file in explorer.
        /// </summary>
        public void OpenInDefaultBrowser()
        {
            if (string.IsNullOrEmpty(Program.HtmlTemplate.LastSaveFilePath)) return;
            ProcessHelper.Open(Program.HtmlTemplate.LastSaveFilePath);
        }
        #endregion
    }
}
