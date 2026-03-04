using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Server.Views
{
    public  class LoginForm
    {
        public const string Html = @"<form action='/Login' method='POST'>
Username: <input type='text' name='Username'/>
Password: <input type='text' name='Password'/>
<input type='submit' value='Log In' />
</form>";
    
    }
}
