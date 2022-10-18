using Business.Misc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AdminController : BaseController
{
    
    [HttpGet("AllowAnonymous")]
    [AllowAnonymous]
    public ApiResponse AllowAnonymous()
    {
        return new ApiResponse(400);
    }
    [HttpGet("Authorize")]
    [Authorize(Roles = "Admin")]
    public ApiResponse Authorize()
    {
        return new ApiResponse(400);
    }
    
    [HttpGet("Admin")]
    [Authorize]
    public ApiResponse Admin()
    {
        return new ApiResponse(400);
    }
    
    [HttpGet("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public ApiResponse SuperAdmin()
    {
        return new ApiResponse(400);
    }
}