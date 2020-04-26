using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tmp5
{
    public static class MultiplayerConfig
    {

        public static int Width
        {
            set; get;
        }

        public static int Height
        {
            set; get;
        }

        public static string Name
        {
            set; get;
        }

        static string ip;
        public static string IP
        {
            get;set;
        }

        public static bool isHost { get; set; }

        public static bool ValidateData()
        {
            if (((Width >= 10 && Width <= 15) &&
                (Height >= 10 && Height <= 15) &&
                !string.IsNullOrEmpty(Name)) || (!string.IsNullOrEmpty(IP) && IsValidateIP(IP) && !string.IsNullOrEmpty(Name)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool IsValidateIP(string Address)
        {
            string Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

            Regex check = new Regex(Pattern);

            if (string.IsNullOrEmpty(Address))
                return false;
            else
                return check.IsMatch(Address, 0);
        }
    }
}
