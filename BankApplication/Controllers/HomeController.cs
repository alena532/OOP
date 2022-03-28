using BankApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BankApplication.Models;
using BankApplication.Models.Entity;
using BankApplication.ViewModel;
using BankApplication.Data;
using Microsoft.EntityFrameworkCore;
using BankApplication.ViewModel.Client;
using BankApplication.ViewModel.Account;

namespace BankApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //string userName = HttpContext.User.Identity.Name;
            //var userWithClient = _context.Users.Where(x => x.UserName == userName).Include(x => x.Clients).Single();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}