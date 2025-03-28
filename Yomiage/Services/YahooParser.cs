using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Yomiage.Services
{
    class YahooParser
    {
        public static string? GetArticleUrlFromPickup(string pikcupHtml) {
            var doc = new HtmlDocument();
            doc.LoadHtml(pikcupHtml);

            var linkNode =  doc.DocumentNode.SelectNodes("//a")?.FirstOrDefault(a => a.InnerText.Contains("記事全文を読む"));
            return linkNode?.GetAttributeValue("href", "");
        }

        public static List<string> GetTexts(string html) {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var root = doc.DocumentNode.SelectSingleNode("//article");
            if (root == null) return [];
            var pNodes = root.SelectNodes(".//p");
            if (pNodes == null || pNodes.Count < 2) return new List<string>(); var result = new List<string>();

            foreach (var pNode in pNodes.Skip(1))
            {
                foreach (var aNode in pNode.SelectNodes(".//a") ?? Enumerable.Empty<HtmlNode>())
                {
                    aNode.Remove();
                }
                var parts = pNode.InnerHtml.Split(["<br>", "<br/>", "<br />"], StringSplitOptions.None);
                foreach (var part in parts)
                {
                    var splited = Regex.Split(HtmlEntity.DeEntitize(part.Trim()), @"(?<=。|、)").Where(s => s != "画像：tenki.jp" && !string.IsNullOrWhiteSpace(s))
                             .ToList();
                    result.AddRange(splited);
                }
            }
            return result;
        }
    }
}
