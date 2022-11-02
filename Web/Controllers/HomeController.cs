using System.Diagnostics;
using Business.Service.Abstract;
using Core.Results.Filter;
using Data.DataTransferObjects.Request;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    
    private readonly IAdvertService _advertService;
    public HomeController(IAdvertService advertService)
    {
        _advertService = advertService;
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
    
    [HttpGet("List")]
    public IActionResult List()
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
    [HttpPost("AdvertsPaginated")]
    public async Task<IActionResult> AdvertsPaginated([FromForm]SearchAdvertRequest model,[FromQuery] PaginationFilter filter, string orderBy)
    {
        var data = await _advertService.GetAdvertsPaginated(model,filter,orderBy);
        
        
        
        // return new ApiResponse(200, data.Data);
        return View("List",data);
    }
}