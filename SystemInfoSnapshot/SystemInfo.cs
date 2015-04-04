using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using Microsoft.Win32;
using OpenHardwareMonitor.Hardware;

namespace SystemInfoSnapshot
{
    public sealed class SystemInfo
    {
        public static string GetTitleHTML()
        {
            return string.Format("{0} - {1}", Environment.MachineName, DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        public static string GetProcessesHTML()
        {
            var result = "<table data-sortable class=\"table table-striped table-bordered table-responsive table-hover sortable-theme-bootstrap\">" +
                         "<thead>" +
                         "<tr>" +
                         "<th>#</th>" +
                         "<th>PID</th>" +
                         //"<th>Title</th>" +
                         "<th>Name</th>" +
                         //"<th>Window Title</th>" +
                         "<th>File</th>" +
                         //"<th>Machine Name</th>" +
                         "<th>Up Time</th>" +
                         //"<th>CPU</th>" +
                         "<th>Threads</th>" +
                         "<th>Memory (MB)</th>" +
                         "<th>Peak Memory (MB)</th>" +
                         "</tr>" +
                         "</thead>" +
                         "<tbody>";
            var i = 0;
            foreach (var process in Process.GetProcesses())
            {
                i++;
                
                try
                {
                    var processinfo = ProcessHelper.GetProcessInfo(process);
                    result += "<tr>" +
                              "<td class=\"index\">" + i + "</td>" +
                              "<td class=\"pid\">" + process.Id + "</td>" +
                        //"<td>" + proccess.Modules[0].ModuleName + "</td>" +
                              "<td class=\"name\">" + process.ProcessName + "</td>" +
                              //"<td>" + process.MainWindowTitle + "</td>" +
                              "<td class=\"file\">" + (processinfo[ProcessHelper.ExecutablePath] ?? string.Empty) + "</td>" +
                        //"<td>" + proccess.MachineName + "</td>" +
                        "<td class=\"uptime\">" + (processinfo[ProcessHelper.StartTime] != null ? DateTime.Now.Subtract(ManagementDateTimeConverter.ToDateTime(processinfo[ProcessHelper.StartTime].ToString())).ToString() : string.Empty) + "</td>" +
                        //"<td>" + process.UserProcessorTime + "</td>" +
                              "<td class=\"threads\">" + process.Threads.Count + "</td>" +
                              "<td class=\"memory\" data-value=\"" + process.WorkingSet64 + "\">" + (process.WorkingSet64 / 1024.0 / 1024.0).ToString("#.##") + "</td>" +
                              "<td class=\"peakmemory\" data-value=\"" + process.PeakWorkingSet64 + "\">" + (process.PeakWorkingSet64 / 1024.0 / 1024.0).ToString("#.##") + "</td>" +
                              "</tr>";
                }
                catch (Exception)
                {
                }
                
            }
            result += "</tbody></table>";
            return result;
        }

        public static string GetServicesHTML()
        {
            // get list of Windows services
            ServiceController[] services = ServiceController.GetServices();
            
            var result = "<table data-sortable class=\"table table-striped table-bordered table-responsive table-hover sortable-theme-bootstrap\">" +
                         "<thead>" +
                         "<tr>" +
                         "<th>#</th>" +
                         "<th>Display Name</th>" +
                         "<th>Service Name</th>" +
                         "<th>Service Type</th>" +
                         //"<th>Machine Name</th>" +
                         "<th width=\"180\" class=\"text-center\">Can Pause/Continue</th>" +
                         "<th width=\"180\" class=\"text-center\">Can Shutdown</th>" +
                         "<th width=\"180\" class=\"text-center\">Can Stop</th>" +
                         "<th>Status</th>" +
                         "</tr>" +
                         "</thead>" +
                         "<tbody>";

            var i = 0;
            // try to find service name
            foreach (ServiceController service in services)
            {
                i++;
                result += "<tr>" +
                          "<td class=\"index\">" + i + "</td>" +
                          "<td class=\"displayname\">" + service.DisplayName + "</td>" +
                          "<td class=\"servicename\">" + service.ServiceName + "</td>" +
                          "<td>" + service.ServiceType + "</td>" +
                          //"<td>" + service.MachineName + "</td>" +
                          "<td class=\"text-center canpause\" data-value=\"" + Convert.ToByte(service.CanPauseAndContinue) + "\">" + (service.CanPauseAndContinue ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>" : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                          "<td class=\"text-center canshutdown\" data-value=\"" + Convert.ToByte(service.CanShutdown) + "\">" + (service.CanShutdown ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>" : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                          "<td class=\"text-center canstop\" data-value=\"" + Convert.ToByte(service.CanStop) + "\">" + (service.CanStop ? "<span class=\"glyphicon glyphicon-ok text-success\"></span>" : "<span class=\"glyphicon glyphicon-remove text-error\"></span>") + "</td>" +
                          "<td class=\"";
                switch (service.Status)
                {
                    case ServiceControllerStatus.Paused:
                        result += "text-warning";
                        break;
                    case ServiceControllerStatus.StartPending:
                        result += "text-primary";
                        break;
                    case ServiceControllerStatus.Running:
                        result += "text-success";
                        break;
                    case ServiceControllerStatus.Stopped:
                        result += "text-danger";
                        break;
                }

                result += " status\">"+service.Status+"</td>" +
                          "</tr>";
                
            }
            result += "</tbody></table>";

            return result;
        }

        public static string GetStartupHTML()
        {
            var result = "<table data-sortable class=\"table table-striped table-bordered table-responsive table-hover sortable-theme-bootstrap\">" +
                         "<thead>" +
                         "<tr>" +
                         "<th>#</th>" +
                         "<th>Key</th>" +
                         "<th>Program</th>" +
                         "<th>File</th>" +
                         "</tr>" +
                         "</thead>" +
                         "<tbody>";
            var i = 0;
            var autoruns = Autorun.GetAutoruns();
            foreach (var autorun in autoruns)
            {
                i++;
                result += "<tr>" +
                            "<td class=\"index\">" + i + "</td>" +
                            "<td class=\"key\">" + autorun.Key + "</td>" +
                            "<td class=\"program\">" + autorun.Name + "</td>" +
                            "<td class=\"file\">" + autorun.Path + "</td>" +
                            "</tr>";
            }
            

            result += "</tbody></table>";
            return result;
        }

        public static string GetProgramsHTML()
        {
            var result = "<table data-sortable class=\"table table-striped table-bordered table-responsive table-hover sortable-theme-bootstrap\">" +
                         "<thead>" +
                         "<tr>" +
                         "<th>#</th>" +
                         "<th>Program</th>" +
                         "<th>Version</th>" +
                         "<th>Publisher</th>" +
                         "<th>Install Date</th>" +
                         "</tr>" +
                         "</thead>" +
                         "<tbody>";
            var i = 0;
            var programs = InstalledProgram.GetInstalledPrograms();
            foreach (var program in programs)
            {
                i++;
                result += "<tr>" +
                            "<td class=\"index\">" + i + "</td>" +
                            "<td class=\"program\">" + program.Name + "</td>" +
                            "<td class=\"version\">" + program.Version + "</td>" +
                            "<td class=\"publisher\">" + program.Publisher + "</td>" +
                            "<td class=\"installdate\">" + program.InstallDate + "</td>" +
                            "</tr>";


            }
            result += "</tbody></table>";
            return result;
        }


        public static string GetSystemInfoHTML()
        {
            

            Dictionary<HardwareType, string> HardwareIcon = new Dictionary<HardwareType, string>
            {
                {HardwareType.Mainboard, "fa fa-cloud fa-3x"},
                {HardwareType.CPU, "fa fa-server fa-3x"},
                {HardwareType.RAM, "fa fa-database fa-3x"},
                {HardwareType.GpuNvidia, "fa fa-picture-o fa-3x"},
                {HardwareType.GpuAti, "fa fa-picture-o fa-3x"},
                {HardwareType.HDD, "fa fa-hdd-o fa-3x"},
            };
            Dictionary<SensorType, string> SensorIcon = new Dictionary<SensorType, string>
            {
                {SensorType.Clock, "fa fa-clock-o fa-2x"},
                {SensorType.Control, "fa fa-circle-o-notch fa-2x"},
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
            Dictionary<SensorType, string> SensorUnit = new Dictionary<SensorType, string>
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
            Computer computer = new Computer{CPUEnabled = true, FanControllerEnabled = true, GPUEnabled = true, HDDEnabled = true, MainboardEnabled = true, RAMEnabled = true};
            computer.Open();

            var result = "<div class=\"panel-group\" id=\"accordion\" role=\"tablist\" aria-multiselectable=\"true\">"+
                            "<div class=\"panel panel-default\">" +
                               "<div class=\"panel-heading\" role=\"tab\" id=\"headingOne\">" +
                                   "<h4 class=\"panel-title\">" +
                                       "<a data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#collapseOne\" aria-expanded=\"false\" aria-controls=\"collapseOne\"><i class=\"fa fa-file-text-o\"></i> Detailed text report</a>" +
                                   "</h4>" +
                                "</div>" +
                                "<div id=\"collapseOne\" class=\"panel-collapse collapse\" role=\"tabpanel\" aria-labelledby=\"headingOne\">" +
                                    "<div class=\"panel-body padding20\">" +
                                        computer.GetReport().Replace(Environment.NewLine, Environment.NewLine+"<br>") +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                         "</div>" +
                         "<div class=\"row\">";
            result += renderHardware(computer.Hardware);
            result += "</div>";
            return result;
        }

        private static string renderHardware(IHardware[] ihardware)
        {
            Dictionary<HardwareType, string> HardwareIcon = new Dictionary<HardwareType, string>
            {
                {HardwareType.Mainboard, "fa fa-cloud fa-3x"},
                {HardwareType.SuperIO, "fa fa-cubes fa-3x"},
                {HardwareType.CPU, "fa fa-server fa-3x"},
                {HardwareType.RAM, "fa fa-database fa-3x"},
                {HardwareType.GpuNvidia, "fa fa-picture-o fa-3x"},
                {HardwareType.GpuAti, "fa fa-picture-o fa-3x"},
                {HardwareType.HDD, "fa fa-hdd-o fa-3x"},
            };
            Dictionary<SensorType, string> SensorIcon = new Dictionary<SensorType, string>
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
            Dictionary<SensorType, string> SensorUnit = new Dictionary<SensorType, string>
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

                    if (sensor.Value.HasValue && (sensor.SensorType == SensorType.Temperature || sensor.SensorType == SensorType.Load || sensor.SensorType == SensorType.Control))
                    {
                        string progressbarType = "info";
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
                        result += "<p></p><div class=\"progress\">" +
                                  "<div class=\"progress-bar progress-bar-" + progressbarType + "\" role=\"progressbar\" aria-valuenow=\"" + (int)sensor.Value +
                                  "\" aria-valuemin=\"0\" aria-valuemax=\"100\" style=\"width: " + (int)sensor.Value +
                                  "%;\"><strong>" + sensor.Value.Value.ToString("#.##") + "%</strong></div></div>";
                    }

                    result += "</div></div>";
                }

                result += renderHardware(hardware.SubHardware);

                result += "</div></div></div>";
            }
            return result;
        }
    }
}
