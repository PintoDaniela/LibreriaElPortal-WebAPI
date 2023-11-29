using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Shared.DTOs;
using System.Net.Http;
using System.Text;
using ElPortalApp.ViewModels;
using ElPortalApp.Session;

namespace ElPortalApp.Interfaces
{
    public interface IAccountService
    {
        Task<SessionUser> Login(LoginViewModel credencialesLogin);
    }
}
