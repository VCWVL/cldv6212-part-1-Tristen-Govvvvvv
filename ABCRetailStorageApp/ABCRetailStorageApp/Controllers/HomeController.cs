using System.Diagnostics;
using ABCRetailStorageApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailStorageApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
