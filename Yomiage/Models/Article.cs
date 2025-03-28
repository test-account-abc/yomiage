using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yomiage.Models
{
    public class Article
    {
        public string Id { get; }
        public string Url { get; set; }
        public string Html { get; set; }
        public string Title { get; }

        public Article(string url, string html, string title) {
            Id = Guid.NewGuid().ToString();
            Url = url;
            Html = html;
            Title = title;
        }
    }
}
