using System;
using System.Collections.Generic;
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

        protected override void Build()
        {
            Computer computer = new Computer { CPUEnabled = true, FanControllerEnabled = true, GPUEnabled = true, HDDEnabled = true, MainboardEnabled = true, RAMEnabled = true };
            computer.Open();

            var result = "<div class=\"panel-group\" id=\"accordion\" role=\"tablist\" aria-multiselectable=\"true\">" +
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
                         "<div class=\"row\">";
            result += RenderHardware(computer.Hardware);
            result += "</div>";
            Html = result;
        }

        /// <summary>
        /// Gets a formated string from a collection of <see cref="IHardware"/>
        /// </summary>
        /// <param name="ihardware"></param>
        /// <returns>A string with formated html.</returns>
        private static string RenderHardware(IEnumerable<IHardware> ihardware)
        {
            // Icons to render
            var HardwareIcon = new Dictionary<HardwareType, string>
            {
                {HardwareType.Mainboard, "fa fa-cloud fa-3x"},
                {HardwareType.SuperIO, "fa fa-cubes fa-3x"},
                {HardwareType.CPU, "fa fa-server fa-3x"},
                {HardwareType.RAM, "fa fa-database fa-3x"},
                {HardwareType.GpuNvidia, "fa fa-picture-o fa-3x"},
                {HardwareType.GpuAti, "fa fa-picture-o fa-3x"},
                {HardwareType.HDD, "fa fa-hdd-o fa-3x"},
            };
            var SensorIcon = new Dictionary<SensorType, string>
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
            var SensorUnit = new Dictionary<SensorType, string>
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

            var result = string.Empty;
            foreach (var hardware in ihardware)
            {
                hardware.Update();

                if (hardware.HardwareType == HardwareType.HDD)
                {
                    result += "<div class=\"col-sm-6 col-md-4 col-lg-4\">";
                }
                else
                {
                    result += "<div class=\"col-sm-12\">";
                }
                result += "<div class=\"well text-center\">";
                if (HardwareIcon.ContainsKey(hardware.HardwareType))
                {
                    result += "<h1><i class=\"" + HardwareIcon[hardware.HardwareType] + "\"></i></h1>";
                }

                result += string.Format("<h2>{0}</h2><h3>{1}</h3><p>&nbsp;</p>", hardware.HardwareType, hardware.Name);

                SensorType? lastSensorType = null;
                result += "<div class=\"row\">";
                foreach (var sensor in hardware.Sensors)
                {
                    if (hardware.Sensors.Length > 3 && lastSensorType.HasValue && lastSensorType.Value != sensor.SensorType)
                        result += "</div><div class=\"clearfix\"></div><div class=\"row\">";
                    lastSensorType = sensor.SensorType;
                    if (hardware.HardwareType == HardwareType.HDD)
                    {
                        result += "<div class=\"col-sm-6\"><div class=\"well\">";
                    }
                    else
                    {
                        result += "<div class=\"col-sm-4 col-md-3 col-lg-2\"><div class=\"well\">";
                    }

                    if (SensorIcon.ContainsKey(sensor.SensorType))
                    {
                        result += "<h1><i class=\"" + SensorIcon[sensor.SensorType] + "\"></i></h1>";
                    }
                    result += string.Format("{0} {1} = <strong>{2}{3}</strong>",
                        sensor.Name,
                        sensor.Name.Contains(sensor.SensorType.ToString()) ? string.Empty : sensor.SensorType.ToString(),
                        sensor.Value.HasValue ? sensor.Value.Value.ToString("#.##") : "no value",
                        SensorUnit[sensor.SensorType]);

                    // Print a progress bar for some sensor types.
                    if (sensor.Value.HasValue && (sensor.SensorType == SensorType.Temperature || sensor.SensorType == SensorType.Load || sensor.SensorType == SensorType.Control))
                    {
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
                        result += "<p></p>" +
                                  "<div class=\"progress\">" +
                                    "<div class=\"progress-bar progress-bar-" + progressbarType + "\" role=\"progressbar\" aria-valuenow=\"" + (int)sensor.Value +
                                    "\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width: " + (int)sensor.Value + "%;\">" +
                                        "<strong>" + sensor.Value.Value.ToString("#.##") + SensorUnit[sensor.SensorType] + "</strong>" +
                                    "</div>" +
                                  "</div>";
                    }

                    result += "</div></div>";
                }

                result += RenderHardware(hardware.SubHardware);

                result += "</div></div></div>";
            }
            return result;
        }
    }
}
