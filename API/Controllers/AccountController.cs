using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTO;
using API.Errors;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        [HttpGet]

        public async Task<ActionResult<UserDto>>GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user =await  _userManager.FindByEmailAsync(email);

              return new UserDto
           {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
           };
            
        }

        [HttpGet("emailexists")]

        public async Task<ActionResult<bool>>CheckMailAsync([FromQuery] string mail)
        {
            return await _userManager.FindByEmailAsync(mail) != null;
        }

        [HttpGet("address")]

        public async Task<ActionResult<Address>>GetUserAdress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email); 
            
            var user =await  _userManager.FindByEmailAsync(email); 
            
            return user.Adress;
        }



        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> Login (LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user==null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);

            if(!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto{
                Email = loginDto.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };


        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
           var user = new AppUser
           {
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName,
            UserName = registerDto.Email
           };

           var result = await _userManager.CreateAsync(user,registerDto.Password);

           if(!result.Succeeded) return BadRequest(new ApiResponse(400));

           return new UserDto
           {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
           };

        }
    }
}