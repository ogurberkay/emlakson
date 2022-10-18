using System.Diagnostics;
using Business.Service.Abstract;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class HomeController : Controller
{
    private readonly  IAdvertService _advertService;
    
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger,IAdvertService advertService)
    {
        _advertService = advertService;
        _logger = logger;
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
    
    [HttpGet("ilanlar")]
    public async Task<IActionResult> List()
    {
        
        var data = await _advertService.GetAllAdverts();
        
        return View(data.Data);
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
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}