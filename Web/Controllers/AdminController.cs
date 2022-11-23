using Data.DataTransferObjects.Request;
using Data.Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{

    [Route("[controller]")]
    public class AdminController : Controller
    {

        private readonly SignInManager<UserEntity> _signInManager;

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login(string email, string passwordl)
        {
            return View();
        }

        [HttpPost("login")]
        public IActionResult LoginPost(string email, string passwordl)
        {


            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet("register")]
        public IActionResult Register(string email, string passwordl)
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult RegisterPost(string firstName, string lastName, string email, string passwordl)
        {

            if (ModelState.IsValid)
            { 
            
            }
                return View("~/Views/Home/Index.cshtml");
        }
    }
}
