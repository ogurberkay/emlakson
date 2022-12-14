using Business.Misc;
using Business.Service.Abstract;
using Core.Results.Filter;
using Data.DataTransferObjects.Request;
using Data.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers;

public class AdvertController : Controller
{
    private readonly IAdvertService _advertService;
    public AdvertController(IAdvertService advertService)
    {
        _advertService = advertService;
    }

    [HttpGet("GetAdverts")]
    public async Task<ApiResponse> GetAdverts()
    {
        var data = await _advertService.GetAllAdverts();
        if (data.Data == null)
            return new ApiResponse(400, "Invalid Operation");
        return new ApiResponse(200, data.Data);
    }

    [HttpPost("AddAdvert")]
    public async Task<ApiResponse> AddAdvert(Advert model)
    {
        var data = await _advertService.AddAdvert(model);
        if (data.Data == null)
            return new ApiResponse(400, "Invalid Operation");
        return new ApiResponse(200, result:data.Data);
    }
    
    [HttpPost("UpdateAdvert/{Id}")]
    public async Task<ApiResponse> UpdateAdvert(Advert model)
    {
        var data = await _advertService.UpdateAdvert(model);
        if (data.Data == null)
            return new ApiResponse(400, "Invalid Operation");
        return new ApiResponse(200, result:data.Data);
    }
    

    [HttpDelete("DeleteAdvert/{Id}")]
    public async Task<ApiResponse> DeleteAdvert(int Id)
    {
        var data = await _advertService.DeleteAdvert(Id);
        if (data.Data == false)
            return new ApiResponse(400, "Invalid Operation");
        return new ApiResponse(200, data.Data);
    }
    
    [HttpGet("GetAdvertById/{Id}")]
    public async Task<ApiResponse> GetAdvertById(int Id)
    {
        var data = await _advertService.GetAdvertById(Id);
        if (data.Data == null)
            return new ApiResponse(400, "Invalid Operation");
        return new ApiResponse(200, data.Data);
    }

    [HttpGet("GetAdvertsApi")]
    public async Task<ApiResponse> GetAdvertsApi(int Id)
    {
        var data = await _advertService.GetAllAdverts();
        if (data.Data == null)
            return new ApiResponse(400, "Invalid Operation");
        return new ApiResponse(200, data.Data);
    }

}