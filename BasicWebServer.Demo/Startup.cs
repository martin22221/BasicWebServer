using BasicWebServer.Server;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;
using BasicWebServer.Server.Views;

namespace BasicWebServer.Demo
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var server = new HttpServer(routes =>
            routes.MapGet("/HTML", new HtmlResponse(FormView.HTML))
            .MapGet("/redirect", new RedirectResponse("https://www.aboutyou.com"))
            .MapPost("/HTML", new TextResponse("",AddFormDataAction))    
            );

            server.Start();
        }
        private static void AddFormDataAction(Request request, Response response)
        {
            response.Body = "";
            foreach (var (key,value)in request.Form)
            {
                response.Body += $"{key} - {value}";
                response.Body += Environment.NewLine;

            }
        }
    }
}
