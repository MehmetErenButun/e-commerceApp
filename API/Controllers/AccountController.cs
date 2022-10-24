using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTO;
using API.Errors;
using API.Extensions;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        [HttpGet]

        public async Task<ActionResult<UserDto>>GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaims(User);

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
        
        [Authorize]
        [HttpGet("address")]

        public async Task<ActionResult<AdressDto>>GetUserAdress()
        {
           var user = await _userManager.FindByUserByClaimsWithAdressAsyn(User);
            
            return _mapper.Map<Address,AdressDto>(user.Adress);
        }

        [Authorize]
        [HttpPut("address")]

        public async Task<ActionResult<AdressDto>> UpdateAdrres(AdressDto addressDto)
        {
            var user = await _userManager.FindByUserByClaimsWithAdressAsyn(User);

            user.Adress = _mapper.Map<AdressDto,Address>(addressDto);

            var result = await _userManager.UpdateAsync(user);

            if(result.Succeeded) return Ok(_mapper.Map<Address,AdressDto>(user.Adress));

            return BadRequest("Problem");

            
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

            if(CheckMailAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse{Errors = new []{"Email adresi zaten kullanılıyor"}});
            }

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