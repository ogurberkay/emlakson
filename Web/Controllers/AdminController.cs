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
using Data.Entities.Identity;
using Data.Enum;

namespace Web.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
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
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AdminController(
            UserManager<UserEntity> userManager,
            IUserStore<UserEntity> userStore,
            SignInManager<UserEntity> signInManager,
            ILogger<AdminController> logger,
            IAdvertService advertService, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _advertService = advertService;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Advert");
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login(string email, string passwordl)
        {
            return View();
        }

        [HttpGet("AboutUs")]
        public async Task<IActionResult> AboutUs()
        {
            var aboutUs = _unitOfWork.Repository<AboutUs>().FindBy().FirstOrDefault();
            if (aboutUs is null)
            {
                aboutUs = new AboutUs();
                aboutUs.Text =
                    @"﻿ Tunca Gayrimenkul, 2006 yılında kurulmuş bir emlak şirketidir. Türkiye'nin çeşitli şehirlerinde faaliyet gösteren şirketimiz, müşteri memnuniyetini ilke edinmiştir.

Marmara’nın her geçen gün gelişmekte olan Kocaeli İlinde Gayrimenkul Danışmanlığı alanında güvenli, objektif ve kaliteli bir şekilde işlemlerinizi sonuçlandırmaktan gurur duyarız. İzmit’te bulunmakta olan genel merkezimizde profesyonel kadromuz genç ve dinamik ekibimizke sorunlarınıza hızlı ve kaliteli çözüm sağlıyoruz.

Müşteri memnuniyetini sağlamak için, uzman ekibimiz müşterilerimize en iyi hizmeti vermeyi amaçlar. Özellikle satış, kiralama, değerleme ve danışmanlık alanlarında uzman olan ekibimiz, müşterilerimize en doğru ve güncel bilgileri sunarak, onların ihtiyaçlarını en iyi şekilde karşılamaya çalışır.

Tunca Gayrimenkul olarak, müşteri memnuniyetini ilke edinmiş ve sürekli olarak piyasayı takip eden bir emlak şirketiyiz. Uzman ekibimiz ile müşterilerimize en iyi hizmeti vermeyi hedefleyen şirketimiz, yeni inşaat projelerine yönelik olarak da en avantajlı seçenekleri sunmayı amaçlar.";
                await _unitOfWork.Repository<AboutUs>().InsertAsync(aboutUs);
                await _unitOfWork.SaveChangesAsync();
            }

            return View(aboutUs);
        }

        public class AboutUsModal
        {
            public int Id { get; set; }
            public string Text { get; set; }
        }

        [HttpGet("AddOrUpdateAboutUs")]
        public async Task<IActionResult> AboutUs([FromForm] AboutUsModal modal)
        {
            var isExists = true;
            var aboutUs = await _unitOfWork.Repository<AboutUs>().FindBy().FirstOrDefaultAsync();
            if (aboutUs is null)
            {
                isExists = false;
                aboutUs = new AboutUs();
            }

            aboutUs.Text = modal.Text;
            if (isExists)
                _unitOfWork.Repository<AboutUs>().Update(aboutUs);
            else
                await _unitOfWork.Repository<AboutUs>().InsertAsync(aboutUs);

            await _unitOfWork.SaveChangesAsync();

            return View(aboutUs);
        }


        [HttpGet("Customer")]
        public IActionResult Customer()
        {
            return View();
        }

        public class UserLoginDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginPost([FromForm] UserLoginDto modal)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var user = await _userManager.FindByEmailAsync(modal.Email);
                if (!user.IsApproved)
                    return BadRequest(new {message = "Kullanıcının yönetici tarafından onaylanması gerekmetedir."});

                var result =
                    await _signInManager.PasswordSignInAsync(modal.Email, modal.Password, false,
                        lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return Ok(new {response = "success"});
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    //Todo add lockout viewbag
                    return BadRequest(new {message = "Kullanıcı hesabı kilitlendi"});
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    //return Page();
                }
            }

            return BadRequest(new {message = "Kullanıcı adı veya şifre hatalı"});
        }

        public class RegisterModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpGet("register")]
        [AllowAnonymous]
        public IActionResult Register(string email, string password)
        {
            return View();
        }

        [HttpGet("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterPost([FromForm] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
                user.Name = model.FirstName;
                user.Surname = model.LastName;
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    _logger.LogInformation("User created a new account with password.");
                    var userId = await _userManager.GetUserIdAsync(user);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return Ok(new {success = true});
                }
            }

            return BadRequest(new {message = "Bu kullanıcı sisteme kayıtlı başka bir kullanıcı maili kullanınız"});
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
                images.ForEach(x => x.Size = x.Size != null ? Math.Round(x.Size ?? 0, 2) : null);
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

                user.IsApproved = true;

                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();

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

        [HttpPost("AddCustomerApi")]
        public async Task<IActionResult> AddCustomer([FromForm] Customer customer, List<string> ExtraAttributeIds)
        {
            try
            {
                customer.CustomerExtraAttributes = new Collection<CustomerExtraAttributes>();
                ExtraAttributeIds
                    .Select(x => Int32.Parse(x))
                    .ToList()
                    .ForEach(x =>
                    {
                        customer.CustomerExtraAttributes.Add(new CustomerExtraAttributes {ExtraAttributeId = x});
                    });

                var data = await _advertService.AddCustomer(customer);
                if (data.ResultStatus == ResultStatusEnum.Success)
                    return RedirectToAction("Customer");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        [HttpGet("GetCustomersApi")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = _advertService.GetAllCustomers().Result.Data;
            return Ok(new {data = customers, status = 200});
        }

        [HttpGet("GetCustomerByIdApi/{id}")]
        public async Task<IActionResult> GetCustomerByIdApi(int id)
        {
            var customer = _advertService.GetCustomerById(id).Result.Data;
            if (customer != null)
            {
                return Ok(new {data = customer, status = 200});
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("UpdateCustomerApi")]
        public async Task<IActionResult> UpdateCustomerApi([FromForm] Customer customer, List<string> ExtraAttributeIds)
        {
            try
            {
                customer.CustomerExtraAttributes = new Collection<CustomerExtraAttributes>();
                ExtraAttributeIds
                    .Select(x => Int32.Parse(x))
                    .ToList()
                    .ForEach(x =>
                    {
                        customer.CustomerExtraAttributes.Add(new CustomerExtraAttributes
                            {ExtraAttributeId = x, CustomerId = customer.Id});
                    });

                var existingCustomer = _advertService.UpdateCustomer(customer).Result.Data;

                if (existingCustomer != null)
                    return RedirectToAction("Customer");
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NotFound();
        }

        [HttpGet("EditCustomer")]
        public async Task<IActionResult> EditCustomer(int id)
        {
            try
            {
                var data = _advertService.GetCustomerById(id).Result.Data;
                ViewBag.Id = id;

                return PartialView("_CustomerEditPartial", data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NotFound();
        }

        [HttpPost("DeleteCustomerApi")]
        public async Task<IActionResult> DeleteCustomerApi(int id)
        {
            var result = _advertService.DeleteCustomer(id).Result.Data;
            if (result == true)
            {
                return Ok(new {status = 200});
            }
            else
            {
                return NotFound();
            }
        }
    }
}