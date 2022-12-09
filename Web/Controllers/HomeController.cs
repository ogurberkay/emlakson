using System.Diagnostics;
using Business.Service.Abstract;
using Core.Results.Filter;
using Data.DataTransferObjects.Request;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Data.Entities.DataTransferObjects.Response;
using X.PagedList;

namespace Web.Controllers;

public class HomeController : Controller
{
    
    private readonly IAdvertService _advertService;
    public HomeController(IAdvertService advertService)
    {
        _advertService = advertService;
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


    public IActionResult Index()
    {
        return View();
    }
    
    

    [HttpGet("Privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet("ContactUs")]
    public IActionResult ContactUs()
    {
        return View();
    }
    
    //[HttpGet("List")]
    //public async Task<IActionResult> List()
    //{
    //    var data = await _advertService.GetAllAdverts();
    //    var dataPaged = await data.Data.ToPagedListAsync(1,10);

    //    return View(dataPaged);
    //}
    
    [HttpGet("404")]
    public IActionResult Admin()
    {
        return View();
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
    
    
    [HttpGet("Gallery")]
    public IActionResult Gallery()
    {
        return View();
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
    public async Task<IActionResult> ListPaginated(SearchAdvertRequest model,[FromQuery] PaginationFilter filter, int? page, int orderBy = 1)
    {
    
        var data = await _advertService.GetAdvertsPaginated(model, filter, orderBy);
        
        if(data.Count > 0) { 
        data.Add(data[0]);
        data.Add(data[0]);
        data.Add(data[0]);
        data.Add(data[0]);
        data.Add(data[0]);
            data.Add(data[0]);
        data.Add(data[2]);
        data.Add(data[2]);
        data.Add(data[2]);
        data.Add(data[2]);
            data.Add(data[2]);
        data.Add(data[2]);
        data.Add(data[2]);
        data.Add(data[1]);
        data.Add(data[1]);
        data.Add(data[1]);
        data.Add(data[1]);
            data.Add(data[1]);
        data.Add(data[1]);
        }
        var dataPaged = await data.ToPagedListAsync(page ?? 1, 2);

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