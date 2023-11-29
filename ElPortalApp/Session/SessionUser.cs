using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ElPortalApp.Session
{
    public class SessionUser
    {        
        public string UserName { get; set; }       
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
