using BasicWebServer.Server.Common;
using System.Text;

namespace BasicWebServer.Server.HTTP
{
    public class ContentResponse : Response
    {
        public ContentResponse(string content, string contentType, Action<Request,Response> preRenderAction = null)
            : base(StatusCode.OK)
        {
            Guard.AgainstNull(content);
            Guard.AgainstNull(contentType);

            PreRenderAction = preRenderAction;  


            this.Body = content;

            this.Headers.Add(Header.ContentType, contentType);
            this.Headers.Add(Header.ContentLength, Encoding.UTF8.GetByteCount(content).ToString());
        }
    }
}


