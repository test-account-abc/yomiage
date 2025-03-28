using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Yomiage.Services
{
    class UrlParser
    {
        public static bool IsYahooPickupUrl(string url) {
            return Regex.IsMatch(url, @"^https:\/\/news\.yahoo\.co\.jp\/pickup\/\d+$");
        }

        public static bool IsYahooArticleUrl(string url)
        {
            return Regex.IsMatch(url, @"^https:\/\/news\.yahoo\.co\.jp\/articles\/\w+$");
        }
    }
}
