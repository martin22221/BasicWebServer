using System.Text;

namespace BasicWebServer.Server.HTTP
{
    public class Response
    {
        public Response(StatusCode statusCode)
        {
            this.StatusCode = statusCode;

            this.Headers = new HeaderCollection();

           Headers.Add(new Header("Server", "BasicWebServer"));
           Headers.Add(new Header("Content-Type", "text/plain; charset=UTF-8"));
        }

        public StatusCode StatusCode { get; }

        public HeaderCollection Headers { get; }

        public string Body { get; set; }


        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append($"HTTP/1.1 {(int)StatusCode} {StatusCode}");

            foreach (var header in Headers)
            {
                result.Append($"{header.Name}:{header.Value}");
            }

            result.AppendLine();

            if (string.IsNullOrWhiteSpace(Body) ==false)
            {
                result.Append(Body);
            }

           return  result.ToString();
        }


    }
}
