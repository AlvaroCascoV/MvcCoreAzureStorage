using Microsoft.AspNetCore.Mvc;
using MvcCoreAzureStorage.Models;
using MvcCoreAzureStorage.Services;
using System.Threading.Tasks;

namespace MvcCoreAzureStorage.Controllers
{
    public class AzureTablesController : Controller
    {
        private ServiceStorageTables service;
        public AzureTablesController(ServiceStorageTables service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<Cliente> clientes = await this.service.GetClientesAsync();
            return View(clientes);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string empresa)
        {
            List<Cliente> clientes = await this.service.GetClientesEmpresaAsync(empresa);
            return View(clientes);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            await this.service.CreateClientAsync(cliente.IdCliente, cliente.Nombre, cliente.Empresa, cliente.Edad, cliente.Salario);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string partitionKey, string rowKey) //partitionKey: empresa, rowKey: idcliente
        {
            await this.service.DeleteClienteAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string partitionKey, string rowKey) //partitionKey: empresa, rowKey: idcliente
        {
            Cliente cliente = await this.service.FindClienteAsync(partitionKey, rowKey);
            return View(cliente);
        }
    }
}
