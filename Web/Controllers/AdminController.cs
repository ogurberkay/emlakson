using Business.Extensions;
using Business.Misc;
using Business.Service.Abstract;
using Data.DataTransferObjects.Request;
using Data.Entities.Models;
using DataAccess.Context;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
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
        private readonly IAdvertService _advertService;



        public AdminController(
      UserManager<UserEntity> userManager,
      IUserStore<UserEntity> userStore,
      SignInManager<UserEntity> signInManager,
      ILogger<AdminController> logger,
      IAdvertService advertService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _advertService = advertService;

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
                var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);
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
            return View();
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
        public async Task<IActionResult> GetUsersApi(int Id)
        {

            using (var context = new ApplicationDbContext())
            {

                var users = await _userManager.Users.ToListAsync();
                return Ok(new { data = users, status = 200 });
            }
        }

        [HttpPost("DeleteUserApi")]
        public async Task<IActionResult> DeleteUserApi(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());

                user.IsDeleted = true;

                await _userManager.UpdateAsync(user);

                return View("~/Views/Admin/User.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return NotFound();
        }


        [HttpPost("ApproveUserApi")]
        public async Task<IActionResult> ApproveUserApi(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());

                user.EmailConfirmed = true;

                await _userManager.UpdateAsync(user);

                return View("~/Views/Admin/User.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return NotFound();
        }

        [HttpPost("EditUserApi")]
        public async Task<IActionResult> GetUsersApi(UserEntity modal)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(modal.Id.ToString());

                user.UserName = modal.UserName;
                user.Name = modal.Name;
                user.Surname = modal.Surname;
                user.Email = modal.Email;
                user.PhoneNumber = modal.PhoneNumber;

                await _userManager.UpdateAsync(user);

                return View("~/Views/Admin/User.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return NotFound();
        }


        [HttpGet("GetAdvertsApi")]
        public async Task<IActionResult> GetAdvertsApi()
        {
            try
            {
                var data = _advertService.GetAllAdverts().Result.Data;

                using (var db = new ApplicationDbContext())
                {
                    //data = db.ExtraAttributess
                }

                return Ok(new { data = data, status = 200 });
            }


            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return NotFound();
        }

        [HttpGet("GetAdvertById")]
        public async Task<IActionResult> GetAdvertById(int id)
        {
            try
            {
                var data = _advertService.GetAdvertById(id).Result.Data;

                return Ok(new { data = data, status = 200 });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return NotFound();
        }

        [HttpGet("EditAdvert")]
        public async Task<IActionResult> EditAdvert(int id)
        {
            try
            {
                var data = _advertService.GetAdvertById(id).Result.Data;
                ViewBag.Id = id;

                return PartialView("_AdvertEditPartial", data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return NotFound();
        }

        [HttpPost("DeleteAdvertApi")]
        public async Task<IActionResult> DeleteAdvertApi(List<int> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var data = await _advertService.DeleteAdvert(id);
                }
                return Ok();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return NotFound();
        }

        [HttpPost("AddAdvertApi")]
        public async Task<IActionResult> AddAdvertApi([FromForm] Advert modal, List<string> ExtraAttributeIds)
        {
            try
            {
                modal.AdvertExtraAttributes = new Collection<AdvertExtraAttributes>();
                ExtraAttributeIds
                    .Select(x => Int32.Parse(x))
                    .ToList()
                    .ForEach(x => {
                        modal.AdvertExtraAttributes.Add(new AdvertExtraAttributes { ExtraAttributeId = x });
                    });
                var data = _advertService.AddAdvert(modal).Result.Data;


                return Ok(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return NotFound();
        }

        [HttpPost("UpdateAdvertApi")]
        public async Task<IActionResult> UpdateAdvertApi(Advert modal,List<string> ExtraAttributeIds)
        {
            
            try
            {
                modal.AdvertExtraAttributes = new Collection<AdvertExtraAttributes>();
                ExtraAttributeIds
                    .Select(x => Int32.Parse(x))
                    .ToList()
                    .ForEach(x => {
                        modal.AdvertExtraAttributes.Add(new AdvertExtraAttributes { ExtraAttributeId = x,AdvertId=modal.Id });
                    });

                var data = _advertService.UpdateAdvert(modal).Result.Data;
                return Ok(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return NotFound();
        }


    }
}
