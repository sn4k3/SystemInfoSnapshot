using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Windows.Forms;
using SystemInfoSnapshot.Core.Disk;
using OpenHardwareMonitor.Hardware;

namespace SystemInfoSnapshot.Reports
{
    public sealed class Hardware : Report
    {
        public const string TemplateVar = "<!--[HARDWARE]-->";

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        private DiskManager diskManager;
        protected override void Build()
        {
            if (SystemHelper.IsWindows)
            {
                diskManager = new DiskManager();
            }
            Computer computer = new Computer { CPUEnabled = true, FanControllerEnabled = true, GPUEnabled = true, HDDEnabled = true, MainboardEnabled = true, RAMEnabled = true };
            try
            {
                computer.Open();
            }
            catch
            {
                WriteNotSupportedMsg();
                return;
            }


            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Id, "accordion");
            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "panel-group");
            HtmlWriter.AddAttribute("role", "tablist");
            HtmlWriter.AddAttribute("aria-multiselectable", "true");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "panel panel-default");
            HtmlWriter.AddAttribute("role", "tab");
            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Id, "headingOne");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "panel-heading");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "panel-title");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.H4);

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Href, "#collapseOne");
            HtmlWriter.AddAttribute("data-toggle", "collapse");
            HtmlWriter.AddAttribute("data-parent", "#accordion");
            HtmlWriter.AddAttribute("aria-expanded", "false");
            HtmlWriter.AddAttribute("aria-controls", "collapseOne");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.A);

            HtmlWriter.RenderTag(HtmlTextWriterTag.I, HtmlTextWriterAttribute.Class, "fa fa-file-text-o", string.Empty);
            HtmlWriter.Write(" Detailed text report");
            HtmlWriter.RenderEndTag(); // </a>
            HtmlWriter.RenderEndTag(); // </h4>
            HtmlWriter.RenderEndTag(); // </div>

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Id, "collapseOne");
            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "panel-collapse collapse");
            HtmlWriter.AddAttribute("role", "tabpanel");
            HtmlWriter.AddAttribute("aria-labelledby", "headingOne");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            HtmlWriter.RenderTag(HtmlTextWriterTag.Div, HtmlTextWriterAttribute.Class, "panel-body padding20", computer.GetReport().Replace(Environment.NewLine, string.Format("{0}<br>", Environment.NewLine)));

            HtmlWriter.RenderEndTag(); // </div>
            HtmlWriter.RenderEndTag(); // </div>
            HtmlWriter.RenderEndTag(); // </div>

            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "row");
            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

            /*var result = "<div class=\"panel-group\" id=\"accordion\" role=\"tablist\" aria-multiselectable=\"true\">" +
                            "<div class=\"panel panel-default\">" +
                               "<div class=\"panel-heading\" role=\"tab\" id=\"headingOne\">" +
                                   "<h4 class=\"panel-title\">" +
                                       "<a data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#collapseOne\" aria-expanded=\"false\" aria-controls=\"collapseOne\"><i class=\"fa fa-file-text-o\"></i> Detailed text report</a>" +
                                   "</h4>" +
                                "</div>" +
                                "<div id=\"collapseOne\" class=\"panel-collapse collapse\" role=\"tabpanel\" aria-labelledby=\"headingOne\">" +
                                    "<div class=\"panel-body padding20\">" +
                                        computer.GetReport().Replace(Environment.NewLine, Environment.NewLine + "<br>") +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                         "</div>" +
                         "<div class=\"row\">";*/
         
            RenderHardware(computer.Hardware);

            HtmlWriter.RenderEndTag(); // </div>
        }

        // Icons to render
        readonly Dictionary<HardwareType, string> HardwareIcon = new Dictionary<HardwareType, string>
            {
                {HardwareType.Mainboard, "fa fa-cloud fa-3x"},
                {HardwareType.SuperIO, "fa fa-cubes fa-3x"},
                {HardwareType.CPU, "fa fa-server fa-3x"},
                {HardwareType.RAM, "fa fa-database fa-3x"},
                {HardwareType.GpuNvidia, "fa fa-picture-o fa-3x"},
                {HardwareType.GpuAti, "fa fa-picture-o fa-3x"},
                {HardwareType.HDD, "fa fa-hdd-o fa-3x"},
            };

        readonly Dictionary<SensorType, string> SensorIcon = new Dictionary<SensorType, string>
            {
                {SensorType.Clock, "fa fa-clock-o fa-2x"},
                {SensorType.Control, "fa fa-tachometer fa-2x"},
                {SensorType.Data, "fa fa-table fa-2x"},
                {SensorType.Factor, "fa fa-picture-o fa-2x"},
                {SensorType.Fan, "fa fa-spinner fa-2x"},
                {SensorType.Flow, "fa fa-stack-overflow fa-2x"},
                {SensorType.Level, "fa fa-flask fa-2x"},
                {SensorType.Load, "fa fa-tasks fa-2x"},
                {SensorType.Power, "fa fa-power-off fa-2x"},
                {SensorType.Temperature, "fa fa-fire fa-2x"},
                {SensorType.Voltage, "fa fa-bolt fa-2x"},
            };

        readonly Dictionary<SensorType, string> SensorUnit = new Dictionary<SensorType, string>
            {
                {SensorType.Clock, "MHz"},
                {SensorType.Control, "%"},
                {SensorType.Data, "GB"},
                {SensorType.Factor, ""},
                {SensorType.Fan, "RPM"},
                {SensorType.Flow, ""},
                {SensorType.Level, ""},
                {SensorType.Load, "%"},
                {SensorType.Power, "W"},
                {SensorType.Temperature, "ºC"},
                {SensorType.Voltage, "V"},
            };

        /// <summary>
        /// Gets a formated string from a collection of <see cref="IHardware"/>
        /// </summary>
        /// <param name="ihardware"></param>
        private void RenderHardware(IEnumerable<IHardware> ihardware)
        {
            foreach (var hardware in ihardware)
            {
                hardware.Update();

                if (hardware.HardwareType == HardwareType.HDD)
                {
                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("col-sm-12 col-md-12 col-lg-6 col-xl-4 hardware_{0}", hardware.HardwareType.ToString().ToLower()));
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                }
                else
                {
                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "col-sm-12");
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                }
                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "well text-center");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                
                if (HardwareIcon.ContainsKey(hardware.HardwareType))
                {
                    if (HardwareIcon.ContainsKey(hardware.HardwareType))
                    {
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.H1);
                        HtmlWriter.RenderTag(HtmlTextWriterTag.I, HtmlTextWriterAttribute.Class, HardwareIcon[hardware.HardwareType], string.Empty);
                        HtmlWriter.RenderEndTag();
                    }
                }

                HtmlWriter.RenderTag(HtmlTextWriterTag.H2, hardware.HardwareType.ToString());
                HtmlWriter.RenderTag(HtmlTextWriterTag.H3, hardware.Name);
                HtmlWriter.RenderTag(HtmlTextWriterTag.P, "&nbsp;");

                SensorType? lastSensorType = null;
                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                foreach (var sensor in hardware.Sensors)
                {
                    if (hardware.Sensors.Length > 3 && lastSensorType.HasValue &&
                        lastSensorType.Value != sensor.SensorType)
                    {
                        HtmlWriter.RenderEndTag(); // </div>
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Div, HtmlTextWriterAttribute.Class, "clearfix", string.Empty);
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                    }
                    lastSensorType = sensor.SensorType;
                    if (hardware.HardwareType == HardwareType.HDD)
                    {
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "col-sm-6");
                    }
                    else
                    {
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "col-sm-4 col-md-3 col-lg-2");
                    }

                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                    HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "well");
                    HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

                    if (SensorIcon.ContainsKey(sensor.SensorType))
                    {
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.H1);
                        HtmlWriter.RenderTag(HtmlTextWriterTag.I, HtmlTextWriterAttribute.Class, SensorIcon[sensor.SensorType], string.Empty);
                        HtmlWriter.RenderEndTag();
                    }

                    HtmlWriter.Write("{0} {1} = <strong>{2}{3}</strong>", 
                        sensor.Name,
                        sensor.Name.Contains(sensor.SensorType.ToString()) ? string.Empty : sensor.SensorType.ToString(),
                        sensor.Value.HasValue ? sensor.Value.Value.ToString("#.##") : "no value",
                        SensorUnit[sensor.SensorType]);

                    // Print a progress bar for some sensor types.
                    if (sensor.Value.HasValue && (sensor.SensorType == SensorType.Temperature || sensor.SensorType == SensorType.Load || sensor.SensorType == SensorType.Control))
                    {
                        HtmlWriter.RenderTag(HtmlTextWriterTag.P, string.Empty);
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "progress");
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

                        string progressbarType;
                        if (sensor.Value >= 80)
                        {
                            progressbarType = "danger";
                        }
                        else if (sensor.Value >= 65)
                        {
                            progressbarType = "warning";
                        }
                        else if (sensor.Value >= 50)
                        {
                            progressbarType = "info";
                        }
                        else
                        {
                            progressbarType = "success";
                        }
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "progress-bar");
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, string.Format("progress-bar-{0}", progressbarType));
                        HtmlWriter.AddAttribute("role", "progressbar");
                        HtmlWriter.AddAttribute("aria-valuenow", ((int)sensor.Value).ToString());
                        HtmlWriter.AddAttribute("aria-valuemin", "0");
                        HtmlWriter.AddAttribute("aria-valuemax", "100");
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("width:{0}%", (int)sensor.Value));
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

                        HtmlWriter.RenderTag(HtmlTextWriterTag.Strong, string.Format("{0}{1}", sensor.Value.Value.ToString("#.##"), SensorUnit[sensor.SensorType]));
                        HtmlWriter.RenderEndTag(); // </div>
                        HtmlWriter.RenderEndTag(); // </div>
                    }

                    HtmlWriter.RenderEndTag(); // </div>
                    HtmlWriter.RenderEndTag(); // </div>
                }

                RenderHardware(hardware.SubHardware);

                if (hardware.HardwareType == HardwareType.HDD && SystemHelper.IsWindows)
                {
                    var disk = diskManager[hardware.Name];
                    if (!ReferenceEquals(disk, null))
                    {
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-left");
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);

                        // Disk Info
                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

                        HtmlWriter.RenderTag(HtmlTextWriterTag.Caption, HtmlTextWriterAttribute.Class, "text-center", "<h3>Information</h3>");

                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                        HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Property");
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Value");

                        HtmlWriter.RenderEndTag(); // </tr>
                        HtmlWriter.RenderEndTag(); // </thead>
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

                        var props = disk.GetType().GetProperties();
                        foreach (var propertyInfo in props)
                        {
                            if (propertyInfo.Name.Equals("Attributes"))
                                continue;
                            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                            HtmlWriter.RenderTag(HtmlTextWriterTag.Td, propertyInfo.Name);
                            HtmlWriter.RenderTag(HtmlTextWriterTag.Td, propertyInfo.GetValue(disk).ToString());
                            HtmlWriter.RenderEndTag();
                        }

                        HtmlWriter.RenderEndTag(); // </tbody>
                        HtmlWriter.RenderEndTag(); // </table>


                        // Disk SMART

                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, TABLE_CLASS);
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Table);

                        HtmlWriter.RenderTag(HtmlTextWriterTag.Caption, HtmlTextWriterAttribute.Class, "text-center", "<h3>S.M.A.R.T.</h3>");

                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);

                        HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Attribute");
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Current");
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Worst");
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Threshold");
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Th, "Raw");
                        HtmlWriter.RenderTag(HtmlTextWriterTag.Th, HtmlTextWriterAttribute.Class, "text-center", "Status");

                        HtmlWriter.RenderEndTag(); // </tr>
                        HtmlWriter.RenderEndTag(); // </thead>

                        HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);

                        foreach (var attribute in disk.Attributes)
                        {
                            if (!attribute.Value.HasData)
                                continue;

                            if (!attribute.Value.IsOK)
                            {
                                HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "danger");
                            }
                            else
                            {
                                if (attribute.Value.Threshold > 0 && !attribute.Value.AttributeName.Contains("time"))
                                {
                                    if (attribute.Value.Raw > 0)
                                    {
                                        if (attribute.Value.Raw >= attribute.Value.Threshold)
                                        {
                                            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "danger");
                                        }
                                        else
                                        {
                                            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "warning");
                                        }
                                    }
                                }
                                else
                                {
                                    if (attribute.Value.IsCritical && attribute.Value.Raw > 0)
                                    {
                                        HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "warning");
                                    }
                                }
                            }

                            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                            HtmlWriter.RenderTag(HtmlTextWriterTag.Td, (attribute.Value.IsCritical ? "<span class=\"text-danger\">*</span> " : String.Empty) + attribute.Value.AttributeName);
                            HtmlWriter.RenderTag(HtmlTextWriterTag.Td, attribute.Value.Current.ToString());
                            HtmlWriter.RenderTag(HtmlTextWriterTag.Td, attribute.Value.Worst.ToString());
                            HtmlWriter.RenderTag(HtmlTextWriterTag.Td, attribute.Value.Threshold.ToString());
                            HtmlWriter.RenderTag(HtmlTextWriterTag.Td, attribute.Value.Raw.ToString());

                            HtmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "text-center isok");
                            HtmlWriter.AddAttribute("data-order", Convert.ToByte(attribute.Value.IsOK).ToString());
                            HtmlWriter.RenderBeginTag(HtmlTextWriterTag.Td);
                            HtmlWriter.Write(attribute.Value.IsOK
                                ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>"
                                : "<span class=\"glyphicon glyphicon-remove text-error\"></span>");
                            HtmlWriter.RenderEndTag();

                            HtmlWriter.RenderEndTag();
                        }

                        HtmlWriter.RenderEndTag(); // </tbody>
                        HtmlWriter.RenderEndTag(); // </table>

                        HtmlWriter.RenderTag(HtmlTextWriterTag.P, "<span class=\"text-danger\">*</span> Potential indicators of imminent electromechanical failure.");

                        if (disk.Status.Equals("Pred Fail"))
                        {
                            HtmlWriter.RenderTag(HtmlTextWriterTag.Div, HtmlTextWriterAttribute.Class, "alert alert-danger", "<strong>This drive is failing and/or in bad condition! Consider making a backup of whole disk or your critical data and replace this disk.<br>" +
                                                                                                                             "If the health keeps declining you will not be able to recover the disk at some point.</strong>");
                        }

                        HtmlWriter.RenderEndTag(); // </div>
                    }
                }

                HtmlWriter.RenderEndTag(); // </div>
                HtmlWriter.RenderEndTag(); // </div>
                HtmlWriter.RenderEndTag(); // </div>
            }
        }
    }
}
