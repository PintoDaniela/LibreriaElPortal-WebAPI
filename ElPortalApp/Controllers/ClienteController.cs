using ElPortalApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ElPortalApp.Controllers
{
    public class ClienteController : Controller
    {
        private IClienteService _servicioCliente;

        public ClienteController(IClienteService clienteService)
        {
            _servicioCliente = clienteService;            
        }


        public async Task<IActionResult> Index()
        {
            var lista = await _servicioCliente.GetClientesAsync();
            return View(lista);
        }
    }
}
