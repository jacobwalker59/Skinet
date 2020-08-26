using System.Security.Claims;
using System.Linq;
using System;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using AutoMapper;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
         private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
       

        ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDTO>> GetCurrentUser()
    {
        //need to evaluate all the things that makes a http context

        var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

        return new UserDTO
        {
            Displayname = user.DisplayName,
            Token = _tokenService.CreateToken(user),
            Email = user.Email
        };

    }


    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDTO>> GetUserAddress()
    {

        var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync(HttpContext.User);

        return _mapper.Map<Address,AddressDTO>(user.Address);

    }

    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO address)
    {


        var user = await _userManager.FindUserByClaimsPrincipleWithAddressAsync
        (HttpContext.User);

        user.Address = _mapper.Map<AddressDTO,Address>(address);

        var result = await _userManager.UpdateAsync(user);

        if(result.Succeeded) return Ok(_mapper.Map<Address,AddressDTO>(user.Address));

        return BadRequest("Problem updating the user");
    }








    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        if (user == null)
        {
            return Unauthorized(new ApiResponse(401));
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password,
        false);

        if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

        return new UserDTO
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            Displayname = user.DisplayName
        };

    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
    {
        if(CheckEmailExistsAsync(registerDTO.Email).Result.Value)
        {
             return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []
             {"Email Address is already in use" }
             });


        }

        var user = new AppUser
        {
            DisplayName = registerDTO.DisplayName,
            Email = registerDTO.Email,
            UserName = registerDTO.Email

            // have to assume that all these - display name, email and username are all properties
            // within Identity
        };

        var result = await _userManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new ApiResponse(400));
        }

        return new UserDTO
        {
            Displayname = user.DisplayName,
            Token = _tokenService.CreateToken(user),
            Email = user.Email
        };



    }
}
}