using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Task_API.Data;
using Task_API.DTO;
using Task_API.Models;
using Task_API.Validation;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IAccountValidation _validation;

        public AccountController(DataContext context, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IMapper mapper,IAccountValidation validation)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _validation = validation;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            if (userRegisterDto == null) { return BadRequest(); }
            //check userName Exist
            if (!await _validation.CheckUserNameExist(userRegisterDto.UserName))
            {
                return BadRequest(new
                {
                    msg = "user Exist please choose another name"
                });
            }
            //Check Email exist
            if (!await _validation.CheckEmailExist(userRegisterDto.Email))
            {
                return BadRequest(new
                {
                    msg = "Email Exist please choose another Email"
                });
            }
            //check is Email or not
            if (!_validation.isEmailValid(userRegisterDto.Email))
            {
                return BadRequest(new { msg = "please write correct email" });
            }
            //check password validation
            if (! _validation.ValidatePassword(userRegisterDto.Password))
            {
                return BadRequest(new { msg = "password must contain at least one lower case ,one upper case,one Number,at least one special character" });
            }
            var user = new AppUser
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email,
                City = userRegisterDto.City,
                Country = userRegisterDto.Country,
                Gender = userRegisterDto.Gender,
            };

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok(new
            {
                Data = _mapper.Map<UserReturnData>(user),
                msg = "register successfully"
            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            if (loginUserDto == null) { return BadRequest(); }
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            if (user == null) { return BadRequest(new { msg = "Email Not Found or incorrect " }); }
            var result = await _signInManager.PasswordSignInAsync(user, loginUserDto.Password, loginUserDto.RememberMe, false);
            if (!result.Succeeded)
            {
                return BadRequest(new { msg = " password not correct" });
            }
            AddCookies(user.UserName, user.Id, loginUserDto.RememberMe, user.Email);
            return Ok(new
            {
                msg = "login successfully"
            });
        }
        //Cookies
        private async void AddCookies(string username, string userId, bool remember, string email)
        {
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId),
               
            };

            var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);

            if (remember)
            {
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(10)

                };

                await HttpContext.SignInAsync
                (
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimIdentity),
                   authProperties
                );
            }
            else
            {
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync
                (
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimIdentity),
                   authProperties
                );
            }
        }
        [Authorize]
        [HttpGet("testAurhorize")]
        public string  Test()
        {

            return "thanks for registeration";
        }

    }
}
