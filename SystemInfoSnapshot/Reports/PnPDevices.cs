namespace SystemInfoSnapshot.Reports
{
    public sealed class PnPDevices : Report
    {
        public const string TemplateVar = "<!--[PNPDEVICES]-->";

        public override string GetTemplateVar()
        {
            return TemplateVar;
        }

        protected override void Build()
        {
            var devices = Devices.GetPnPDevices();
            var result = "<table data-sortable class=\"table table-striped table-bordered table-responsive table-hover sortable-theme-bootstrap\">" +
                            "<thead>" +
                                "<tr>" +
                                "<th>#</th>" +
                                "<th>Device ID</th>" +
                                "<th>Description</th>" +
                                "<th>Manufacturer</th>" +
                                "</tr>" +
                            "</thead>" +
                            "<tbody>";
            var i = 0;
            foreach (var device in devices)
            {
                i++;
                result += "<tr>" +
                            "<td class=\"index\">" + i + "</td>" +
                            "<td class=\"deviceid\">" + device.DeviceID + "</td>" +
                            "<td class=\"description\">" + device.Description + "</td>" +
                            "<td class=\"manufacturer\">" + device.Manufacturer + "</td>" +
                           "</tr>";


            }
            result += "</tbody></table>";
            Html = result;
        }
    }
}
