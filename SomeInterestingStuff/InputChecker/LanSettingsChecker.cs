using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeInterestingStuff.InputChecker
{
    public static class LanSettingsChecker
    {
        /// <summary>
        /// Returns true if Port number is in range 0 - 65535.
        /// </summary>
        /// <param name="Port"></param>
        /// <returns></returns>
        public static bool IsValidPort(int Port)
        {
            return (Port >= 0 && Port <= 65535);
        }

        /// <summary>
        /// Returns true if IP string matches IP pattern.
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public static bool IsValidMAC(string Address)
        {
            Address = Address.Replace(" ", "").Replace(":", "").Replace("-", "");

            // Match pattern for MAC address
            // Matches:12-23-34-45-56-67, 12:23:34:45:56:67, 122334455667
            // But not:12:34-4556-67
            string Pattern = @"^(?:[0-9a-fA-F]{2}:){5}[0-9a-fA-F]{2}|(?:[0-9a-fA-F]{2}-){5}[0-9a-fA-F]{2}|(?:[0-9a-fA-F]{2}){5}[0-9a-fA-F]{2}$";
            //Regular Expression object
            System.Text.RegularExpressions.Regex check =
                new System.Text.RegularExpressions.Regex(Pattern);

            //check to make sure an ip address was provided    
            if (string.IsNullOrEmpty(Address))
                //returns false if IP is not provided    
                return false;
            else
                //Matching the pattern    
                return check.IsMatch(Address, 0);
        }

        /// <summary>
        /// Returns true if MAC string matches MAC pattern.
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public static bool IsValidIP(string Address)
        {
            //Match pattern for IP address    
            string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            //Regular Expression object    
            System.Text.RegularExpressions.Regex check =
                new System.Text.RegularExpressions.Regex(Pattern);

            //check to make sure an ip address was provided    
            if (string.IsNullOrEmpty(Address))
                //returns false if IP is not provided    
                return false;
            else
                //Matching the pattern    
                return check.IsMatch(Address, 0);
        }
    }
}
