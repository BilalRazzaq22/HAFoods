using System;
using System.Linq;
using System.Management;
using System.Net;

namespace PrizeBondChecker.Core.CoreUtilities
{
    public static class CoreUtils
    {
        private static string _macAddress;

        public static string ConnectionString { get; set; }

        public static string GetMachineMacAddress()
        {
            if (!String.IsNullOrEmpty(_macAddress))
                return _macAddress;

            string macAddress = string.Empty;
            ManagementObjectSearcher MOS = null;

            try
            {
                //Code for retrieving Network Adapter Configuration
                MOS = new ManagementObjectSearcher(@"Select * From Win32_NetworkAdapterConfiguration");
                foreach (var mac in MOS.Get())
                {
                    macAddress = mac["MACAddress"] != null ? mac["MACAddress"].ToString() : null;
                    _macAddress = macAddress;
                    Console.WriteLine("{1} Mac Add:{0}", macAddress, DateTime.Now.ToLongTimeString());

                    if (!String.IsNullOrEmpty(macAddress) && !macAddress.Contains("O.E.M"))
                        break;
                }
            }
            catch (Exception)
            {

            }



            if (!String.IsNullOrEmpty(macAddress) && macAddress.Contains("O.E.M"))
            {
                macAddress = null;
                _macAddress = null;
            }

            else if (!string.IsNullOrEmpty(macAddress))
            {
                _macAddress = macAddress.Replace(".", "").Replace(":", "");
                return _macAddress;
            }


            try
            {

                //Code for retrieving Processor's Identity

                foreach (ManagementObject getPID in MOS.Get())
                {
                    macAddress = getPID["ProcessorID"] != null ? getPID["ProcessorID"].ToString() : null;

                }

                _macAddress = macAddress;

                Console.WriteLine("Processor Id:{0}", macAddress);

            }
            catch (Exception)
            {

            }



            if (!String.IsNullOrEmpty(macAddress) && macAddress.Contains("O.E.M"))
            {
                macAddress = null;
                _macAddress = null;
            }

            else if (!string.IsNullOrEmpty(macAddress))
            {
                _macAddress = macAddress;
                return _macAddress;
            }


            try
            {
                //Code for retrieving motherboard's serial number
                MOS = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
                foreach (var getserial in MOS.Get())
                {
                    macAddress = getserial["SerialNumber"] != null ? getserial["SerialNumber"].ToString() : null;
                }

            }
            catch (Exception)
            {

            }
            Console.WriteLine("MB Serial:{0}", macAddress);

            if (!String.IsNullOrEmpty(macAddress) && macAddress.Contains("O.E.M"))
            {
                macAddress = null;
                _macAddress = null;
            }

            else if (!string.IsNullOrEmpty(macAddress))
            {
                _macAddress = macAddress;
                return _macAddress;
            }

            try
            {
                var dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + "C" + @":""");
                dsk.Get();
                macAddress = dsk["VolumeSerialNumber"].ToString();
            }
            catch (Exception)
            {

            }
            if (!String.IsNullOrEmpty(macAddress) && macAddress.Contains("O.E.M"))
            {
                macAddress = null;
                _macAddress = null;
            }
            else if (!string.IsNullOrEmpty(macAddress))
            {
                _macAddress = macAddress;
                return _macAddress;
            }
            if (!String.IsNullOrEmpty(macAddress))
            {
                _macAddress = macAddress.Replace(".", "").Replace(":", "");
            }
            return _macAddress;
        }

        public static bool IsNumeric(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c) && c != '.')
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetIP(string hostName = "")
        {
            if (String.IsNullOrEmpty(hostName))
                hostName = Dns.GetHostName();

            // Find host by name
            var iphostentry = Dns.GetHostEntry(hostName);

            // Grab the first IP addresses
            String ipStr = "";
            foreach (var ipaddress in iphostentry.AddressList.OrderByDescending(o => o.Address))
            {
                ipStr = ipaddress.ToString();
                return ipStr;
            }
            return ipStr;
        }

    }
}
