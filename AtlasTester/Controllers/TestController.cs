using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AtlasTester.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AtlasTester.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        public string Host { get; set; } = "https://localhost";

        public string Port { get; set; } = "44321";

        public string Resource { get; set; } = "/api/v1/task/4165495";

        public string QueryString { get; set; } = string.Empty;

        public string QueryUrl { get; set; } = string.Empty;

        public IActionResult Index()
        {
            ViewData["Host"] = Host;
            ViewData["Port"] = Port;
            ViewData["Resource"] = Resource;
            ViewData["QueryString"] = QueryString;
            ViewData["Url"] = QueryUrl;

            return View();
        }

        public async Task<IActionResult> Tokens()
        {
            ViewData["IdToken"] = await HttpContext.GetTokenAsync("id_token");
            var exp = User.Claims.First(claim => claim.Type == "exp").Value;
            ViewData["IdTokenExpires"] = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(double.Parse(exp)).ToLocalTime().ToString();

            ViewData["RefreshToken"] = await HttpContext.GetTokenAsync("refresh_token");

            return View();
        }

        public async Task<IActionResult> Test(string host, string port, string resource, string queryString)
        {
            var idToken = await HttpContext.GetTokenAsync("id_token");

            Host = host;
            Port = port;
            Resource = resource;
            QueryString = queryString;

            ViewData["Host"] = Host;
            ViewData["Port"] = Port;
            ViewData["Resource"] = Resource;
            ViewData["QueryString"] = QueryString;
            ViewData["Message"] = "Test Get";

            using (var client = new HttpClient())
            {
                var urlString = Host + ":" + Port + Resource;

                if (!string.IsNullOrEmpty(QueryString))
                {
                    urlString += "?" + QueryString;
                }

                QueryUrl = urlString;
                var url = new Uri(urlString);
                ViewData["Url"] = url;

                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", idToken);

                var response = await client.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string json;
                    using (var content = response.Content)
                    {
                        json = await content.ReadAsStringAsync();
                    }

                    ViewData["Data"] = JValue.Parse(json).ToString(Formatting.Indented);
                }
                else
                {
                    ViewData["Data"] = $"Data failed to load (with status code {response.StatusCode})";
                }
            }

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}