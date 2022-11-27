using Business.Misc;
using Data.DataTransferObjects.Request;
using Data.Entities.Models;
using DataAccess.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Web.Controllers
{

    [Route("[controller]")]
    public class AdminController : Controller
    {

        private readonly SignInManager<UserEntity> _signInManager;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IUserStore<UserEntity> _userStore;
        private readonly IUserEmailStore<UserEntity> _emailStore;
        private readonly ILogger<AdminController> _logger;



        public AdminController(
      UserManager<UserEntity> userManager,
      IUserStore<UserEntity> userStore,
      SignInManager<UserEntity> signInManager,
      ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
        }


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
        public async Task<IActionResult> LoginPost(string email, string password)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(email, password,false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return View("~/Views/Home/Index.cshtml");

                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    //Todo add lockout viewbag
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    //return Page();
                }
            }

            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet("register")]
        public IActionResult Register(string email, string password)
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterPost(string firstName, string lastName, string email, string password)
        {

            if (ModelState.IsValid)
            {
                var user = CreateUser();
               
                await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
                user.Name = firstName;
                user.Surname = lastName;
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    var userId = await _userManager.GetUserIdAsync(user);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return View("~/Views/Home/Index.cshtml");

                }


            }
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpGet("Advert")]
        public IActionResult Advert()
        {
            return View();
        }

        [HttpGet("User")]
        public async Task<IActionResult> User()
        {
            using (var context = new ApplicationDbContext())
            {

                var users = await _userManager.Users.ToListAsync();

                return View(users);

            }
        }

        private UserEntity CreateUser()
        {
            try
            {
                return Activator.CreateInstance<UserEntity>();
            }
            catch
            {
                throw new InvalidOperationException($"Error Occur, please contact with admin.");
            }
        }

        private IUserEmailStore<UserEntity> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Error Occur, please contact with admin.");
            }
            return (IUserEmailStore<UserEntity>)_userStore;
        }

        [HttpGet("GetUsersApi")]
        public async Task<ApiResponse> GetUsersApi(int Id)
        {

            using(var context = new ApplicationDbContext()) {

                var users= await _userManager.Users.ToListAsync();

                  return new ApiResponse(200, users);
            }
        }



    }
}
