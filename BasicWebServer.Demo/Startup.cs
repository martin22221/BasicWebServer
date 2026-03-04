using BasicWebServer.Server;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;
using BasicWebServer.Server.Views;
using System.Text;
using System.Web;

namespace BasicWebServer.Demo
{
    public class Startup
    {
        private static string Filename = "content.txt";
        private const string Username = "user";
        private const string Password = "user123";

        static async Task Main(string[] args)
        {
            await DownloadSitesAsTextFile(Filename, new string[]
            {
                "https://www.aboutyou.com",
                "https://www.google.com",
                "https://www.github.com"
            });
            var server = new HttpServer(routes =>


            routes.MapGet("/", new HtmlResponse(FormView.HTML))
            .MapGet("/redirect", new RedirectResponse("https://www.aboutyou.com"))
            .MapPost("/HTML", new TextResponse("", AddFormDataAction))
            .MapGet("/content", new HtmlResponse(DownloadForm.Html))
            .MapGet("/cookies", new HtmlResponse("", AddCookiesAction))
            .MapPost("/content", new TextFileResponse(Filename))
            .MapGet("/session", new HtmlResponse("", DisplaySessionInfoAction))
            .MapGet("/login", new HtmlResponse(LoginForm.Html))
            .MapPost("/Login", new HtmlResponse("", LoginAction))
            .MapGet("/logout", new HtmlResponse("", LogoutAction))
            .MapGet("/UserProfile", new HtmlResponse("", GetUserDataAction))
            );

            await server.Start();
        }
        private static void AddFormDataAction(Request request, Response response)
        {
            response.Body = "";
            foreach (var (key, value) in request.Form)
            {
                response.Body += $"{key} - {value}";
                response.Body += Environment.NewLine;

            }
        }



        private static async Task<string> DownloadWebSiteContent(string url)
        {
            var client = new HttpClient();

            using (client)
            {
                var response = await client.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();
                return html;
            }
        }

        private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();
            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContent(url));
            }

            var responses = await Task.WhenAll(downloads);

            var responsesString = string.Join($"{Environment.NewLine}{new string('-', 100)}", responses);

            await File.WriteAllTextAsync(fileName, responsesString);
        }


        private static void AddCookiesAction(Request request, Response response)
        {

            if (request.Cookies.Any(c => c.Name != Session.SessionCookieName))
            {


                var cookieText = new StringBuilder();
                cookieText.AppendLine("<h1>Cookies:<h1");
                cookieText
                 .Append("<table border='1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in request.Cookies)
                {
                    cookieText.Append("<tr>");
                    cookieText
                        .Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookieText
                        .Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                    cookieText.Append("</tr>");
                }

                cookieText.Append("</table>");

                response.Body = cookieText.ToString();
            }
            else
            {
                response.Body = "<h1> Cookies set!</h1>";
            }

        }


        private static void DisplaySessionInfoAction(Request request, Response response)
        {
            var sessionExists = request.Session
             .Contains(Session.SessionCurrentDateKey);

            var bodyText = "";

            if (sessionExists)
            {
                var currentDate = request.Session[Session.SessionCurrentDateKey];
                bodyText = $"Stored date: {currentDate}!";
            }
            else
            {
                bodyText = "Current date stored!";
            }

            response.Body = "";
            response.Body += bodyText;
        }

        private static void LoginAction(Request request, Response response)
        {
            request.Session.Clear();

            var bodyText = "";

            var usernameMatches = request.Form["Username"] == Startup.Username;
            var passwordMatches = request.Form["Password"] == Startup.Password;

            if (usernameMatches && passwordMatches)
            {
                request.Session[Session.SessionUserKey] = "MyUserId";

                response.Cookies.Add(Session.SessionCookieName,
                    request.Session.Id);

                bodyText = "<h3>Logged successfully!</h3>";
            }
            else
            {
                bodyText = LoginForm.Html;
            }

            response.Body = "";
            response.Body += bodyText;
        }

        private static void LogoutAction(Request request, Response response)
        {
            request.Session.Clear();

            response.Body = "";
            response.Body += "<h3>Logged out successfully!</h3>";
        }

        private static void GetUserDataAction(Request request, Response response)
        {
            if (request.Session.Contains(Session.SessionUserKey))
            {
                response.Body = "";
                response.Body += $"<h3>Currently logged-in user " +
                    $"is with username '{Username}'</h3>";
            }
            else
            {
                response.Body = "";
                response.Body += "<h3>You should first log in " +
                    "- <a href='/Login'>Login</a></h3>";
            }
        }
    }
}
