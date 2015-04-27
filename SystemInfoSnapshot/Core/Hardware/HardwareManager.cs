/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SystemInfoSnapshot.Core.Disk;
using SystemInfoSnapshot.Core.Process;
using OpenHardwareMonitor.Hardware;

namespace SystemInfoSnapshot.Core.Hardware
{
    public sealed class HardwareManager : IEnumerable<IHardware>
    {
        #region Properties
        /// <summary>
        /// Gets the computer hardware and sensors.
        /// </summary>
        public Computer Computer { get; private set; }

        /// <summary>
        /// Gets the disks detailed info and smart
        /// </summary>
        public DiskManager DiskManager { get; private set; }

        /// <summary>
        /// Gets the computer hardware and sensors.
        /// </summary>
        public IHardware[] Hardware
        {
            get { return Computer.Hardware;  }
        }

        /// <summary>
        /// Gets if the harware monitor is supported under the running environment.
        /// </summary>
        public bool IsSupported { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public HardwareManager(bool cpuEnabled = true, bool fanControllerEnabled = true, bool gpuEnabled = true, bool hddEnabled = true, bool mainboardEnabled = true, bool ramEnabled = true)
        {
            Computer = new Computer
            {
                CPUEnabled = cpuEnabled, 
                FanControllerEnabled = fanControllerEnabled,
                GPUEnabled = gpuEnabled,
                HDDEnabled = hddEnabled,
                MainboardEnabled = mainboardEnabled,
                RAMEnabled = ramEnabled
            };
            try
            {
                Computer.Open();
                IsSupported = true;
            }
            catch (Exception)
            {
                IsSupported = false;
            }
            DiskManager = new DiskManager();
        }
        #endregion

        #region Methods

        public string GetReport()
        {
            return Computer.GetReport();
        }

        public void Update()
        {
            DiskManager.Update();
        }

        #endregion

        #region Static Methods
        /// <summary>
        /// Gets a list of processes on this machine.
        /// </summary>
        /// <returns></returns>
        public static List<ProcessItem> GetProcesses()
        {
            return System.Diagnostics.Process.GetProcesses().Select(process => new ProcessItem(process)).OrderBy(item => -item.WorkingSet64).ToList();
        }

        
        #endregion

        #region Overrides
        public IEnumerator<IHardware> GetEnumerator()
        {
            return Computer.Hardware.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return Computer.GetReport();
        }

        #endregion
    }
}
