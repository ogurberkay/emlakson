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
using Core.Extensions;
using Data.Enum;

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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;


        public AdminController(
            UserManager<UserEntity> userManager,
            IUserStore<UserEntity> userStore,
            SignInManager<UserEntity> signInManager,
            ILogger<AdminController> logger,
            IAdvertService advertService, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _advertService = advertService;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
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
                    return RedirectToAction("Index", "Home");
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

            return RedirectToAction("Index", "Home");
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

                    return RedirectToAction("Index", "Home");
                }
            }

            return RedirectToAction("Index", "Home");
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

        [HttpGet("Gallery")]
        public async Task<IActionResult> Gallery()
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

            return (IUserEmailStore<UserEntity>) _userStore;
        }

        [HttpGet("GetUsersApi")]
        public async Task<IActionResult> GetUsersApi(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                var users = await _userManager.Users.ToListAsync();
                return Ok(new {data = users, status = 200});
            }
        }

        [HttpPost("DeleteUserApi")]
        public async Task<IActionResult> DeleteUserApi(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user is not null)
                {
                    user.IsDeleted = false;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("User");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return NotFound();
        }


        [HttpPost("AddImageApi")]
        public async Task<IActionResult> AddImageApi([FromForm] GalleryImage modal)
        {
            try
            {
                // Calculate the size of the image in megabytes
                modal.Size = (decimal?) modal.ImageFile?.ImageFile?.Length / (1024 * 1024);
                
           
                if (modal.ImageFile is not null)
                {

                        var withoutExtension = Path.GetFileNameWithoutExtension(modal.ImageFile?.ImageFile?.FileName);
                        var uniqueFileName = StringExtensions.GetUniqueFileName(withoutExtension)
                                             + Path.GetExtension(modal.ImageFile?.ImageFile?.FileName);
                        modal.ImageName = withoutExtension;
                        var filePath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "img", uniqueFileName);

                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        await modal.ImageFile.ImageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

                        modal.ImageFile.ImageName = withoutExtension;
                        modal.ImageFile.ImageFile = modal.ImageFile?.ImageFile;
                        modal.ImageFile.ImagePath = filePath;
                    
                    
                    await _unitOfWork.Repository<GalleryImage>().InsertAsync(modal);

                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction("Gallery");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NotFound();
        }
        
        [HttpGet("SeeImage")]
        public async Task<IActionResult> SeeImage(int id)
        {
            try
            {
                var data = await _unitOfWork.Repository<GalleryImage>().FindBy(x => x.Id == id)
                    .Include(x => x.ImageFile)
                    .FirstOrDefaultAsync();
                ViewBag.Id = id;
                ViewBag.ImageName = data.ImageFile?.ImageName + '.' + data.ImageFile?.ImagePath?.Split('.')[^1];

                return PartialView("_GalleryImagePartial", data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NotFound();
        }

        // [HttpPost("EditImageApi")]
        // public async Task<IActionResult> EditImageApi(GalleryImage modal)
        // {
        //     try
        //     {
        //         var galleryImage = await _unitOfWork.Repository<GalleryImage>()
        //             .FindBy(x => modal.Id == x.Id)
        //             .FirstOrDefaultAsync();
        //         if (galleryImage is null)
        //             return NotFound();
        //
        //         if ((modal.ImageFile?.ImageFile is not null) && (modal.ImageFile.ImagePath is not null))
        //         {
        //             var withoutExtension = Path.GetFileNameWithoutExtension(modal.ImageFile?.ImageFile?.FileName);
        //             var uniqueFileName = StringExtensions.GetUniqueFileName(withoutExtension)
        //                                  + Path.GetExtension(modal.ImageFile?.ImageFile?.FileName);
        //
        //             var filePath = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "img", uniqueFileName);
        //
        //             Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        //
        //             await modal.ImageFile.ImageFile.CopyToAsync(new FileStream(filePath, FileMode.Create));
        //
        //             galleryImage.ImageFile.ImageName = withoutExtension;
        //             galleryImage.ImageFile.ImageFile = modal.ImageFile?.ImageFile;
        //             galleryImage.ImageFile.ImagePath = filePath;
        //
        //         }
        //
        //         //advert.AdvertDescription = model.AdvertDescription;
        //         _unitOfWork.Repository<GalleryImage>().Update(galleryImage);
        //         await _unitOfWork.SaveChangesAsync();
        //         return View("~/Views/Admin/Gallery.cshtml");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex);
        //     }
        //
        //     return View("~/Views/Admin/Gallery.cshtml");
        // }

        [HttpGet("GetImagesApi")]
        public async Task<IActionResult> GetImagesApi()
        {
            using (var context = new ApplicationDbContext())
            {
                var images = await _unitOfWork.Repository<GalleryImage>().FindBy().ToListAsync();
                images.ForEach(x => x.Size = x.Size != null ? Math.Round(x.Size ?? 0,2) : null);
                return Ok(new {data = images, status = 200});
            }
        }

        [HttpPost("DeleteImageApi")]
        public async Task<IActionResult> DeleteImageApi(int id)
        {
            try
            {
                var image = await _unitOfWork.Repository<GalleryImage>().FindBy(x => x.Id == id)
                    .FirstOrDefaultAsync();
                if (image is not null)
                {
                    _unitOfWork.Repository<GalleryImage>().HardDelete(image);
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction("Gallery");
                }
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

                return Ok(new {data = data, status = 200});
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

                return Ok(new {data = data, status = 200});
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
                ViewBag.ImageName = data.ImageFile?.ImageName + '.' + data.ImageFile?.ImagePath?.Split('.')[^1];

                return PartialView("_AdvertEditPartial", data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NotFound();
        }


        [HttpPost("DeleteAdvertApi")]
        public async Task<IActionResult> DeleteAdvertApi(int id)
        {
            try
            {
                var data = await _advertService.DeleteAdvert(id);
                if (data.ResultStatus == ResultStatusEnum.Success)
                    return RedirectToAction("Advert");
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
                    .ForEach(x =>
                    {
                        modal.AdvertExtraAttributes.Add(new AdvertExtraAttributes {ExtraAttributeId = x});
                    });
                var data = await _advertService.AddAdvert(modal);

                if (data.ResultStatus == ResultStatusEnum.Success)
                    return RedirectToAction("Advert");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NotFound();
        }

        [HttpPost("UpdateAdvertApi")]
        public async Task<IActionResult> UpdateAdvertApi(Advert modal, List<string> ExtraAttributeIds)
        {
            try
            {
                modal.AdvertExtraAttributes = new Collection<AdvertExtraAttributes>();
                ExtraAttributeIds
                    .Select(x => Int32.Parse(x))
                    .ToList()
                    .ForEach(x =>
                    {
                        modal.AdvertExtraAttributes.Add(new AdvertExtraAttributes
                            {ExtraAttributeId = x, AdvertId = modal.Id});
                    });

                var data = await _advertService.UpdateAdvert(modal);
                if (data.ResultStatus == ResultStatusEnum.Success)
                    return RedirectToAction("Advert");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NotFound();
        }
    }
}