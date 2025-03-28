using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Yomiage.Services
{
    class HttpRequest
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task<string> FetchHtmlAsync(string url)
        {
            try {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            } catch (Exception ex)
            {
                // TODO エラーハンドリング
                return string.Empty;
            }
        }
    }
}
