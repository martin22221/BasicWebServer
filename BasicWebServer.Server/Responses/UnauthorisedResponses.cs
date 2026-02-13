using BasicWebServer.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Server.Responses
{
    internal class UnauthorisedResponses : Response
    {
        public UnauthorisedResponses() : base(StatusCode.Unauthorized)
        { 
          
        }

    }
}
