using System;
using System.Net;
 
namespace CTGroup.Utility
{
   public class HostService
    {
        public static string GetIP()
        {
            return Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
        }

        public static string GetLocalIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string nIpAddress = string.Empty;
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }

                nIpAddress = context.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                nIpAddress = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            }

            return nIpAddress;
        }
    }
}
