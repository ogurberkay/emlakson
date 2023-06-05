using System.Data.Entity;
using System.Diagnostics;
using Business.Service.Abstract;
using Core.Results.Filter;
using Data.DataTransferObjects.Request;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Data.Entities.DataTransferObjects.Response;
using Data.Entities.Models;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAdvertService _advertService;
    public HomeController(IAdvertService advertService, IUnitOfWork unitOfWork)
    {
        _advertService = advertService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("Asis")]
    public IActionResult Asis()
	{
        return View();
	}

    [HttpGet("Video")]
    public IActionResult Video()
    {
        return View();
    }


    public async Task<IActionResult> Index()
    {
         var data = await _advertService.GetAdvertsPaginated(new SearchAdvertRequest(), new PaginationFilter());
         if (data.Count < 12)
         {
             var i = data.Count;
             while(i< 12){
                 for (int j = 0; j < i; j++)
                 {
                     data.Add(data[j]);
                 }

                 i = data.Count;
             }
         }
         var dataPaged = await data.ToPagedListAsync(1, 12);

    
        return View("~/Views/Home/index.cshtml", dataPaged);

    }
    
    

    [HttpGet("Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet("ContactUs")]
    public async Task<IActionResult> ContactUs()
    {
        var aboutUs = _unitOfWork.Repository<AboutUs>().FindBy().FirstOrDefault();
        return View(aboutUs);
    }
    
    
    
    
    [HttpGet("404")]
    public IActionResult Admin()
    {
        return View("~/Views/Admin/Index.cshtml");
    }
    [HttpGet("404")]
    public IActionResult NotFound()
    {
        return View();
    }



    [HttpGet("kayar")]
    public IActionResult kayar()
    {
        return View();
    }

    [HttpGet("ContactUsNew")]
    public IActionResult ContactUsNew()
    {
        return View();
    }
    
    [HttpPost("ContactUsNew")]
    public IActionResult ContactUsNew([FromForm]ContactUs contactUs)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _unitOfWork.Repository<ContactUs>().Insert(contactUs);
                _unitOfWork.SaveChangesAsync();
                return RedirectToAction("ContactUsNew");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        return View(contactUs);
    }
    
    
    [HttpGet("Gallery")]
    public IActionResult Gallery()
    {
        try
        {

            var images = _unitOfWork.Repository<GalleryImage>().FindBy("ImageFile")
                .ToList();
            return View(images);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
    
    [HttpGet("Takim")]
    public IActionResult Takım()
    {
        return View();
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
    [HttpGet("Home/List")]
    public async Task<IActionResult> ListPaginated(SearchAdvertRequest model,[FromQuery] PaginationFilter filter, int? page)
    {
    
        var data = await _advertService.GetAdvertsPaginated(model, filter);
        
        var dataPaged = await data.ToPagedListAsync(page ?? 1, 10);

        ViewBag.SearhResult = dataPaged.Count;
        ViewBag.SearchKeyWord = model.SearchKeyWord ?? "";
        ViewBag.Location = model.Location ?? "";
        ViewBag.AdvertType = model.AdvertType ?? "";
        ViewBag.BedroomNumber = model.BedroomNumber ?? -1;
        ViewBag.BathroomNumber = model.BathroomNumber ?? -1;
        ViewBag.ExtraAttributes = model.ExtraAttributes ?? "";
        ViewBag.PricesStartValue = model.PricesStartValue;
        ViewBag.PricesEndValue = model.PricesEndValue;
        ViewBag.MetresStartValue = model.MetresStartValue;
        ViewBag.MetresEndValue = model.MetresEndValue;


        // return new ApiResponse(200, data.Data);
        return View("~/Views/Home/List.cshtml", dataPaged);
    }
}