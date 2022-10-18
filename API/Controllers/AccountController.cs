using System.IdentityModel.Tokens.Jwt;
using Business.Misc;
using Core.Exceptions;
using Data.DataTransferObjects.Request;
using Data.Entities.DataTransferObjects.Response;
using Data.Entities.Models;
using Data.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly JwtHandler _jwtHandler;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<User> userManager, JwtHandler jwtHandler, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _jwtHandler = jwtHandler;
        _roleManager = roleManager;
    }

    [HttpPost("Registration")]
    public async Task<ApiResponse> RegisterUser([FromBody] UserRegistrationRequestDto model)
    {
        if (model is null || !ModelState.IsValid)
            return new ApiResponse(400);

        var user = new User()
        {
            Email = model.Email,
            UserName = model.Email,
        };

        
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return new ApiResponse(400, errors);
        }

        var role = await _roleManager.FindByNameAsync("ADMIN");
        
        await _userManager.AddToRoleAsync(user, role?.Name ?? "ADMIN");

        return new ApiResponse("Success",201);
    }
    
    [HttpPost("Login")]
    public async Task<ApiResponse> Login([FromBody] UserAuthenticationRequestDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return new ApiResponse(403, new {ErrorMessage = "Invalid Authentication"});
        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var claims = await _jwtHandler.GetClaims(user);
        var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new ApiResponse(200,result:new UserAuthenticationResponseDto {IsAuthSuccessful = true, Token = token});

    }
}