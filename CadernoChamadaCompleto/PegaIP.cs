using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management;

namespace CadernoChamadaCompleto
{
    public class PegaIP
    {
        public string PegarIP()
        {
            List<string> ips = new List<string>();

            ManagementClass _mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection _moc = _mc.GetInstances();

            foreach (ManagementObject moc_ in _moc)
            {
                if (!(bool)moc_["ipEnabled"])
                {
                    continue;
                }

                string[] enderecoIP = (string[])moc_["IPAddress"];

                foreach (string sIP in enderecoIP)
                {
                    return sIP;
                }
            }

            return "";
        }
    }
}